using AngouriMath;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;

namespace TropApprox {
    public static class TropicalMatrixOperations {
        public static Entity.Matrix PseudoInverse(Entity.Matrix matrix) {
            Algebras.CurrAlgebra ??= Algebras.MaxPlus;

            matrix = matrix.T;

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = ((double)((Number.Real)matrix[i, j])).ToString(CultureInfo.InvariantCulture);
                    matrix = matrix.WithElement(i, j, Algebras.CurrAlgebra($"({a})^(-1)"));
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

            Algebras.CurrAlgebra ??= Algebras.MaxPlus;

            var result = MathS.ZeroMatrix(matrixA.RowCount, matrixB.ColumnCount);

            StringBuilder sb = new() {
                Capacity = 100
            };


            double element;
            for(int rowA = 0, colB = 0; rowA < matrixA.RowCount; colB++) {
                for(int j = 0; j < matrixA.ColumnCount; j++) {
                    double a = (double)((Number.Real)matrixA[rowA, j]);
                    double b = (double)((Number.Real)matrixB[j, colB]);
                    sb.Append($"({a})*({b})+");
                }
                sb.Length--; // delete last "+" sign
                element = Algebras.CurrAlgebra(sb.ToString());
                sb.Length = 0;

                result = result.WithElement(rowA, colB, element);
                if(colB == matrixA.RowCount - 1 || colB == matrixB.ColumnCount - 1) {
                    colB = -1;
                    ++rowA;
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

            Algebras.CurrAlgebra ??= Algebras.MaxPlus;

            var result = MathS.ZeroMatrix(matrix.RowCount, matrix.ColumnCount);

            for(int i = 0; i < matrix.RowCount; i++) {
                for(int j = 0; j < matrix.ColumnCount; j++) {
                    var a = ((double)((Number.Real)matrix[i, j])).ToString(CultureInfo.InvariantCulture);
                    result = result.WithElement(i, j, Algebras.CurrAlgebra($"({a})*({scalar})"));
                }
            }

            return result;
        }
    }
}
