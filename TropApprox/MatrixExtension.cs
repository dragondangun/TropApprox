using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using TMO = TropApprox.TropicalMatrixOperations;

namespace TropApprox {
    /// <summary>
    /// Static class of extension functions for <see cref="Entity.Matrix"/>
    /// </summary>
    public static class MatrixExtension {
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
        public static Entity.Matrix PseudoInverse(this Entity.Matrix matrix, Algebra algebra) => TMO.PseudoInverse(matrix, algebra);

        /// <summary>
        /// Tropical Pseudo-inverse of <paramref name="matrix"/> (vector) in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="matrix">
        /// Matrix (vector) which you want to pseudo-inverse
        /// <returns>
        /// Pseudo-inversed matrix or vector
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> (or vector) can't be null!
        /// </exception>
        public static Entity.Matrix PseudoInverse(this Entity.Matrix matrix) => TMO.PseudoInverse(matrix, Current.Algebra);

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
        public static Entity.Matrix TropicalMatrixMultiplication(this Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra)
            => TMO.TropicalMatrixMultiplication(matrixA, matrixB, algebra);

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
        public static Entity.Matrix TropicalMatrixAddition(this Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra)
            => TMO.TropicalMatrixAddition(matrixA, matrixB, algebra);

        /// <summary>
        /// Tropical addition of <paramref name="matrixA"/> and <paramref name="matrixB"/> in terms of <see cref="Current.Algebra"/> tropical algebra
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
        public static Entity.Matrix TropicalMatrixAddition(this Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TMO.TropicalMatrixAddition(matrixA, matrixB, Current.Algebra);

        /// <summary>
        /// Multiplication of matrices <paramref name="matrixA"/> (or/and vectors) and <paramref name="matrixB"/> in terms of <see cref="Current.Algebra"/> tropical algebra
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
        public static Entity.Matrix TropicalMatrixMultiplication(this Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TMO.TropicalMatrixMultiplication(matrixA, matrixB, Current.Algebra);

        /// <summary>
        /// Multiplication of <paramref name="matrix"/> (vector) and <paramref name="scalar"/> in terms of tropical <paramref name="algebra"/>
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
        public static Entity.Matrix TropicalMatrixScalarMultiplication(this Entity.Matrix matrix, Number.Real scalar, Algebra algebra)
            => TMO.TropicalMatrixScalarMultiplication(matrix, scalar, algebra);

        /// <summary>
        /// Multiplication of <paramref name="matrix"/> (vector) and <paramref name="scalar"/> in terms of <see cref="Current.Algebra"/> tropical algebra 
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
        public static Entity.Matrix TropicalMatrixScalarMultiplication(this Entity.Matrix matrix, Number.Real scalar)
            => TMO.TropicalMatrixScalarMultiplication(matrix, scalar, Current.Algebra);

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
        public static bool IsEqualTo(this Entity.Matrix a, Entity.Matrix b)
            => TMO.AreMatriciesEqual(a, b);
    }
}
