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
                            // do nothing
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
    }
}
