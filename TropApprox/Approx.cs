using AngouriMath;
using AngouriMath.Extensions;
using HonkSharp.Fluency;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using static AngouriMath.MathS;
using TMO = TropApprox.TropicalMatrixOperations;

namespace TropApprox {
    public static class Approx {

        public static Entity.Matrix CreateMatrixX(Entity.Matrix vectorX, int MLeft, int MRight, Algebra algebra, int d = 1) {
            var columnCount = MRight - MLeft + 1;
            var K = vectorX.RowCount;

            var result = MathS.ZeroMatrix(K, columnCount);

            for(int i = 0; i < K; i++) {
                for(int j = 0, m = MLeft; m <= MRight; j++, m++) {
                    var element = algebra.Calculate($"({vectorX[i]})^({m}/{d})");
                    result = result.WithElement(i, j, element);
                }
            }

            return result;
        }

        public static Entity.Matrix CreateMatrixX(Entity.Matrix vectorX, int MLeft, int MRight, int d = 1)
            => CreateMatrixX(vectorX, MLeft, MRight, Current.Algebra, d);

        public static Entity.Matrix CreateVectorY(Entity.Matrix vectorX, Entity function) {
            if((function).ToString().Length == 0) {
                throw new ArgumentException("Function is empty!");
            }

            var K = vectorX.RowCount;

            var result = MathS.ZeroVector(K);

            var xVariable = Var("x");

            for(int i = 0; i < K; i++) {
                var xValue = vectorX[i];
                var val = function.Substitute(xVariable, xValue).EvalNumerical().RealPart;
                result = result.WithElement(i, val);
            }

            return result;
        }

        public static Entity.Matrix CreateMatrixY(Entity.Matrix vectorX, Entity function) {
            var vectorY = CreateVectorY(vectorX, function);
            var result = TMO.GetIdentityMatrix(vectorX.RowCount);

            for(int i = 0; i<vectorX.RowCount; i++) {
                result = result.WithElement(i, i, vectorY[i]);
            }

            return result; 
        }

        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, Algebra algebra, int d = 1) {
            var vectorY = CreateVectorY(vectorX, function);
            var matrixX = CreateMatrixX(vectorX, mLeft, mRight, algebra, d);
            var vectorYPseudoInversed = TropicalMatrixOperations.PseudoInverse(vectorY, algebra);
            var vectorYPseudoInversedMultMatrixX = TropicalMatrixOperations.TropicalMatrixMultiplication(vectorYPseudoInversed, matrixX, algebra);
            var important = TropicalMatrixOperations.PseudoInverse(vectorYPseudoInversedMultMatrixX, algebra);
            var matrixXMultImportant = TropicalMatrixOperations.TropicalMatrixMultiplication(matrixX, important, algebra);
            var matrixXMultImportantPseudoInversed = TropicalMatrixOperations.PseudoInverse(matrixXMultImportant, algebra);
            Delta = (Number.Real)TropicalMatrixOperations.TropicalMatrixMultiplication(matrixXMultImportantPseudoInversed, vectorY, algebra)[0];

            var sqrtDelta = MathS.Vector(algebra.Calculate($"({Delta})^(1/2)"));

            var theta = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(sqrtDelta, important, algebra);

            return theta;
        }

        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, int d = 1)
            => ApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out Delta, Current.Algebra, d);

        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, int d = 1)
            => ApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, Current.Algebra, d);

        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, Algebra algebra, int d = 1)
            => ApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, algebra, d);

        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, Algebra algebra, int d = 1) {
            var vectorY = CreateVectorY(vectorX, function);
            var matrixX = CreateMatrixX(vectorX, mLeft, mRight, algebra, d);

            var important = vectorY.PseudoInverse(algebra).TropicalMatrixMultiplication(matrixX, algebra).PseudoInverse(algebra);
            Delta = (Number.Real)matrixX.TropicalMatrixMultiplication(important, algebra).PseudoInverse(algebra).TropicalMatrixMultiplication(vectorY, algebra)[0];

            var sqrtDelta = MathS.Vector(algebra.Calculate($"({Delta})^(1/2)"));

            var theta = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(sqrtDelta, important, algebra);

            return theta;
        }

        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, int d = 1)
            => PipeApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out Delta, Current.Algebra, d);

        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, int d = 1)
            => PipeApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, Current.Algebra, d);

        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, Algebra algebra, int d = 1)
            => PipeApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, algebra, d);

        public static Entity ApproximateFunction
        (
            Entity function,
            Entity.Matrix vectorX,
            int MLeft, int MRight,
            out Entity P, out Entity Q,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            out Number.Real Delta,
            int d = 1
        )
        {
            var X = CreateMatrixX(vectorX, MLeft, MRight, d);
            var Y = CreateMatrixY(vectorX, function);
            var YX = Y.TropicalMatrixMultiplication(X);

            Optimization.PipeSolveTwoSidedEquation(X, YX, out theta, out sigma, out Delta);

            P = TropicalPolynomial.CreatePolynomial(theta, MLeft, MRight, d);
            Q = TropicalPolynomial.CreatePolynomial(sigma, MLeft, MRight, d);

            return TropicalPolynomial.CreateRationalFunction(P, Q);
        }

        public static Entity ApproximateFunction(Entity function, Entity.Matrix vectorX, int MLeft, int MRight, int d = 1)
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out _, out _, out _, d);

        public static Entity ApproximateFunction(Entity function, Entity.Matrix vectorX, int MLeft, int MRight, out Number.Real Delta, int d = 1)
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out _, out _, out Delta, d);
        
        public static Entity ApproximateFunction
            (
                Entity function, Entity.Matrix vectorX,
                int MLeft, int MRight,
                out Entity.Matrix theta, out Entity.Matrix sigma,
                int d = 1
            )
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out theta, out sigma, out _, d);

        public static Entity ApproximateFunction
            (
                Entity function, Entity.Matrix vectorX,
                int MLeft, int MRight,
                out Entity.Matrix theta, out Entity.Matrix sigma,
                out Number.Real Delta,
                int d = 1
            )
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out theta, out sigma, out Delta, d);

        public static Entity ApproximateFunction(Entity function, Entity.Matrix vectorX, int MLeft, int MRight, out Entity P, out Entity Q, int d = 1)
            => ApproximateFunction(function, vectorX, MLeft, MRight, out P, out Q, out _, out _, out _, d);

        public static Entity ApproximateFunction
            (
                Entity function, Entity.Matrix vectorX,
                int MLeft, int MRight,
                out Entity P, out Entity Q,
                out Number.Real Delta,
                int d = 1
            )
            => ApproximateFunction(function, vectorX, MLeft, MRight, out P, out Q, out _, out _, out Delta, d);

        public static Entity ApproximateFunction
            (
                Entity function, Entity.Matrix vectorX,
                int MLeft, int MRight,
                out Entity P, out Entity Q,
                out Entity.Matrix theta, out Entity.Matrix sigma,
                int d = 1
            )
            => ApproximateFunction(function, vectorX, MLeft, MRight, out P, out Q, out theta, out sigma, out _, d);

    }
}
