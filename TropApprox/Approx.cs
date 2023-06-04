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

        /// <summary>
        /// Creates matrix X for approximation
        /// </summary>
        /// <param name="vectorX">
        /// Vector of x / vector of points for approximation
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of polynomial
        /// </param>
        /// <param name="MRight">
        /// Greatest power of polynomial
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which matrix X must be calculated
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Matrix X for approximation
        /// </returns>
        /// <exception cref="ArgumentException">
        /// MLeft must be less than MRight <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix CreateMatrixX(Entity.Matrix vectorX, int MLeft, int MRight, Algebra algebra, int d = 1) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var columnCount = MRight - MLeft + 1;
            if(columnCount < 1) {
                throw new ArgumentException("MLeft must be less than MRight");
            }
            var K = vectorX.RowCount;
            if(K < 1) {
                throw new ArgumentException("Vector of X is empty");
            }
            if(d <= 0) {
                throw new ArgumentException("d must be > 0");
            }

            var result = MathS.ZeroMatrix(K, columnCount);

            for(int i = 0; i < K; i++) {
                for(int j = 0, m = MLeft; m <= MRight; j++, m++) {
                    var element = algebra.Calculate($"({vectorX[i]})^({m}/{d})");
                    result = result.WithElement(i, j, element);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates matrix X for approximation in terms of <see cref="Current.Algebra"/>
        /// </summary>
        /// <param name="vectorX">
        /// Vector of x / vector of points for approximation
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of polynomial
        /// </param>
        /// <param name="MRight">
        /// Greatest power of polynomial
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Matrix X for approximation
        /// </returns>
        /// <exception cref="ArgumentException">
        /// MLeft must be less than MRight<br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix CreateMatrixX(Entity.Matrix vectorX, int MLeft, int MRight, int d = 1)
            => CreateMatrixX(vectorX, MLeft, MRight, Current.Algebra, d);

        /// <summary>
        /// Create vector Y for approximation (vector of function values in approximation points)
        /// </summary>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="function">
        /// Function for approximation
        /// </param>
        /// <returns>
        /// Vector of function values in approximation points / vector Y for approximation
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty<br/>
        /// or <br/>
        /// Vector of x is empty
        /// </exception>
        public static Entity.Matrix CreateVectorY(Entity.Matrix vectorX, Entity function) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            if((function).ToString().Length == 0) {
                throw new ArgumentException("Function is empty!");
            }

            var K = vectorX.RowCount;
            if(K < 1) {
                throw new ArgumentException("Vector of X is empty");
            }

            var result = MathS.ZeroVector(K);

            var xVariable = Var("x");

            for(int i = 0; i < K; i++) {
                var xValue = vectorX[i];
                var val = function.Substitute(xVariable, xValue).EvalNumerical().RealPart;
                result = result.WithElement(i, val);
            }

            return result;
        }

        /// <summary>
        /// Create diagonal matrix Y for approximation
        /// </summary>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="function">
        /// Function for approximation
        /// </param>
        /// <returns>
        /// Diagonal matrix of function values in approximation points / diagonal matrix Y for approximation
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty<br/>
        /// or <br/>
        /// Vector of x is empty
        /// </exception>
        public static Entity.Matrix CreateMatrixY(Entity.Matrix vectorX, Entity function) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var vectorY = CreateVectorY(vectorX, function);
            var result = TMO.GetIdentityMatrix(vectorX.RowCount);

            for(int i = 0; i<vectorX.RowCount; i++) {
                result = result.WithElement(i, i, vectorY[i]);
            }

            return result; 
        }

        /// <summary>
        /// Approximation of convex function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which you want to approximate. <br/>
        /// Use <see cref="MaxPlus"/>-algebra for convex function, <see cref="MinPlus"/>-algebra for concave function. <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Convex_function"/> <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Concave_function"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, Algebra algebra, int d = 1) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Approximation of convex function in terms of <see cref="Current.Algebra"/>
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, int d = 1)
            => ApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out Delta, Current.Algebra, d);

        /// <summary>
        /// Approximation of convex function in terms of <see cref="Current.Algebra"/>
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, int d = 1)
            => ApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, Current.Algebra, d);

        /// <summary>
        /// Approximation of convex function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which you want to approximate. <br/>
        /// Use <see cref="MaxPlus"/>-algebra for convex function, <see cref="MinPlus"/>-algebra for concave function. <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Convex_function"/> <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Concave_function"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix ApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, Algebra algebra, int d = 1)
            => ApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, algebra, d);

        /// <summary>
        /// Approximation of convex function
        /// </summary>
        /// <remarks>
        /// Don't use extra variables as in <see cref="ApproximateFunctionWithPolynomial(Entity, Entity.Matrix, int, int, Algebra, int)"./>
        /// </remarks>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which you want to approximate. <br/>
        /// Use <see cref="MaxPlus"/>-algebra for convex function, <see cref="MinPlus"/>-algebra for concave function. <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Convex_function"/> <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Concave_function"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, Algebra algebra, int d = 1) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var vectorY = CreateVectorY(vectorX, function);
            var matrixX = CreateMatrixX(vectorX, mLeft, mRight, algebra, d);

            var important = vectorY.PseudoInverse(algebra).TropicalMatrixMultiplication(matrixX, algebra).PseudoInverse(algebra);
            Delta = (Number.Real)matrixX.TropicalMatrixMultiplication(important, algebra).PseudoInverse(algebra).TropicalMatrixMultiplication(vectorY, algebra)[0];

            var sqrtDelta = MathS.Vector(algebra.Calculate($"({Delta})^(1/2)"));

            var theta = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(sqrtDelta, important, algebra);

            return theta;
        }

        /// <summary>
        /// Approximation of convex function in terms of <see cref="Current.Algebra"/>
        /// </summary>
        /// <remarks>
        /// Don't use extra variables as in <see cref="ApproximateFunctionWithPolynomial(Entity, Entity.Matrix, int, int, Algebra, int)"./>
        /// </remarks>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, out Number.Real Delta, int d = 1)
            => PipeApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out Delta, Current.Algebra, d);

        /// <summary>
        /// Approximation of convex function in terms of <see cref="Current.Algebra"/>
        /// </summary>
        /// <remarks>
        /// Don't use extra variables as in <see cref="ApproximateFunctionWithPolynomial(Entity, Entity.Matrix, int, int, Algebra, int)"./>
        /// </remarks>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, int d = 1)
            => PipeApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, Current.Algebra, d);

        /// <summary>
        /// Approximation of convex function
        /// </summary>
        /// <remarks>
        /// Don't use extra variables as in <see cref="ApproximateFunctionWithPolynomial(Entity, Entity.Matrix, int, int, Algebra, int)"./>
        /// </remarks>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="mLeft">
        /// Smallest power of result polynomial
        /// </param>
        /// <param name="mRight">
        /// Greatest power of result polynomial
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which you want to approximate. <br/>
        /// Use <see cref="MaxPlus"/>-algebra for convex function, <see cref="MinPlus"/>-algebra for concave function. <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Convex_function"/> <br/>
        /// <seealso href="https://en.wikipedia.org/wiki/Concave_function"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial that approximates funciton
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity.Matrix PipeApproximateFunctionWithPolynomial(Entity function, Entity.Matrix vectorX, int mLeft, int mRight, Algebra algebra, int d = 1)
            => PipeApproximateFunctionWithPolynomial(function, vectorX, mLeft, mRight, out _, algebra, d);

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="P">
        /// Polynomial <paramref name="P"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="Q">
        /// Polynomial <paramref name="Q"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="theta">
        /// Coefficients of polynomial <paramref name="P"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="sigma">
        /// Coefficients of polynomial <paramref name="Q"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
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
            using var _ = Settings.DowncastingEnabled.Set(false);
            var mp = MaxPlus.Instance;
            var X = CreateMatrixX(vectorX, MLeft, MRight, mp, d);
            var Y = CreateMatrixY(vectorX, function);
            var YX = Y.TropicalMatrixMultiplication(X, mp);

            Optimization.PipeSolveTwoSidedEquation(X, YX, out theta, out sigma, out Delta, mp);

            P = TropicalPolynomial.CreatePolynomial(theta, MLeft, MRight, d);
            Q = TropicalPolynomial.CreatePolynomial(sigma, MLeft, MRight, d);

            return TropicalPolynomial.CreateRationalFunction(P, Q);
        }

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity ApproximateFunction(Entity function, Entity.Matrix vectorX, int MLeft, int MRight, int d = 1)
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out _, out _, out _, d);

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity ApproximateFunction(Entity function, Entity.Matrix vectorX, int MLeft, int MRight, out Number.Real Delta, int d = 1)
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out _, out _, out Delta, d);

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="theta">
        /// Coefficients of polynomial P in rational functions P/Q
        /// </param>
        /// <param name="sigma">
        /// Coefficients of polynomial Q in rational functions P/Q
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity ApproximateFunction
            (
                Entity function, Entity.Matrix vectorX,
                int MLeft, int MRight,
                out Entity.Matrix theta, out Entity.Matrix sigma,
                int d = 1
            )
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out theta, out sigma, out _, d);

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="theta">
        /// Coefficients of polynomial P in rational functions P/Q
        /// </param>
        /// <param name="sigma">
        /// Coefficients of polynomial P in rational functions P/Q
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function P/Q
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity ApproximateFunction
            (
                Entity function, Entity.Matrix vectorX,
                int MLeft, int MRight,
                out Entity.Matrix theta, out Entity.Matrix sigma,
                out Number.Real Delta,
                int d = 1
            )
            => ApproximateFunction(function, vectorX, MLeft, MRight, out _, out _, out theta, out sigma, out Delta, d);

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="P">
        /// Polynomial <paramref name="P"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="Q">
        /// Polynomial <paramref name="Q"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity ApproximateFunction(Entity function, Entity.Matrix vectorX, int MLeft, int MRight, out Entity P, out Entity Q, int d = 1)
            => ApproximateFunction(function, vectorX, MLeft, MRight, out P, out Q, out _, out _, out _, d);

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="P">
        /// Polynomial <paramref name="P"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="Q">
        /// Polynomial <paramref name="Q"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="Delta">
        /// Error of approximation in terms of Chebyshev distance <br/>
        /// <see href="https://en.wikipedia.org/wiki/Chebyshev_distance"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
        public static Entity ApproximateFunction
            (
                Entity function, Entity.Matrix vectorX,
                int MLeft, int MRight,
                out Entity P, out Entity Q,
                out Number.Real Delta,
                int d = 1
            )
            => ApproximateFunction(function, vectorX, MLeft, MRight, out P, out Q, out _, out _, out Delta, d);

        /// <summary>
        /// Approximate function
        /// </summary>
        /// <param name="function">
        /// Function to approximate
        /// </param>
        /// <param name="vectorX">
        /// Vector of approximation points
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of result rational function
        /// </param>
        /// <param name="MRight">
        /// Greatest power of result rational function
        /// </param>
        /// <param name="P">
        /// Polynomial <paramref name="P"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="Q">
        /// Polynomial <paramref name="Q"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="theta">
        /// Coefficients of polynomial <paramref name="P"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="sigma">
        /// Coefficients of polynomial <paramref name="Q"/> in rational functions <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials (in result of approximation). <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Function must be non-empty <br/>
        /// or <br/>
        /// Vector of x is empty <br/>
        /// or <br/>
        /// d must be greater than 0
        /// </exception>
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
