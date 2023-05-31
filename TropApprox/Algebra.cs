using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApprox {

    /// <summary>
    /// Abstract class. Every tropical algebra class must inherit this class.
    /// </summary>
    public abstract class Algebra {
        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        readonly Number.Real zero;
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        readonly Number.Real one;
        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        virtual public Number.Real Zero { get => zero; }
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        virtual public Number.Real One { get => one; }

        /// <summary>
        /// Calculate value of expression.
        /// </summary>
        /// <param name="expr">
        /// Expression value of which you want to find.
        /// </param>
        /// <returns>
        /// Value of expression
        /// </returns>
        public abstract Entity Calculate(Entity expr);
        /// <summary>
        /// Parse expression for further evaluation
        /// </summary>
        /// <param name="expr">
        /// The expression to parse
        /// </param>
        /// <returns>
        /// Parsed expression
        /// </returns>
        protected abstract Entity Parse(Entity expr);
    }

    /// <summary>
    /// MaxPlus tropical algebra
    /// </summary>
    public class MaxPlus:Algebra {
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        private static MaxPlus instance;
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        public static MaxPlus Instance {
            get {
                instance ??= new MaxPlus();
                return instance;
            }
        }

        /// <summary>
        /// Protected constructor for singleton pattern realization
        /// </summary>
        protected MaxPlus() { }

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        readonly Number.Real zero = Number.Real.NegativeInfinity;
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        readonly Number.Real one = 0;

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        override public Number.Real Zero { get => zero; }
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        override public Number.Real One { get => one; }

        /// <summary>
        /// Calculate value of expression.
        /// </summary>
        /// <remarks>
        /// Return or <see cref="Entity.Matrix"/>, either <see cref="Number.Real"/>
        /// </remarks>
        /// <param name="expr">
        /// Expression value of which you want to find.
        /// </param>
        /// <returns>
        /// Value of expression
        /// </returns>
        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

        /// <summary>
        /// Parse expression for further evaluation
        /// </summary>
        /// <param name="expr">
        /// The expression to parse
        /// </param>
        /// <returns>
        /// Parsed expression
        /// </returns>
        override protected Entity Parse(Entity expr)
        => expr switch {
            Number.Real r => r,
            Entity.Matrix m => m,
            Sumf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixAddition(matrixA, matrixB, instance),
            Sumf(var a, var b) => sum(a, b),
            Powf(var a, var b) => Parse(a) * b.EvalNumerical().RealPart,
            Mulf(Number.Real scalar, Entity.Matrix matrix) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrix, Number.Real scalar) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) when matrixA.IsScalar || matrixB.IsScalar
                                                        => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrixA, matrixB, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixMultiplication(matrixA, matrixB, instance),
            Mulf(var a, var b) => Parse(a) + Parse(b),
            Divf(var a, var b) => Parse(a) - Parse(b),
        };

        /// <summary>
        /// Tropical sum of two <see cref="Number.Real"/>s.
        /// </summary>
        /// <param name="a">
        /// Left addendum.
        /// </param>
        /// <param name="b">
        /// Right addendum.
        /// </param>
        /// <returns>
        /// Sum of two <see cref="Number.Real"/>.
        /// </returns>
        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a);
            var bParsed = Parse(b);
            return (aParsed > bParsed).EvalBoolean() ? aParsed : bParsed; 
        }
    }

    /// <summary>
    /// MinPlus tropical algebra
    /// </summary>
    public class MinPlus:Algebra {
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        private static MinPlus instance;
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        public static MinPlus Instance {
            get {
                instance ??= new MinPlus();
                return instance;
            }
        }

        /// <summary>
        /// Protected constructor for singleton pattern realization
        /// </summary>
        protected MinPlus() { }

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        readonly Number.Real zero = Number.Real.PositiveInfinity;
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        readonly Number.Real one = 0;

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        override public Number.Real Zero { get => zero; }
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        override public Number.Real One { get => one; }

        /// <summary>
        /// Calculate value of expression.
        /// </summary>
        /// <remarks>
        /// Return or <see cref="Entity.Matrix"/>, either <see cref="Number.Real"/>
        /// </remarks>
        /// <param name="expr">
        /// Expression value of which you want to find.
        /// </param>
        /// <returns>
        /// Value of expression
        /// </returns>
        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

        /// <summary>
        /// Parse expression for further evaluation
        /// </summary>
        /// <param name="expr">
        /// The expression to parse
        /// </param>
        /// <returns>
        /// Parsed expression
        /// </returns>
        override protected Entity Parse(Entity expr)
        => expr switch {
            Number.Real r => r,
            Entity.Matrix m => m,
            Sumf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixAddition(matrixA, matrixB, instance),
            Sumf(var a, var b) => sum(a, b),
            Powf(var a, var b) => Parse(a) * b.EvalNumerical().RealPart,
            Mulf(Number.Real scalar, Entity.Matrix matrix) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrix, Number.Real scalar) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) when matrixA.IsScalar || matrixB.IsScalar
                                                        => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrixA, matrixB, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixMultiplication(matrixA, matrixB, instance),
            Mulf(var a, var b) => Parse(a) + Parse(b),
            Divf(var a, var b) => Parse(a) - Parse(b),
        };

        /// <summary>
        /// Tropical sum of two <see cref="Number.Real"/>s.
        /// </summary>
        /// <param name="a">
        /// Left addendum.
        /// </param>
        /// <param name="b">
        /// Right addendum.
        /// </param>
        /// <returns>
        /// Sum of two <see cref="Number.Real"/>.
        /// </returns>
        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a);
            var bParsed = Parse(b);
            return (aParsed < bParsed).EvalBoolean() ? aParsed : bParsed;
        }
    }

    /// <summary>
    /// MaxTimes tropical algebra
    /// </summary>
    public class MaxTimes:Algebra {
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        private static MaxTimes instance;
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        public static MaxTimes Instance {
            get {
                instance ??= new MaxTimes();
                return instance;
            }
        }

        /// <summary>
        /// Protected constructor for singleton pattern realization
        /// </summary>
        protected MaxTimes() { }

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        readonly Number.Real zero = 0;
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        readonly Number.Real one = 1;

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        override public Number.Real Zero { get => zero; }
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        override public Number.Real One { get => one; }

        /// <summary>
        /// Calculate value of expression.
        /// </summary>
        /// <remarks>
        /// Return or <see cref="Entity.Matrix"/>, either <see cref="Number.Real"/>
        /// </remarks>
        /// <param name="expr">
        /// Expression value of which you want to find.
        /// </param>
        /// <returns>
        /// Value of expression
        /// </returns>
        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

        /// <summary>
        /// Parse expression for further evaluation
        /// </summary>
        /// <param name="expr">
        /// The expression to parse
        /// </param>
        /// <returns>
        /// Parsed expression
        /// </returns>
        override protected Entity Parse(Entity expr)
        => expr switch {
            Number.Real r => BordersCheck(r),
            Entity.Matrix m => m,
            Sumf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixAddition(matrixA, matrixB, instance),
            Sumf(var a, var b) => sum(a, b),
            Powf(var a, var b) => Pow(Parse(a), b.EvalNumerical().RealPart),
            Mulf(Number.Real scalar, Entity.Matrix matrix) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrix, Number.Real scalar) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) when matrixA.IsScalar || matrixB.IsScalar
                                                        => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrixA, matrixB, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixMultiplication(matrixA, matrixB, instance),
            Mulf(var a, var b) => Parse(a) * Parse(b),
            Divf(var a, var b) => Parse(a) / Parse(b),
        };

        /// <summary>
        /// Tropical sum of two <see cref="Number.Real"/>s.
        /// </summary>
        /// <param name="a">
        /// Left addendum.
        /// </param>
        /// <param name="b">
        /// Right addendum.
        /// </param>
        /// <returns>
        /// Sum of two <see cref="Number.Real"/>.
        /// </returns>
        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a).EvalNumerical().RealPart; // sometimes parsed parts needed to be pre-evalued before comparison
            var bParsed = Parse(b).EvalNumerical().RealPart;
            return (aParsed > bParsed) ? aParsed : bParsed;
        }

        /// <summary>
        /// Checks if <paramref name="a"/> belongs to semi-ring.
        /// </summary>
        /// <remarks>
        /// MaxTimes use set of non-negative rational numbers.
        /// </remarks>
        /// <param name="a">
        /// <see cref="Number.Real"/> to check belonging to set.
        /// </param>
        /// <returns>
        /// Returns <paramref name="a"/> if it belongs to set, 
        /// throw exception otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Throw exception if <paramref name="a"/> doesn't belong to set.
        /// </exception>
        private static Number.Real BordersCheck(Number.Real a) {
            if(a >= 0) {
                return a;
            }

            throw new ArgumentException("Number must be >= 0");
        }
    }

    /// <summary>
    /// MinTimes tropical algebra
    /// </summary>
    public class MinTimes:Algebra {
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        private static MinTimes instance;
        /// <summary>
        /// Realization of singleton pattern
        /// </summary>
        public static MinTimes Instance {
            get {
                instance ??= new MinTimes();
                return instance;
            }
        }

        /// <summary>
        /// Protected constructor for singleton pattern realization
        /// </summary>
        protected MinTimes() { }

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        readonly Number.Real zero = Number.Real.PositiveInfinity;
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        readonly Number.Real one = 1;

        /// <summary>
        /// Annihilating element <br/> <see href="https://en.wikipedia.org/wiki/Absorbing_element"/>
        /// </summary>
        override public Number.Real Zero { get => zero; }
        /// <summary>
        /// Identity element <br/> <see href="https://en.wikipedia.org/wiki/Identity_element"/>
        /// </summary>
        override public Number.Real One { get => one; }

        /// <summary>
        /// Calculate value of expression.
        /// </summary>
        /// <remarks>
        /// Return or <see cref="Entity.Matrix"/>, either <see cref="Number.Real"/>
        /// </remarks>
        /// <param name="expr">
        /// Expression value of which you want to find.
        /// </param>
        /// <returns>
        /// Value of expression
        /// </returns>
        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

        /// <summary>
        /// Parse expression for further evaluation
        /// </summary>
        /// <param name="expr">
        /// The expression to parse
        /// </param>
        /// <returns>
        /// Parsed expression
        /// </returns>
        override protected Entity Parse(Entity expr)
        => expr switch {
            Number.Real r => BordersCheck(r),
            Entity.Matrix m => m,
            Sumf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixAddition(matrixA, matrixB, instance),
            Sumf(var a, var b) => sum(a, b),
            Powf(var a, var b) => Pow(Parse(a), b.EvalNumerical().RealPart),
            Mulf(Number.Real scalar, Entity.Matrix matrix) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrix, Number.Real scalar) => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrix, scalar, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) when matrixA.IsScalar || matrixB.IsScalar
                                                        => TropicalMatrixOperations.TropicalMatrixScalarMultiplication(matrixA, matrixB, instance),
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixMultiplication(matrixA, matrixB, instance),
            Mulf(var a, var b) => Parse(a) * Parse(b),
            Divf(var a, var b) => Parse(a) / Parse(b),
        };

        /// <summary>
        /// Tropical sum of two <see cref="Number.Real"/>s.
        /// </summary>
        /// <param name="a">
        /// Left addendum.
        /// </param>
        /// <param name="b">
        /// Right addendum.
        /// </param>
        /// <returns>
        /// Sum of two <see cref="Number.Real"/>.
        /// </returns>
        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a).EvalNumerical().RealPart; // sometimes parsed parts needed to be pre-evalued before comparison
            var bParsed = Parse(b).EvalNumerical().RealPart;
            return (aParsed < bParsed) ? aParsed : bParsed;
        }

        /// <summary>
        /// Checks if <paramref name="a"/> belongs to semi-ring.
        /// </summary>
        /// <remarks>
        /// MinTimes use set of positive rational numbers.
        /// </remarks>
        /// <param name="a">
        /// <see cref="Number.Real"/> to check belonging to set.
        /// </param>
        /// <returns>
        /// Returns <paramref name="a"/> if it belongs to set, 
        /// throw exception otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Throw exception if <paramref name="a"/> doesn't belong to set.
        /// </exception>
        private static Number.Real BordersCheck(Number.Real a) { 
            if (a > 0) {
                return a;
            }

            throw new ArgumentException("Number must be > 0");
        }
    }

    /// <summary>
    /// Static class, that allows use specific algebra, without passing argument every time. <br/>
    /// Use MaxPlus-algebra as default. You can pass any instance of <see cref="Algebra"/> subclass.
    /// </summary>
    public static class Current {
        private static Algebra algebra;
        public static Algebra Algebra {
            set => algebra = value;
            get { 
                algebra ??= MaxPlus.Instance;
                return algebra;
            }
        }
    }
}
