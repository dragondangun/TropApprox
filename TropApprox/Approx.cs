using AngouriMath;
using AngouriMath.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApprox {
    public static class Approx {

        public static Entity.Matrix CreateMatrixX(Entity.Matrix vectorX, int MLeft, int MRight, int d = 1) {
            var columnCount = MRight - MLeft + 1;
            var K = vectorX.RowCount;

            var result = MathS.ZeroMatrix(K, columnCount);

            Algebra.CurrAlgebra ??= Algebra.MaxPlus;

            for(int i = 0; i < K; i++) {
                for(int j = 0, m = MLeft; m <= MRight; j++, m++) {
                    double b = (double)((Number.Real)vectorX[i]);
                    string str = $"({b.ToString(CultureInfo.InvariantCulture)})^({m}/{d})";
                    Entity expr = str;
                    var element = Algebra.CurrAlgebra(expr);
                    result = result.WithElement(i, j, element);
                }
            }

            return result;
        }

        public static Entity.Matrix CreateVectorY(Entity.Matrix vectorX, string function) {
            var K = vectorX.RowCount;

            var result = MathS.ZeroVector(K);

            Algebra.CurrAlgebra ??= Algebra.MaxPlus;

            double xValue;
            var xVariable = Var("x");

            for(int i = 0; i < K; i++) {
                xValue = (double)((Number.Real)vectorX[i]);
                var val = ((double)function.Substitute(xVariable, xValue).EvalNumerical().RealPart); ;
                result = result.WithElement(i, val);
            }

            return result;
        }

        public static Entity.Matrix ApproximateFunction(string function, Entity.Matrix vectorX, int mLeft, int mRight, int d = 1) {
            Algebra.CurrAlgebra ??= Algebra.MaxPlus;

            var vectorY = CreateVectorY(vectorX, function);
            var matrixX = CreateMatrixX(vectorX, mLeft, mRight, d);
            var vectorYPseudoInversed = TropicalMatrixOperations.PseudoInverse(vectorY);
            var vectorYPseudoInversedMultMatrixX = TropicalMatrixOperations.TropicalMatrixMultiplication(vectorYPseudoInversed, matrixX);
            var important = TropicalMatrixOperations.PseudoInverse(vectorYPseudoInversedMultMatrixX);
            var matrixXMultImportant = TropicalMatrixOperations.TropicalMatrixMultiplication(matrixX, important);
            var matrixXMultImportantPseudoInversed = TropicalMatrixOperations.PseudoInverse(matrixXMultImportant);
            var deltaScalar = TropicalMatrixOperations.TropicalMatrixMultiplication(matrixXMultImportantPseudoInversed, vectorY);
            
            double delta = (double)((Number.Real)deltaScalar[0]);
            string str = $"({delta.ToString(CultureInfo.InvariantCulture)})^(1/2)";
            var sqrtDelta = MathS.Vector(Algebra.CurrAlgebra(str));

            var theta = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(sqrtDelta, important);

            return theta;
        }
    }
}
