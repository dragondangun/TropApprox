﻿using AngouriMath;
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

    }
}
