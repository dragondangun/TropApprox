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

        #region Pseudoinverse
        public static Entity.Matrix PseudoInverse(Entity.Matrix matrix, Algebra algebra) {
            matrix = matrix.T;

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = (Number.Real)matrix[i, j];
                    if(a == algebra.Zero) {
                        matrix = matrix.WithElement(i, j, algebra.Calculate($"{algebra.Zero}"));
                    }
                    else {
                        matrix = matrix.WithElement(i, j, algebra.Calculate($"({a})^(-1)"));
                    }
                }
            }

            return matrix;
        }

        #region Pseudoinverse overloading
        public static Entity.Matrix PseudoInverse(Entity.Matrix matrix) => PseudoInverse(matrix, Current.Algebra);

        #endregion
        #endregion

        #region Tropical Matrix Multiplication

        public static Entity.Matrix TropicalMatrixMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra) {
            if(matrixA.IsScalar || matrixB.IsScalar) {
                return TropicalMatrixScalarMultiplication(matrixA, matrixB, algebra);
            }

            if(matrixA.ColumnCount != matrixB.RowCount &&
                matrixA.RowCount != matrixB.ColumnCount) {
                throw new ArgumentException("Matrices don't fit each other. A.ColumnCount must be equal to B.RowCount AND A.RowCount must be equal to B.ColumnCount!");
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
                        bool aNeg = algebra.Zero == a;
                        bool bNeg = algebra.Zero == b;

                        if(aNeg || bNeg) {
                            continue;
                        }
                        else {
                            sb.Append($"({a})*({b})+");
                        }
                    }
                    if(sb.Length > 0) {
                        sb.Length--; // delete last "+" sign
                        element = algebra.Calculate(sb.ToString());
                        sb.Length = 0;
                        result = result.WithElement(i, j, element);
                    }
                    else {
                        result = result.WithElement(i, j, algebra.Zero);
                    }
                }
            }

            return result;
        }

        #region Tropical Matrix Multiplication overloading

        public static Entity.Matrix TropicalMatrixMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TropicalMatrixMultiplication(matrixA, matrixB, Current.Algebra);

        #endregion
        #endregion

        #region Tropical Matrix Scalar Multiplication

        public static Entity.Matrix TropicalMatrixScalarMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra) {
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
                throw new ArgumentException("None of matrices is scalar. Try TropicalMatrixMultiplication.");
            }

            var result = MathS.ZeroMatrix(matrix.RowCount, matrix.ColumnCount);

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = matrix[i, j];
                    if(algebra.Zero == a) {
                        result = result.WithElement(i, j, a);
                    }
                    else {
                        result = result.WithElement(i, j, algebra.Calculate($"({a})*({scalar})"));
                    }
                }
            }

            return result;
        }
        
        #region Tropical Matrix Scalar Multiplication overloading

        public static Entity.Matrix TropicalMatrixScalarMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TropicalMatrixScalarMultiplication(matrixA, matrixB, Current.Algebra);
        #endregion
        #endregion

        #region Tropical Matrix Addition

        public static Entity.Matrix TropicalMatrixAddition(Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra) {
            if(matrixA.ColumnCount != matrixB.ColumnCount &&
                matrixA.RowCount != matrixB.RowCount) {
                throw new ArgumentException("Matrices must be the same size");
            }

            var result = MathS.ZeroMatrix(matrixA.RowCount, matrixA.ColumnCount);

            for(int i = 0; i < result.RowCount; i++) {
                for(int j = 0; j < result.ColumnCount; j++) {
                    var a = matrixA[i, j];
                    var b = matrixB[i, j];
                    int aZero = algebra.Zero == a ? 1 : 0;
                    int bZero = algebra.Zero == b ? 2 : 0;
                    int isZero = aZero + bZero;

                    switch(isZero) {
                        case 0:
                            result = result.WithElement(i, j, (Entity)algebra.Calculate($"({a})+({b})"));
                            break;
                        case 1:
                            result = result.WithElement(i, j, (Entity)algebra.Calculate($"({b})"));
                            break;
                        case 2:
                            result = result.WithElement(i, j, (Entity)algebra.Calculate($"({a})"));
                            break;
                        case 3:
                            result = result.WithElement(i, j, (Entity)algebra.Zero);
                            break;
                    }
                }
            }

            return result;
        }
        
        #region Tropical Matrix Addition overloading

        public static Entity.Matrix TropicalMatrixAddition(Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TropicalMatrixAddition(matrixA, matrixB, Current.Algebra);
        #endregion
        #endregion

        #region tr

        public static Number.Real tr(Entity.Matrix matrix, Algebra algebra) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            Number.Real result;
            StringBuilder sb = new() {
                Capacity = 100
            };


            for(int i = 0; i < matrix.RowCount; i++) {
                var a = matrix[i, i];

                if(algebra.Zero == a) {
                    continue;
                }

                sb.Append($"{a}+");
            }

            if(sb.Length == 0) {
                result = algebra.Zero;
            }
            else {
                --sb.Length;
                result = (Number.Real)algebra.Calculate(sb.ToString());
            }

            return result;
        }
        
        #region tr overloading

        public static Number.Real tr(Entity.Matrix matrix) => tr(matrix, Current.Algebra);
        #endregion
        #endregion

        #region Get N Powers of Matrix
        public static IEnumerable<Entity.Matrix> GetNPowersOfMatrix(Entity.Matrix matrix, int n, Algebra algebra, bool withIdentityMatrix = false) {
            if(n < 1) {
                throw new ArgumentException("N must be greater or equal to one");
            }

            List<Entity.Matrix> result = new() {
                Capacity = withIdentityMatrix ? n + 1 : n,
            };

            if(withIdentityMatrix) {
                result.Add(GetIdentityMatrix(matrix.RowCount, algebra));
            }

            var poweredMatrix = matrix;

            for(int i = 1; i <= n; i++) {
                result.Add(poweredMatrix);
                poweredMatrix = i == n ? poweredMatrix : TropicalMatrixMultiplication(poweredMatrix, matrix, algebra);
            }

            return result;
        }

        #region Get N Powers of Matrix overloading

        public static IEnumerable<Entity.Matrix> GetNPowersOfMatrix(Entity.Matrix matrix, int n, bool withIdentityMatrix = false)
            => GetNPowersOfMatrix(matrix, n, Current.Algebra, withIdentityMatrix);
        #endregion
        #endregion

        #region Get Next N Powers of Matrix

        private static void GetNextNPowersOfMatrix(ref List<Entity.Matrix> matrixPowers, int n, Algebra algebra, bool withIdentityMatrix = false) {
            if(n < 1) {
                throw new ArgumentException("N must be greater or equal to one");
            }

            int firstIndex = withIdentityMatrix ? 1 : 0;

            matrixPowers.Capacity += n;

            var firstPowerMatrix = matrixPowers[firstIndex];
            var lastPowerMatrix = matrixPowers[matrixPowers.Count];

            var poweredMatrix = TropicalMatrixMultiplication(firstPowerMatrix, lastPowerMatrix, algebra);
            for(int i = 1; i <= n; i++) {
                matrixPowers.Add(poweredMatrix);
                poweredMatrix = i == n ? poweredMatrix : TropicalMatrixMultiplication(poweredMatrix, firstPowerMatrix, algebra);
            }
        }

        #region Get Next N Powers of Matrix overloading

        private static void GetNextNPowersOfMatrix(ref List<Entity.Matrix> matrixPowers, int n, bool withIdentityMatrix = false)
            => GetNextNPowersOfMatrix(ref matrixPowers, n, Current.Algebra, withIdentityMatrix);
        #endregion
        #endregion

        #region Get Identity Matrix

        public static Entity.Matrix GetIdentityMatrix(int size, Algebra algebra) {
            if(size < 1) {
                throw new ArgumentException("Size must be greater or equal to one");
            }

            var result = MathS.ZeroMatrix(size);
            var tempColumn = MathS.ZeroVector(size);

            tempColumn = tempColumn.WithElement(0, algebra.One);
            for(int i = 1; i < size; i++) {
                tempColumn = tempColumn.WithElement(i, algebra.Zero);
            }

            for(int i = 0; i < size - 1; i++) {
                result = result.WithColumn(i, tempColumn);
                tempColumn = tempColumn.WithElement(i, algebra.Zero);
                tempColumn = tempColumn.WithElement(i + 1, algebra.One);
            }

            result = result.WithColumn(size - 1, tempColumn);

            return result;
        }

        #region Get Identity Matrix overloading

        public static Entity.Matrix GetIdentityMatrix(int size) => GetIdentityMatrix(size, Current.Algebra);
        #endregion
        #endregion

        #region Get Zero Matrix

        public static Entity.Matrix GetZeroMatrix(int size, Algebra algebra) {
            if(size < 1) {
                throw new ArgumentException("Size must be greater or equal to one");
            }

            var result = MathS.ZeroMatrix(size);
            var tempColumn = MathS.ZeroVector(size);

            for(int i = 0; i < size; i++) {
                tempColumn = tempColumn.WithElement(i, algebra.Zero);
            }

            for(int i = 0; i < size; i++) {
                result = result.WithColumn(i, tempColumn);
            }

            return result;
        }

        public static Entity.Matrix GetZeroMatrix(int rowCount, int columnCount, Algebra algebra) {
            if(rowCount < 1 || columnCount < 1) {
                throw new ArgumentException("Dimensions must be greater or equal to one");
            }

            var result = MathS.ZeroMatrix(rowCount, columnCount);
            var cond = rowCount > columnCount;
            var biggest = cond ? rowCount : columnCount;
            var smallest = cond ? columnCount : rowCount;

            var tempColumn = MathS.ZeroVector(biggest);

            for(int i = 0; i < biggest; i++) {
                tempColumn = tempColumn.WithElement(i, algebra.Zero);
            }

            for(int i = 0; i < smallest; i++) {
                result = result.WithColumn(i, tempColumn);
            }

            return result;
        }

        #region Get Zero Matrix overloading

        public static Entity.Matrix GetZeroMatrix(int size) => GetZeroMatrix(size, Current.Algebra);

        public static Entity.Matrix GetZeroMatrix(int rowCount, int columnCount)
            => GetZeroMatrix(rowCount, columnCount, Current.Algebra);
        #endregion
        #endregion

        #region Get Spectral Radius

        public static Entity GetSpectralRadius(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            matrixPowers = GetNPowersOfMatrix(matrix, (int)matrix.ColumnCount, algebra);

            List<Entity> tracks = new() {
                Capacity = matrix.ColumnCount,
            };

            foreach(var m in matrixPowers) {
                tracks.Add(tr(m, algebra));
            }

            StringBuilder sb = new() {
                Capacity = 100,
            };

            int i = 1;

            foreach(var t in tracks) {
                if(t != algebra.Zero) {
                    sb.Append($"({t})^(1/{i})+");
                }
                i++;
            }

            Entity result;
            if(sb.Length != 0) {
                sb.Length--;
                result = algebra.Calculate(sb.ToString());
            }
            else {
                result = algebra.Zero;
            }

            return result;
        }

        #region Get Spectral Radius overloadings

        public static Entity GetSpectralRadius(Entity.Matrix matrix, Algebra algebra) => GetSpectralRadius(matrix, out _, algebra);

        public static Entity GetSpectralRadius(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers)
            => GetSpectralRadius(matrix, out matrixPowers, Current.Algebra);

        public static Entity GetSpectralRadius(Entity.Matrix matrix) => GetSpectralRadius(matrix, out _);
        #endregion
        #endregion

        #region Tr

        public static Number.Real Tr(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Matrix must be square!");
            }

            matrixPowers = GetNPowersOfMatrix(matrix, matrix.ColumnCount, algebra);

            List<Number.Real> tracks = new() {
                Capacity = matrix.ColumnCount,
            };

            foreach(var m in matrixPowers) {
                tracks.Add(tr(m, algebra));
            }

            StringBuilder sb = new() {
                Capacity = 100,
            };

            foreach(var t in tracks) {
                if(t != algebra.Zero) {
                    sb.Append($"({t})+");
                }
            }

            Number.Real result;
            if(sb.Length != 0) {
                sb.Length--;
                result = (Number.Real)algebra.Calculate(sb.ToString());
            }
            else {
                result = algebra.Zero;
            }

            return result;
        }
        
        #region Tr overloadings

        public static Number.Real Tr(Entity.Matrix matrix, Algebra algebra) => Tr(matrix, out _, algebra);

        public static Number.Real Tr(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers)
            => Tr(matrix, out matrixPowers, Current.Algebra);

        public static Number.Real Tr(Entity.Matrix matrix) => Tr(matrix, out _);
        #endregion
        #endregion

        #region Kleene Star

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) {
            if(!matrix.IsSquare) {
                throw new ArgumentException("Kleene star isn't defined. Use TryKleeneStar() or KleeneStarEnumerator()!");
            }

            _Tr = Tr(matrix, out matrixPowers, algebra);

            (matrixPowers as List<Entity.Matrix>)?.RemoveAt(matrixPowers.Count() - 1);

            if((Number.Real)_Tr > algebra.One) {
                throw new ArgumentException("Tr(matrix) must <= identity element (1). Use TryKleeneStar() or KleeneStarEnumerator()!");
            }
            var idenity = GetIdentityMatrix(matrix.ColumnCount, algebra);
            var result = idenity;

            foreach(var m in matrixPowers) {
                result = TropicalMatrixAddition(result, m, algebra);
            }

            (matrixPowers as List<Entity.Matrix>)?.Insert(0, idenity);

            return result;
        }

        #region Kleene Star overloadings

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, Algebra algebra) => KleeneStar(matrix, out _, out _, algebra);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr, Algebra algebra) => KleeneStar(matrix, out _Tr, out _, algebra);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) 
            => KleeneStar(matrix, out _, out matrixPowers, algebra);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr, out IEnumerable<Entity.Matrix> matrixPowers)
            => KleeneStar(matrix, out _Tr, out matrixPowers, Current.Algebra);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix) => KleeneStar(matrix, out _, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr) => KleeneStar(matrix, out _Tr, out _);

        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) => KleeneStar(matrix, out _, out matrixPowers);
        #endregion

        #region Try Kleene Star

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra, int k = -1) {
            k = k < 0 ? matrix.ColumnCount - 1 : k;
            List<Entity.Matrix> matrixPowersList;
            if(!matrix.IsSquare) {
                _Tr = Tr(matrix, out matrixPowers, algebra);
                matrixPowersList = matrixPowers.ToList();
                matrixPowersList.Insert(0, GetIdentityMatrix(matrix.ColumnCount, algebra));

                if(_Tr > algebra.One) {
                    var c = matrixPowersList.Count - 1;
                    if(c < k) {
                        GetNextNPowersOfMatrix(ref matrixPowersList, k - c, algebra, true);
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
                matrixPowersList = GetNPowersOfMatrix(matrix, k, algebra, true).ToList();
            }

            var result = k > 0 ? matrixPowersList[0] : null;
            foreach(var m in matrixPowersList) {
                result = TropicalMatrixAddition(result, m, algebra);
            }

            matrixPowers = matrixPowersList;
            return result;
        }
        
        #region Try Kleene Star overloadings

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra, int k = -1)
            => TryKleeneStar(matrix, out _, out matrixPowers, algebra, k);

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, Algebra algebra, int k = -1)
            => TryKleeneStar(matrix, out _Tr, out _, algebra, k);

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, Algebra algebra, int k = -1)
            => TryKleeneStar(matrix, out _, out _, algebra, k);

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, out IEnumerable<Entity.Matrix> matrixPowers, int k = -1)
            => TryKleeneStar(matrix, out _Tr, out matrixPowers, Current.Algebra, k);

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, int k = -1)
            => TryKleeneStar(matrix, out _, out matrixPowers, k);

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, int k = -1)
            => TryKleeneStar(matrix, out _Tr, out _, k);

        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, int k = -1)
            => TryKleeneStar(matrix, out _, out _, k);

        #endregion
        #endregion
        
        #region Kleene Star Enumerator

        public static IEnumerable<Entity.Matrix> KleeneStarEnumerator(Entity.Matrix matrix, Algebra algebra) {
            Entity.Matrix result = GetIdentityMatrix(matrix.ColumnCount, algebra);
            Entity.Matrix power = GetIdentityMatrix(matrix.ColumnCount, algebra);
            Entity.Matrix prev;
            while(true) {
                yield return result;

                prev = result;
                power = TropicalMatrixMultiplication(power, matrix, algebra);
                result = TropicalMatrixAddition(result, power, algebra);

                if(prev == result) {
                    break;
                }
            }
        }

        #region Kleene Star Enumerator overloading

        public static IEnumerable<Entity.Matrix> KleeneStarEnumerator(Entity.Matrix matrix) => KleeneStarEnumerator(matrix, Current.Algebra);

        #endregion
        #endregion
        #endregion
    }
}
