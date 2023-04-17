using AngouriMath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;

namespace TropApprox {
    public static class TropicalMatrixOperations {
        public static Entity.Matrix PseudoInverse(Entity.Matrix matrix) {
            matrix = matrix.T;

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = (double)(Number.Real)matrix[i, j];
                    var astr = a.ToString(CultureInfo.InvariantCulture);
                    if(a == Current.Algebra.Zero) {
                        matrix = matrix.WithElement(i, j, Current.Algebra.Calculate($"{Current.Algebra.Zero}"));
                    }
                    else {
                        matrix = matrix.WithElement(i, j, Current.Algebra.Calculate($"({a})^(-1)"));
                    }
                }
            }

            return matrix;
        }

        public static Entity.Matrix TropicalMatrixMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB) {
            if(matrixA.IsScalar || matrixB.IsScalar) {
                return TropicalMatrixScalarMultiplication(matrixA, matrixB);
            }

            if(matrixA.ColumnCount != matrixB.RowCount &&
                matrixA.RowCount != matrixB.ColumnCount) {
                throw new InvalidOperationException();
            }

            var result = MathS.ZeroMatrix(matrixA.RowCount, matrixB.ColumnCount);

            StringBuilder sb = new() {
                Capacity = 100
            };


            double element;

            for(int i = 0; i < result.RowCount; i++) {
                for(int j = 0; j < result.ColumnCount; j++) {
                    for(int c = 0; c < matrixA.ColumnCount; c++) {
                        double a = (double)((Number.Real)matrixA[i, c]);
                        double b = ((double)((Number.Real)matrixB[c, j]));
                        bool aNeg = Current.Algebra.Zero == a;
                        bool bNeg = Current.Algebra.Zero == b;

                        string astr = ((double)((Number.Real)matrixA[i, c])).ToString(CultureInfo.InvariantCulture);
                        string bstr = ((double)((Number.Real)matrixB[c, j])).ToString(CultureInfo.InvariantCulture);
                        if(aNeg || bNeg) {
                            continue;
                        }
                        else {
                            sb.Append($"({astr})*({bstr})+");
                        }
                    }
                    if(sb.Length > 0) {
                        sb.Length--; // delete last "+" sign
                        element = Current.Algebra.Calculate(sb.ToString());
                        sb.Length = 0;
                        result = result.WithElement(i, j, element);
                    }
                    else {
                        result = result.WithElement(i, j, Current.Algebra.Zero);
                    }
                }
            }

            return result;
        }

        public static Entity.Matrix TropicalMatrixScalarMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB) {
            Entity.Matrix matrix;
            string scalar;
            if(matrixA.IsScalar) {
                scalar = ((double)((Number.Real)matrixA[0, 0])).ToString(CultureInfo.InvariantCulture);
                matrix = matrixB;
            }
            else if(matrixB.IsScalar) {
                scalar = ((double)((Number.Real)matrixB[0, 0])).ToString(CultureInfo.InvariantCulture);
                matrix = matrixA;
            }
            else {
                throw new InvalidOperationException();
            }

            var result = MathS.ZeroMatrix(matrix.RowCount, matrix.ColumnCount);

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = (double)(Number.Real)matrix[i, j];
                    var astr = a.ToString(CultureInfo.InvariantCulture);
                    if(Current.Algebra.Zero == a) {
                        result = result.WithElement(i, j, a);
                    }
                    else {
                        result = result.WithElement(i, j, Current.Algebra.Calculate($"({astr})*({scalar})"));
                    }
                }
            }

            return result;
        }

        public static Entity.Matrix TropicalMatrixAddition(Entity.Matrix matrixA, Entity.Matrix matrixB) {
            if(matrixA.ColumnCount != matrixB.ColumnCount &&
                matrixA.RowCount != matrixB.RowCount) {
                throw new InvalidOperationException();
            }

            var result = MathS.ZeroMatrix(matrixA.RowCount, matrixA.ColumnCount);

            for(int i = 0; i < result.RowCount; i++) {
                for(int j = 0; j < result.ColumnCount; j++) {
                    double a = (double)((Number.Real)matrixA[i, j]);
                    double b = ((double)((Number.Real)matrixB[i, j]));
                    int aZero = Current.Algebra.Zero == a ? 1 : 0;
                    int bZero = Current.Algebra.Zero == b ? 2 : 0;
                    int isZero = aZero + bZero;

                    string astr = a.ToString(CultureInfo.InvariantCulture);
                    string bstr = b.ToString(CultureInfo.InvariantCulture);

                    switch(isZero) {
                        case 0:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Calculate($"({astr})+({bstr})"));
                            break;
                        case 1:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Calculate($"({bstr})"));
                            break;
                        case 2:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Calculate($"({astr})"));
                            break;
                        case 3:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Zero);
                            break;
                    }
                }
            }

            return result;
        }

        public static double tr(Entity.Matrix matrix) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            double result;
            StringBuilder sb = new() {
                Capacity = 100
            };


            for(int i = 0; i < matrix.RowCount; i++) {
                double a = (double)((Number.Real)matrix[i, i]);

                if(Current.Algebra.Zero == a) {
                    continue;
                }

                sb.Append($"{a.ToString(CultureInfo.InvariantCulture)}+");
            }

            if(sb.Length == 0) {
                result = Current.Algebra.Zero;
            }
            else {
                --sb.Length;
                result = Current.Algebra.Calculate(sb.ToString());
            }

            return result;
        }

        public static IEnumerable<Entity.Matrix> GetNPowersOfMatrix(Entity.Matrix matrix, int n, bool withIdentityMatrix = false) {
            if(n < 1) {
                throw new ArgumentException("N must be greater or equal to one");
            }

            List<Entity.Matrix> result = new() {
                Capacity = withIdentityMatrix ? n+1 : n,
            };

            if(withIdentityMatrix) {
                result.Add(GetIdentityMatrix(matrix.RowCount));
            }

            var poweredMatrix = matrix;

            for(int i = 1; i <= n; i++) {
                result.Add(poweredMatrix);
                poweredMatrix = i == n ? poweredMatrix : TropicalMatrixMultiplication(poweredMatrix, matrix);
            }

            return result;
        }

        public static Entity.Matrix GetIdentityMatrix(int size) {
            if(size < 1) {
                throw new ArgumentException("Size must be greater or equal to one");
            }

            var result = MathS.ZeroMatrix(size);
            var tempColumn = MathS.ZeroVector(size);

            tempColumn = tempColumn.WithElement(0, Current.Algebra.One);
            for(int i = 1; i < size; i++) {
                tempColumn = tempColumn.WithElement(i, Current.Algebra.Zero);
            }

            for(int i = 0; i < size-1; i++) {
                result = result.WithColumn(i, tempColumn);
                tempColumn = tempColumn.WithElement(i, Current.Algebra.Zero);
                tempColumn = tempColumn.WithElement(i+1, Current.Algebra.One);
            }

            result = result.WithColumn(size-1, tempColumn);

            return result;
        }

        public static double GetSpectralRadius(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            matrixPowers = GetNPowersOfMatrix(matrix, (int)matrix.ColumnCount);

            List<double> tracks = new() {
                Capacity = matrix.ColumnCount,
            };

            foreach(var m in matrixPowers) {
                tracks.Add(tr(m));
            }

            StringBuilder sb = new() {
                Capacity = 100,
            };

            int i = 1;

            foreach(var t in tracks) {
                if(t != Current.Algebra.Zero) {
                    sb.Append($"({t.ToString(CultureInfo.InvariantCulture)})^(1/{i})+");
                }
                i++;
            }

            double result;
            if(sb.Length != 0) {
                sb.Length--;
                result = Current.Algebra.Calculate(sb.ToString());
            }
            else {
                result = Current.Algebra.Zero;
            }

            return result;
        }

        public static double GetSpectralRadius(Entity.Matrix matrix) => GetSpectralRadius(matrix, out _);

        public static double Tr(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            matrixPowers = GetNPowersOfMatrix(matrix, (int)matrix.ColumnCount);

            List<double> tracks = new() {
                Capacity = matrix.ColumnCount,
            };

            foreach(var m in matrixPowers) {
                tracks.Add(tr(m));
            }

            StringBuilder sb = new() {
                Capacity = 100,
            };

            foreach(var t in tracks) {
                if(t != Current.Algebra.Zero) {
                    sb.Append($"({t.ToString(CultureInfo.InvariantCulture)})+");
                }
            }

            double result;
            if(sb.Length != 0) {
                sb.Length--;
                result = Current.Algebra.Calculate(sb.ToString());
            }
            else {
                result = Current.Algebra.Zero;
            }

            return result;
        }

        public static double Tr(Entity.Matrix matrix) => Tr(matrix, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out double _Tr, out IEnumerable<Entity.Matrix> matrixPowers) {
            _Tr = Tr(matrix, out matrixPowers);

            (matrixPowers as List<Entity.Matrix>)?.RemoveAt(matrixPowers.Count() - 1);
            (matrixPowers as List<Entity.Matrix>)?.Insert(0, GetIdentityMatrix(matrix.ColumnCount));

            if(_Tr > Current.Algebra.One) {
                throw new ArgumentException("Tr(matrix) must <= identity element (1)");
            }

            var result = (matrixPowers as List<Entity.Matrix>)?[0];

            for(int i = 1; i < (matrixPowers as List<Entity.Matrix>)?.Count; i++) {
                result = TropicalMatrixAddition(result, (matrixPowers as List<Entity.Matrix>)?[i]);
            }

            return result;
        }

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix) => KleeneStar(matrix, out _, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out double _Tr) => KleeneStar(matrix, out _Tr, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) => KleeneStar(matrix, out _, out matrixPowers);
    }
}
