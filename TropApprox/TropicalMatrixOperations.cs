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
                    var a = (Number.Real)matrix[i, j];
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


            Entity element;

            for(int i = 0; i < result.RowCount; i++) {
                for(int j = 0; j < result.ColumnCount; j++) {
                    for(int c = 0; c < matrixA.ColumnCount; c++) {
                        var a = matrixA[i, c];
                        var b = matrixB[c, j];
                        bool aNeg = Current.Algebra.Zero == a;
                        bool bNeg = Current.Algebra.Zero == b;

                        if(aNeg || bNeg) {
                            continue;
                        }
                        else {
                            sb.Append($"({a})*({b})+");
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
            Entity scalar;
            if(matrixA.IsScalar) {
                scalar = matrixA[0, 0];
                matrix = matrixB;
            }
            else if(matrixB.IsScalar) {
                scalar = matrixB[0, 0];
                matrix = matrixA;
            }
            else {
                throw new InvalidOperationException();
            }

            var result = MathS.ZeroMatrix(matrix.RowCount, matrix.ColumnCount);

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = matrix[i, j];
                    if(Current.Algebra.Zero == a) {
                        result = result.WithElement(i, j, a);
                    }
                    else {
                        result = result.WithElement(i, j, Current.Algebra.Calculate($"({a})*({scalar})"));
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
                    var a = matrixA[i, j];
                    var b = matrixB[i, j];
                    int aZero = Current.Algebra.Zero == a ? 1 : 0;
                    int bZero = Current.Algebra.Zero == b ? 2 : 0;
                    int isZero = aZero + bZero;

                    //string astr = a.ToString(CultureInfo.InvariantCulture);
                    //string bstr = b.ToString(CultureInfo.InvariantCulture);

                    switch(isZero) {
                        case 0:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Calculate($"({a})+({b})"));
                            break;
                        case 1:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Calculate($"({b})"));
                            break;
                        case 2:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Calculate($"({a})"));
                            break;
                        case 3:
                            result = result.WithElement(i, j, (Entity)Current.Algebra.Zero);
                            break;
                    }
                }
            }

            return result;
        }

        public static Entity tr(Entity.Matrix matrix) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            Entity result;
            StringBuilder sb = new() {
                Capacity = 100
            };


            for(int i = 0; i < matrix.RowCount; i++) {
                var a = matrix[i, i];

                if(Current.Algebra.Zero == a) {
                    continue;
                }

                sb.Append($"{a}+");
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

        private static void GetNextNPowersOfMatrix(ref List<Entity.Matrix> matrixPowers, int n, bool withIdentityMatrix = false) {
            if(n < 1) {
                throw new ArgumentException("N must be greater or equal to one");
            }

            //List<Entity.Matrix> result = 
            int firstIndex = withIdentityMatrix ? 1 : 0;

            matrixPowers.Capacity += n;

            var firstPowerMatrix = matrixPowers[firstIndex];
            var lastPowerMatrix = matrixPowers[matrixPowers.Count];

            var poweredMatrix = TropicalMatrixMultiplication(firstPowerMatrix, lastPowerMatrix);
            for(int i = 1; i <= n; i++) {
                matrixPowers.Add(poweredMatrix);
                poweredMatrix = i == n ? poweredMatrix : TropicalMatrixMultiplication(poweredMatrix, firstPowerMatrix);
            }
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

        public static Entity GetSpectralRadius(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            matrixPowers = GetNPowersOfMatrix(matrix, (int)matrix.ColumnCount);

            List<Entity> tracks = new() {
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
                    sb.Append($"({t})^(1/{i})+");
                }
                i++;
            }

            Entity result;
            if(sb.Length != 0) {
                sb.Length--;
                result = Current.Algebra.Calculate(sb.ToString());
            }
            else {
                result = Current.Algebra.Zero;
            }

            return result;
        }

        public static Entity GetSpectralRadius(Entity.Matrix matrix) => GetSpectralRadius(matrix, out _);

        public static Entity Tr(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            matrixPowers = GetNPowersOfMatrix(matrix, matrix.ColumnCount);

            List<Entity> tracks = new() {
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
                    sb.Append($"({t})+");
                }
            }

            Entity result;
            if(sb.Length != 0) {
                sb.Length--;
                result = Current.Algebra.Calculate(sb.ToString());
            }
            else {
                result = Current.Algebra.Zero;
            }

            //throw new NotImplementedException();
            return result;
        }

        public static Entity Tr(Entity.Matrix matrix) => Tr(matrix, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Entity _Tr, out IEnumerable<Entity.Matrix> matrixPowers) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Kleene star isn't defined. Use TryKleeneStar() or KleeneStarEnumerator()!");
            }

            _Tr = Tr(matrix, out matrixPowers);

            (matrixPowers as List<Entity.Matrix>)?.RemoveAt(matrixPowers.Count() - 1);
            (matrixPowers as List<Entity.Matrix>)?.Insert(0, GetIdentityMatrix(matrix.ColumnCount));

            if((Number.Real)_Tr > Current.Algebra.One) {
                throw new ArgumentException("Tr(matrix) must <= identity element (1)");
            }

            var result = (matrixPowers as List<Entity.Matrix>)?[0];

            for(int i = 1; i < (matrixPowers as List<Entity.Matrix>)?.Count; i++) {
                result = TropicalMatrixAddition(result, (matrixPowers as List<Entity.Matrix>)?[i]);
            }

            //throw new NotImplementedException();
            return result;
        }

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix) => KleeneStar(matrix, out _, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Entity _Tr) => KleeneStar(matrix, out _Tr, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) => KleeneStar(matrix, out _, out matrixPowers);

        public static Entity.Matrix TryKleeneStar(Entity.Matrix matrix, out Entity? _Tr, out IEnumerable<Entity.Matrix> matrixPowers, int k = -1) {
            k = k < 0 ? matrix.ColumnCount - 1 : k;
            List<Entity.Matrix> matrixPowersList;
            if(!matrix.IsSquare) {
                _Tr = Tr(matrix, out matrixPowers);
                matrixPowersList = matrixPowers.ToList();
                matrixPowersList.Insert(0, GetIdentityMatrix(matrix.ColumnCount));

                if((Number.Real)_Tr > Current.Algebra.One) {
                    var c = matrixPowersList.Count - 1;
                    if(c < k) {
                        GetNextNPowersOfMatrix(ref matrixPowersList, k - c, true);
                    }
                    else if(c > k) {
                        matrixPowersList.RemoveRange(k, c - k);
                    }
                }
                else {
                    matrixPowersList.RemoveAt(matrixPowersList.Count - 1);
                }
            }
            else {
                _Tr = null;
                matrixPowersList = GetNPowersOfMatrix(matrix, k, true).ToList();
            }

            var result = matrixPowersList[0];

            for(int i = 1; i < matrixPowersList.Count; i++) {
                result = TropicalMatrixAddition(result, matrixPowersList[i]);
            }

            matrixPowers = matrixPowersList;

            return result;
        }

        public static Entity.Matrix TryKleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, int k = -1)
            => TryKleeneStar(matrix, out _, out matrixPowers, k);

        public static Entity.Matrix TryKleeneStar(Entity.Matrix matrix, out Entity _Tr, int k = -1)
            => TryKleeneStar(matrix, out _Tr, out _, k);

        public static Entity.Matrix TryKleeneStar(Entity.Matrix matrix, int k = -1)
            => TryKleeneStar(matrix, out _, out _, k);

        public static IEnumerable<Entity.Matrix> KleeneStarEnumerator(Entity.Matrix matrix) {
            Entity.Matrix result = GetIdentityMatrix(matrix.ColumnCount);
            Entity.Matrix power = GetIdentityMatrix(matrix.ColumnCount);
            Entity.Matrix prev = result;
            while(true) {
                yield return result;

                prev = result;
                power = TropicalMatrixMultiplication(power, matrix);
                result = TropicalMatrixAddition(result, power);

                if(prev == result) {
                    break;
                }
            }
        }
    }
}
