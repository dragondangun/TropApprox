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
using static AngouriMath.MathS;

namespace TropApprox {
    public static class TropicalMatrixOperations {

        #region Pseudoinverse
        /// <summary>
        /// Tropical Pseudo-inverse of <paramref name="matrix"/> (vector) in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="matrix">
        /// Matrix (vector) which you want to pseudo-inverse
        /// </param>
        /// <param name="algebra">
        /// Algerba in terms of which you want to pseudo-inverse <paramref name="matrix"/> (vector)
        /// </param>
        /// <returns>
        /// Pseudo-inversed matrix or vector
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> (or vector) can't be null!
        /// </exception>
        public static Entity.Matrix PseudoInverse(Entity.Matrix matrix, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            
            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

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
        /// <summary>
        /// Tropical Pseudo-inverse of <paramref name="matrix"/> (vector) in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="matrix">
        /// Matrix (vector) which you want to pseudo-inverse
        /// </param>
        /// <returns>
        /// Pseudo-inversed <paramref name="matrix"/> (vector)
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> (vector) can't be null!
        /// </exception>
        public static Entity.Matrix PseudoInverse(Entity.Matrix matrix) => PseudoInverse(matrix, Current.Algebra);

        #endregion
        #endregion

        #region Tropical Matrix Multiplication

        /// <summary>
        /// Multiplication of matrices <paramref name="matrixA"/> (or/and vectors) and <paramref name="matrixB"/> in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="matrixA">
        /// Left matrix (vector) in multiplication
        /// </param>
        /// <param name="matrixB">
        /// Right matrix (vector) in multiplication
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want calculate tropical matrix multiplication
        /// </param>
        /// <returns>
        /// Product of matrices <paramref name="matrixA"/> (or/and vectors) and <paramref name="matrixB"/> tropical multiplication
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Nor <paramref name="matrixA"/>, neither <paramref name="matrixB"/> can be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Matricies (vectors) <paramref name="matrixA"/> and <paramref name="matrixB"/> must be appropriate size
        /// </exception>
        public static Entity.Matrix TropicalMatrixMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);

            if(matrixA is null) {
                throw new ArgumentNullException(nameof(matrixA));
            }
            if(matrixB is null) {
                throw new ArgumentNullException(nameof(matrixB));
            }

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

        /// <summary>
        /// Multiplication of matrices <paramref name="matrixA"/> (or/and vectors) and <paramref name="matrixB"/> in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="matrixA">
        /// Left matrix (vector) in multiplication
        /// </param>
        /// <param name="matrixB">
        /// Right matrix (vector) in multiplication
        /// </param>
        /// <returns>
        /// Product of matrices <paramref name="matrixA"/> (or/and vectors) and <paramref name="matrixB"/> tropical multiplication
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Nor <paramref name="matrixA"/>, neither <paramref name="matrixB"/> can be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Matricies (vectors) <paramref name="matrixA"/> and <paramref name="matrixB"/> must be appropriate size
        /// </exception>
        public static Entity.Matrix TropicalMatrixMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TropicalMatrixMultiplication(matrixA, matrixB, Current.Algebra);

        #endregion
        #endregion

        #region Tropical Matrix Scalar Multiplication

        /// <summary>
        /// Multiplication of <paramref name="matrix"/> (vector) and <paramref name="scalar"/> in terms of tropical algebra <paramref name="algebra"/>
        /// </summary>
        /// <param name="matrix">
        /// Matrix (vector) which is multiplicated by <paramref name="scalar"/>
        /// </param>
        /// <param name="scalar">
        /// Scalar value which is multiplicated by <paramref name="matrix"/>
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want calculate tropical multiplication of a <paramref name="scalar"/> by a <paramref name="matrix"/>
        /// </param>
        /// <returns>
        /// Tropical product of <paramref name="matrix"/> by <paramref name="scalar"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static Entity.Matrix TropicalMatrixScalarMultiplication(Entity.Matrix matrix, Number.Real scalar, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }
            if(scalar == algebra.Zero) {
                return GetZeroMatrix(matrix.RowCount, matrix.ColumnCount, algebra);
            }

            Entity.Matrix result = MathS.ZeroMatrix(matrix.RowCount, matrix.ColumnCount);

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = matrix[i, j];
                    if(algebra.Zero == a) {
                        result = result.WithElement(i, j, a);
                    }
                    else {
                        var element = algebra.Calculate($"({a})*({scalar})");
                        result = result.WithElement(i, j, element);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Tropical multiplication of matrix (vector) and scalar in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="matrixA">
        /// Matrix (vector) or scalar in form of 1x1 matrix
        /// </param>
        /// <param name="matrixB">
        /// Matrix (vector) or scalar in form of 1x1 matrix
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want calculate tropical multiplication of a matrix by a scalar
        /// </param>
        /// <returns>
        /// Tropical product of matrix by scalar
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrixA"/> and <paramref name="matrixB"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrixA"/> and <paramref name="matrixB"/> are not scalar. One of them must be scalar.
        /// </exception>
        public static Entity.Matrix TropicalMatrixScalarMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix matrix;
            Number.Real scalar;
            
            if(matrixA is null) {
                throw new ArgumentNullException(nameof(matrixA));
            }
            if(matrixB is null) {
                throw new ArgumentNullException(nameof(matrixB));
            }

            if(matrixA.IsScalar) {
                scalar = (Number.Real)matrixA[0, 0];
                matrix = matrixB;
            }
            else if(matrixB.IsScalar) {
                scalar = (Number.Real)matrixB[0, 0];
                matrix = matrixA;
            }
            else {
                throw new ArgumentException("None of matrices is scalar. Try TropicalMatrixMultiplication.");
            }

            var result = scalar == algebra.Zero ?
                GetZeroMatrix(matrix.RowCount, matrix.ColumnCount, algebra) :
                TropicalMatrixScalarMultiplication(matrix, scalar, algebra);

            return result;
        }

        #region Tropical Matrix Scalar Multiplication overloading

        /// <summary>
        /// Multiplication of matrix (vector) and scalar in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="matrix">
        /// Matrix (vector) which is multiplicated by <paramref name="scalar"/>
        /// </param>
        /// <param name="scalar">
        /// Scalar value which is multiplicated by <paramref name="matrix"/>
        /// </param>
        /// <returns>
        /// Tropical product of <paramref name="matrix"/> by <paramref name="scalar"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static Entity.Matrix TropicalMatrixScalarMultiplication(Entity.Matrix matrix, Number.Real scalar)
            => TropicalMatrixScalarMultiplication(matrix, scalar, Current.Algebra);

        /// <summary>
        /// Tropical multiplication of matrix (vector) and scalar in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="matrixA">
        /// Matrix (vector) or scalar in form of 1x1 matrix
        /// </param>
        /// <param name="matrixB">
        /// Matrix (vector) or scalar in form of 1x1 matrix
        /// </param>
        /// <returns>
        /// Tropical product of matrix by scalar
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrixA"/> and <paramref name="matrixB"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrixA"/> and <paramref name="matrixB"/> are not scalar. One of them must be scalar.
        /// </exception>
        public static Entity.Matrix TropicalMatrixScalarMultiplication(Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TropicalMatrixScalarMultiplication(matrixA, matrixB, Current.Algebra);
        #endregion
        #endregion

        #region Tropical Matrix Addition

        /// <summary>
        /// Tropical addition of <paramref name="matrixA"/> and <paramref name="matrixB"/> in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="matrixA">
        /// Left addendum
        /// </param>
        /// <param name="matrixB">
        /// Right addendum
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want calculate tropical addition of two matrices
        /// </param>
        /// <returns>
        /// Tropical sum of two matrices
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrixA"/> and <paramref name="matrixB"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Matricies (vectors) <paramref name="matrixA"/> and <paramref name="matrixB"/> must be appropriate size
        /// </exception>
        public static Entity.Matrix TropicalMatrixAddition(Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);

            if(matrixA is null) {
                throw new ArgumentNullException(nameof(matrixA));
            }
            if(matrixB is null) {
                throw new ArgumentNullException(nameof(matrixB));
            }

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

        /// <summary>
        /// Tropical addition of <paramref name="matrixA"/> and <paramref name="matrixB"/> in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="matrixA">
        /// Left addendum
        /// </param>
        /// <param name="matrixB">
        /// Right addendum
        /// </param>
        /// <returns>
        /// Tropical sum of two matrices
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrixA"/> and <paramref name="matrixB"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Matricies (vectors) <paramref name="matrixA"/> and <paramref name="matrixB"/> must be appropriate size
        /// </exception>
        public static Entity.Matrix TropicalMatrixAddition(Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TropicalMatrixAddition(matrixA, matrixB, Current.Algebra);
        #endregion
        #endregion

        #region tr

        /// <summary>
        /// Tropical <paramref name="matrix"/> trace in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="matrix">
        /// The matrix for which you need to find the trace
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want find <paramref name="matrix"/> trace
        /// </param>
        /// <returns>
        /// Tropical trace of <paramref name="matrix"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> must be square
        /// </exception>
        public static Number.Real tr(Entity.Matrix matrix, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            
            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

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

        /// <summary>
        /// Tropical <paramref name="matrix"/> trace in terms of <see cref="Current.Algebra"/> a tropical algebra
        /// </summary>
        /// <param name="matrix">
        /// The matrix for which you need to find the trace
        /// </param>
        /// <returns>
        /// Tropical trace of <paramref name="matrix"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> must be square
        /// </exception>
        public static Number.Real tr(Entity.Matrix matrix) => tr(matrix, Current.Algebra);
        #endregion
        #endregion

        #region Get N Powers of Matrix
        /// <summary>
        /// Finds <paramref name="n"/> powers of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="matrix">
        /// The matrix <paramref name="n"/> powers of which need to find.
        /// </param>
        /// <param name="n">
        /// N is number of powers, must be >= 1
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which matrix powers are calculated
        /// </param>
        /// <param name="withIdentityMatrix">
        /// When <paramref name="withIdentityMatrix"/> is <see langword="true"/> first element will be identity matrix. <br/>
        /// When <paramref name="withIdentityMatrix"/> is <see langword="false"/> first element will be <paramref name="matrix"/>.  <br/>
        /// <paramref name="withIdentityMatrix"/> is false by default.
        /// </param>
        /// <returns>
        /// Returns <see langword="IEnumerable&lt;Entity.Matrix&gt;"/> 
        /// with <paramref name="n"/> powers of <paramref name="matrix"/> 
        /// in terms of tropic <paramref name="algebra"/> (or <paramref name="n"/>+1
        /// if <paramref name="withIdentityMatrix"/> is true, than first element will be
        /// identity martrix)
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="withIdentityMatrix"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="n"/> must be >= 1
        /// </exception>
        public static IEnumerable<Entity.Matrix> GetNPowersOfMatrix(Entity.Matrix matrix, int n, Algebra algebra, bool withIdentityMatrix = false) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            
            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

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

        /// <summary>
        /// Finds <paramref name="n"/> powers of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="matrix">
        /// The matrix <paramref name="n"/> powers of which need to find.
        /// </param>
        /// <param name="n">
        /// N is number of powers, must be >= 1
        /// </param>
        /// <param name="withIdentityMatrix">
        /// When <paramref name="withIdentityMatrix"/> is <see langword="true"/> first element will be identity matrix. <br/>
        /// When <paramref name="withIdentityMatrix"/> is <see langword="false"/> first element will be <paramref name="matrix"/>.  <br/>
        /// <paramref name="withIdentityMatrix"/> is false by default.
        /// </param>
        /// <returns>
        /// Returns <see langword="IEnumerable&lt;Entity.Matrix&gt;"/> 
        /// with <paramref name="n"/> powers of <paramref name="matrix"/> 
        /// in terms of tropic <paramref name="algebra"/> (or <paramref name="n"/>+1
        /// if <paramref name="withIdentityMatrix"/> is true, than first element will be
        /// identity martrix)
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="withIdentityMatrix"/> can't be null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="n"/> must be >= 1
        /// </exception>
        public static IEnumerable<Entity.Matrix> GetNPowersOfMatrix(Entity.Matrix matrix, int n, bool withIdentityMatrix = false)
            => GetNPowersOfMatrix(matrix, n, Current.Algebra, withIdentityMatrix);
        #endregion
        #endregion

        #region Get Next N Powers of Matrix

        /// <summary>
        /// Add next <paramref name="n"/> powers of matrix to <paramref name="matrixPowers"/>. <br/> 
        /// Powers are calculated in terms of tropical <paramref name="algebra"/>. <br/>
        /// First element of list <paramref name="matrixPowers"/> is first power of matrix and 
        /// last element is the largest power of matrix. <br/>
        /// If <paramref name="withIdentityMatrix"/> is true than second element of list <paramref name="matrixPowers"/>
        /// is first power of matrix and first element of list is identity matrix.
        /// </summary>
        /// <remarks>
        /// For internal use only.
        /// </remarks>
        /// <param name="matrixPowers">
        /// List of matrix powers from 1 to K, where K is <paramref name="matrixPowers"/>.Count. <br/>
        /// If <paramref name="withIdentityMatrix"/> is <see langword="true"/> than <paramref name="matrixPowers"/> is list
        /// of matrix powers from 0 to K-1.
        /// </param>
        /// <param name="n">
        /// <paramref name="n"/> powers to add to list <paramref name="matrixPowers"/>. <br/>
        /// e.g. there are matrix powers from 1 to 4 in <paramref name="matrixPowers"/> list and <paramref name="n"/> is 2. <br/>
        /// This funtion will add 5th and 6th matrix powers to <paramref name="matrixPowers"/> list. 
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which matrix powers are calculated
        /// </param>
        /// <param name="withIdentityMatrix">
        /// If <paramref name="withIdentityMatrix"/> is <see langword="false"/> then first element of <paramref name="matrixPowers"/> list must be 1st power of the matrix. <br/>
        /// If <paramref name="withIdentityMatrix"/> is <see langword="true"/> then second element of <paramref name="matrixPowers"/> list must be 1st power of the matrix. <br/>
        /// <paramref name="withIdentityMatrix"/> is false by default.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="n"/> must be >= 1
        /// </exception>
        private static void GetNextNPowersOfMatrix(ref List<Entity.Matrix> matrixPowers, int n, Algebra algebra, bool withIdentityMatrix = false) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Add next <paramref name="n"/> powers of matrix to <paramref name="matrixPowers"/>. <br/> 
        /// Powers are calculated in terms of tropical <see cref="Current.Algebra"/>. <br/>
        /// First element of list <paramref name="matrixPowers"/> is first power of matrix and 
        /// last element is the largest power of matrix. <br/>
        /// If <paramref name="withIdentityMatrix"/> is true than second element of list <paramref name="matrixPowers"/>
        /// is first power of matrix and first element of list is identity matrix.
        /// </summary>
        /// <remarks>
        /// For internal use only.
        /// </remarks>
        /// <param name="matrixPowers">
        /// List of matrix powers from 1 to K, where K is <paramref name="matrixPowers"/>.Count. <br/>
        /// If <paramref name="withIdentityMatrix"/> is <see langword="true"/> than <paramref name="matrixPowers"/> is list
        /// of matrix powers from 0 to K-1.
        /// </param>
        /// <param name="n">
        /// <paramref name="n"/> powers to add to list <paramref name="matrixPowers"/>. <br/>
        /// e.g. there are matrix powers from 1 to 4 in <paramref name="matrixPowers"/> list and <paramref name="n"/> is 2. <br/>
        /// This funtion will add 5th and 6th matrix powers to <paramref name="matrixPowers"/> list. 
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which matrix powers are calculated
        /// </param>
        /// <param name="withIdentityMatrix">
        /// If <paramref name="withIdentityMatrix"/> is <see langword="false"/> then first element of <paramref name="matrixPowers"/> list must be 1st power of the matrix. <br/>
        /// If <paramref name="withIdentityMatrix"/> is <see langword="true"/> then second element of <paramref name="matrixPowers"/> list must be 1st power of the matrix. <br/>
        /// <paramref name="withIdentityMatrix"/> is false by default.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="n"/> must be >= 1
        /// </exception>
        private static void GetNextNPowersOfMatrix(ref List<Entity.Matrix> matrixPowers, int n, bool withIdentityMatrix = false)
            => GetNextNPowersOfMatrix(ref matrixPowers, n, Current.Algebra, withIdentityMatrix);
        #endregion
        #endregion

        #region Get Identity Matrix

        /// <summary>
        /// Get identity matrix with size of <paramref name="size"/> in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <remarks>
        /// See also <seealso cref="Algebra.One"/> and <seealso cref="Algebra.Zero"/>
        /// </remarks>
        /// <param name="size">
        /// Desired size of indentity matrix
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which identity matrix is defined
        /// </param>
        /// <returns>
        /// Returns indetity matrix with size of <paramref name="size"/> in terms of tropical <paramref name="algebra"/> 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="size"/> must be >= 1
        /// </exception>
        public static Entity.Matrix GetIdentityMatrix(int size, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Get identity matrix with size of <paramref name="size"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <remarks>
        /// See also <seealso cref="Algebra.One"/> and <seealso cref="Algebra.Zero"/>
        /// </remarks>
        /// <param name="size">
        /// Desired size of indentity matrix
        /// </param>
        /// <returns>
        /// Returns indetity matrix with size of <paramref name="size"/> in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="size"/> must be >= 1
        /// </exception>
        public static Entity.Matrix GetIdentityMatrix(int size) => GetIdentityMatrix(size, Current.Algebra);
        #endregion
        #endregion

        #region Get Zero Matrix

        /// <summary>
        /// Get zero matrix with size of <paramref name="size"/> in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <remarks>
        /// See also <seealso cref="Algebra.Zero"/>
        /// </remarks>
        /// <param name="size">
        /// Desired size of zero matrix
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which zero matrix is defined
        /// </param>
        /// <returns>
        /// Zero matrix with size of <paramref name="size"/> in terms of tropical <paramref name="algebra"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="size"/> must >= 1
        /// </exception>
        public static Entity.Matrix GetZeroMatrix(int size, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Get zero matrix with <paramref name="rowCount"/> rows and <paramref name="columnCount"/> columns in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <remarks>
        /// See also <seealso cref="Algebra.Zero"/>
        /// </remarks>
        /// <param name="rowCount">
        /// Desired row count
        /// </param>
        /// <param name="columnCount">
        /// Desired column count
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which zero matrix is defined
        /// </param>
        /// <returns>
        /// Zero matrix with <paramref name="rowCount"/> rows and <paramref name="columnCount"/> in terms of tropical <paramref name="algebra"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Dimensions <paramref name="rowCount"/> and <paramref name="columnCount"/> must >= 1
        /// </exception>
        public static Entity.Matrix GetZeroMatrix(int rowCount, int columnCount, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            if(rowCount < 1 || columnCount < 1) {
                throw new ArgumentException("Dimensions must be greater or equal to one");
            }

            var result = MathS.ZeroMatrix(rowCount, columnCount);
            var cond = rowCount > columnCount;
            var biggest = cond ? rowCount : columnCount;
            var smallest = cond ? columnCount : rowCount;

            var tempVector = MathS.ZeroVector(biggest);

            for(int i = 0; i < biggest; i++) {
                tempVector = tempVector.WithElement(i, algebra.Zero);
            }

            if(cond) {
                for(int i = 0; i < smallest; i++) {
                    result = result.WithColumn(i, tempVector);
                }
            }
            else {
                tempVector = tempVector.T;
                for(int i = 0; i < smallest; i++) {
                    result = result.WithRow(i, tempVector);
                }
            }

            return result;
        }

        #region Get Zero Matrix overloading

        /// <summary>
        /// Get zero matrix with size of <paramref name="size"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <remarks>
        /// See also <seealso cref="Algebra.Zero"/>
        /// </remarks>
        /// <param name="size">
        /// Desired size of zero matrix
        /// </param>
        /// <returns>
        /// Zero matrix with size of <paramref name="size"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="size"/> must >= 1
        /// </exception>
        public static Entity.Matrix GetZeroMatrix(int size) => GetZeroMatrix(size, Current.Algebra);

        /// <summary>
        /// Get zero matrix with <paramref name="rowCount"/> rows and <paramref name="columnCount"/> columns in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <remarks>
        /// See also <seealso cref="Algebra.Zero"/>
        /// </remarks>
        /// <param name="rowCount">
        /// Desired row count
        /// </param>
        /// <param name="columnCount">
        /// Desired column count
        /// </param>
        /// <returns>
        /// Zero matrix with <paramref name="rowCount"/> rows and <paramref name="columnCount"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Dimensions <paramref name="rowCount"/> and <paramref name="columnCount"/> must >= 1
        /// </exception>
        public static Entity.Matrix GetZeroMatrix(int rowCount, int columnCount)
            => GetZeroMatrix(rowCount, columnCount, Current.Algebra);
        #endregion
        #endregion

        #region Get Spectral Radius

        /// <summary>
        /// Get spectral radius of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix spectral radius of which you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of calculating spectral radius. <br/>
        /// You can use it if you want to.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which spectral radius is calculated.
        /// </param>
        /// <returns>
        /// Returns spectral radius of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static Number.Real GetSpectralRadius(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            
            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

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

        #region Get Spectral Radius overloadings

        /// <summary>
        /// Get spectral radius of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix spectral radius of which you want to find.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which spectral radius is calculated.
        /// </param>
        /// <returns>
        /// Returns spectral radius of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static Number.Real GetSpectralRadius(Entity.Matrix matrix, Algebra algebra) => GetSpectralRadius(matrix, out _, algebra);

        /// <summary>
        /// Get spectral radius of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <param name="matrix">
        /// The matrix spectral radius of which you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of calculating spectral radius. <br/>
        /// You can use it if you want to.
        /// </param>
        /// <returns>
        /// Returns spectral radius of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static Number.Real GetSpectralRadius(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers)
            => GetSpectralRadius(matrix, out matrixPowers, Current.Algebra);

        /// <summary>
        /// Get spectral radius of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <param name="matrix">
        /// The matrix spectral radius of which you want to find.
        /// </param>
        /// <returns>
        /// Returns spectral radius of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static Number.Real GetSpectralRadius(Entity.Matrix matrix) => GetSpectralRadius(matrix, out _);
        #endregion
        #endregion

        #region Tr

        /// <summary>
        /// Get Tr of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <remarks>
        /// Tr is tropical analogue for matrix determinant
        /// </remarks>
        /// <param name="matrix">
        /// The matrix Tr of wich you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of calculating Tr. <br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which Tr is calculated.
        /// </param>
        /// <returns>
        /// Tr of matrix in terms of tropical <paramref name="algebra"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        public static Number.Real Tr(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);

            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

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

        /// <summary>
        /// Get Tr of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <remarks>
        /// Tr is tropical analogue for matrix determinant
        /// </remarks>
        /// <param name="matrix">
        /// The matrix Tr of wich you want to find.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which Tr is calculated.
        /// </param>
        /// <returns>
        /// Tr of matrix in terms of tropical <paramref name="algebra"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        public static Number.Real Tr(Entity.Matrix matrix, Algebra algebra) => Tr(matrix, out _, algebra);

        /// <summary>
        /// Get Tr of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <remarks>
        /// Tr is tropical analogue for matrix determinant
        /// </remarks>
        /// <param name="matrix">
        /// The matrix Tr of wich you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of calculating Tr. <br/>
        /// You can use it if you want.
        /// </param>
        /// <returns>
        /// Tr of matrix in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        public static Number.Real Tr(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers)
            => Tr(matrix, out matrixPowers, Current.Algebra);

        /// <summary>
        /// Get Tr of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <remarks>
        /// Tr is tropical analogue for matrix determinant
        /// </remarks>
        /// <param name="matrix">
        /// The matrix Tr of wich you want to find.
        /// </param>
        /// <returns>
        /// Tr of matrix in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> must be square.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        public static Number.Real Tr(Entity.Matrix matrix) => Tr(matrix, out _);
        #endregion
        #endregion

        #region Kleene Star

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of tropical <paramref name="algebra"/></item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);

            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

            if(!matrix.IsSquare) {
                throw new ArgumentException("Kleene star isn't defined for non-square matrix");
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

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of tropical <paramref name="algebra"/></item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, Algebra algebra) => KleeneStar(matrix, out _, out _, algebra);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of tropical <paramref name="algebra"/></item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr, Algebra algebra) => KleeneStar(matrix, out _Tr, out _, algebra);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of tropical <paramref name="algebra"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of tropical <paramref name="algebra"/></item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra) 
            => KleeneStar(matrix, out _, out matrixPowers, algebra);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of <see cref="Current.Algebra"/> tropical algebra.</item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr, out IEnumerable<Entity.Matrix> matrixPowers)
            => KleeneStar(matrix, out _Tr, out matrixPowers, Current.Algebra);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of <see cref="Current.Algebra"/> tropical algebra.</item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix) => KleeneStar(matrix, out _, out _);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of <see cref="Current.Algebra"/> tropical algebra.</item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out Number.Real _Tr) => KleeneStar(matrix, out _Tr, out _);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>Kleene Star isn't defined for non-square matrix.</item>
        /// <item>Kleene Star isn't defined for matrix Tr of which is greater than identity element in terms of <see cref="Current.Algebra"/> tropical algebra.</item> 
        /// </list>
        /// </exception>
        public static Entity.Matrix KleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers) => KleeneStar(matrix, out _, out matrixPowers);
        #endregion

        #region Try Kleene Star

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/> or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra, int k = -1) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            
            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

            k = k < 0 ? matrix.ColumnCount - 1 : k;
            List<Entity.Matrix> matrixPowersList;
            if(matrix.IsSquare) {
                _Tr = Tr(matrix, out matrixPowers, algebra);
                matrixPowersList = matrixPowers.ToList();
                matrixPowersList.Insert(0, GetIdentityMatrix(matrix.ColumnCount, algebra));

                if(_Tr > algebra.One) {
                    var c = matrixPowersList.Count - 1;
                    if(c < k) {
                        GetNextNPowersOfMatrix(ref matrixPowersList, k - c, algebra, true);
                    }
                    else if(c > k) {
                        matrixPowersList.RemoveRange(k+1, c - k);
                    }
                }
                else {
                    matrixPowersList.RemoveAt(matrixPowersList.Count - 1);
                }
            }
            else {
                throw new ArgumentException("Kleene star isn't defined for non-square matrix.");
            }

            var result = GetZeroMatrix(matrix.RowCount, algebra);
            foreach(var m in matrixPowersList) {
                result = TropicalMatrixAddition(result, m, algebra);
            }

            matrixPowers = matrixPowersList;
            return result;
        }

        #region Try Kleene Star overloadings

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/> or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, Algebra algebra, int k = -1)
            => TryKleeneStar(matrix, out _, out matrixPowers, algebra, k);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/> or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, Algebra algebra, int k = -1)
            => TryKleeneStar(matrix, out _Tr, out _, algebra, k);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of tropical <paramref name="algebra"/> or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, Algebra algebra, int k = -1)
            => TryKleeneStar(matrix, out _, out _, algebra, k);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, out IEnumerable<Entity.Matrix> matrixPowers, int k = -1)
            => TryKleeneStar(matrix, out _Tr, out matrixPowers, Current.Algebra, k);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="matrixPowers">
        /// <see langword="IEnumerable"/> of matrix powers is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out IEnumerable<Entity.Matrix> matrixPowers, int k = -1)
            => TryKleeneStar(matrix, out _, out matrixPowers, k);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="_Tr">
        /// <see cref="Tr(Entity.Matrix, Algebra)"/> is by-product of finding Kleene Star.<br/>
        /// You can use it if you want.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, out Number.Real? _Tr, int k = -1)
            => TryKleeneStar(matrix, out _Tr, out _, k);

        /// <summary>
        /// Find Kleene Star of <paramref name="matrix"/> if it's defined in terms of <see cref="Current.Algebra"/> tropical algebra or return some step of it (see <paramref name="k"/>).
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="k">
        /// <paramref name="k"/>th step to return when Kleene star is not defined. <br/>
        /// If <paramref name="k"/> &lt; 0 then final step set to column count of <paramref name="matrix"/> - 1.
        /// If k = 0 then zero matrix will be returned. 
        /// </param>
        /// <returns>
        /// Kleene Star of <paramref name="matrix"/> in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Kleene Star isn't defined for non-square matrix.
        /// </exception>
        public static Entity.Matrix? TryKleeneStar(Entity.Matrix matrix, int k = -1)
            => TryKleeneStar(matrix, out _, out _, k);

        #endregion
        #endregion

        #region Kleene Star Enumerator

        /// <summary>
        /// Sequence of Kleene Stars steps in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which you want to find Kleene Star.
        /// </param>
        /// <returns>
        /// Return sequence of Kleene Stars steps in terms of tropical <paramref name="algebra"/>. <br/>
        /// Sequence breaks if next step equals to previous.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static IEnumerable<Entity.Matrix> KleeneStarEnumerator(Entity.Matrix matrix, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);

            if(matrix is null) {
                throw new ArgumentNullException(nameof(matrix));
            }

            Entity.Matrix result = GetIdentityMatrix(matrix.ColumnCount, algebra);
            Entity.Matrix power = result;
            Entity.Matrix prev;
            while(true) {
                yield return result;

                prev = result;
                power = TropicalMatrixMultiplication(power, matrix, algebra);
                result = TropicalMatrixAddition(result, power, algebra);

                if(prev.IsEqualTo(result)) {
                    yield break;
                }
            }
        }

        #region Kleene Star Enumerator overloading

        /// <summary>
        /// Sequence of Kleene Stars steps in terms of tropical <paramref name="algebra"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix Kleene Star of which you want to find.
        /// </param>
        /// <returns>
        /// Return sequence of Kleene Stars steps in terms of tropical <paramref name="algebra"/>. <br/>
        /// Sequence breaks if next step equals to previous.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> can't be null
        /// </exception>
        public static IEnumerable<Entity.Matrix> KleeneStarEnumerator(Entity.Matrix matrix) => KleeneStarEnumerator(matrix, Current.Algebra);

        #endregion
        #endregion
        #endregion

        #region Get Vector of Ones

        /// <summary>
        /// Get vector of <see cref="Algebra.One"/>s in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="size">
        /// Size of the vector
        /// </param>
        /// <param name="algebra">
        /// Tropical algebra in terms of which vector of ones must be returned.
        /// </param>
        /// <returns>
        /// Returns vector of ones in terms of tropical <paramref name="algebra"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="size"/> must be &gt;=1
        /// </exception>
        public static Entity.Matrix GetVectorOfOnes(int size, Algebra algebra) {
            if(size < 1) {
                throw new ArgumentException("Size must be >= 1");
            }

            Entity.Matrix result = MathS.ZeroVector(size);

            for(int i = 0; i < size; i++) {
                result = result.WithElement(i, algebra.One);
            }

            return result;
        }

        /// <summary>
        /// Get vector of <see cref="Current.Algebra.One"/>s in terms of <see cref="Current.Algebra"/> tropical algebra.
        /// </summary>
        /// <param name="size">
        /// Size of the vector
        /// </param>
        /// <returns>
        /// Returns vector of ones in terms of tropical <see cref="Current.Algebra"/> tropical algebra.
        public static Entity.Matrix GetVectorOfOnes(int size) => GetVectorOfOnes(size, Current.Algebra);

        #endregion

        /// <summary>
        /// Compares two matrices <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        /// <remarks>
        /// Protected from mistakes, that occuring cause of AngouriMath inner logic.
        /// </remarks>
        /// <param name="a">
        /// Left matrix for comparing.
        /// </param>
        /// <param name="b">
        /// Right matrix for comparing.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if matrices are equal and <see langword="false"/> if they are not.
        /// </returns>
        public static bool AreMatriciesEqual(Entity.Matrix a, Entity.Matrix b) {
            using var _ = Settings.DowncastingEnabled.Set(false);

            if(a == b) {
                return true; // if a == b is true then a equals b, but if a == b is false a may be equals b cause of AngouriMath logic
            }

            if(a is null && b is null) {
                return true;
            }
            else if(a is null || b is null) {
                return false;
            }
            
            if(a.ColumnCount != b.ColumnCount ||
                a.RowCount != b.RowCount)
                return false;

            // We can't compare as ``a[i,j] == b[i,j]`` cause of inner logic of AngouriMath
            // Read more in official Discord server: https://discord.com/channels/642350046213439489/897053152044535808/1097626035761197056
            for(int i = 0; i < a.RowCount; i++) {
                for(int j = 0; j < b.ColumnCount; j++) {
                    if((a[i,j] < b[i,j] |
                        a[i, j] > b[i, j]).EvalBoolean())
                        return false;
                }
            }

            return true;
        }
    }
}
