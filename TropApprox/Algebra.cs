using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApprox {

    public abstract class Algebra {
        readonly Number.Real zero;
        readonly Number.Real one;
        virtual public Number.Real Zero { get => zero; }
        virtual public Number.Real One { get => one; }

        public abstract Entity Calculate(Entity expr);
        protected abstract Entity Parse(Entity expr);
    }

    public class MaxPlus:Algebra {
        private static MaxPlus instance;
        public static MaxPlus Instance {
            get {
                instance ??= new MaxPlus();
                return instance;
            }
        }

        protected MaxPlus() { }

        readonly Number.Real zero = Number.Real.NegativeInfinity;
        readonly Number.Real one = 0;

        override public Number.Real Zero { get => zero; }
        override public Number.Real One { get => one; }

        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

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

        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a);
            var bParsed = Parse(b);
            return (aParsed > bParsed).EvalBoolean() ? aParsed : bParsed;
        }
    }

    public class MinPlus:Algebra {
        private static MinPlus instance;
        public static MinPlus Instance {
            get {
                instance ??= new MinPlus();
                return instance;
            }
        }

        protected MinPlus() { }

        readonly Number.Real zero = Number.Real.PositiveInfinity;
        readonly Number.Real one = 0;

        override public Number.Real Zero { get => zero; }
        override public Number.Real One { get => one; }

        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

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

        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a);
            var bParsed = Parse(b);
            return (aParsed < bParsed).EvalBoolean() ? aParsed : bParsed;
        }
    }

    public class MaxTimes:Algebra {
        private static MaxTimes instance;
        public static MaxTimes Instance {
            get {
                instance ??= new MaxTimes();
                return instance;
            }
        }

        protected MaxTimes() { }

        readonly Number.Real zero = 0;
        readonly Number.Real one = 1;

        override public Number.Real Zero { get => zero; }
        override public Number.Real One { get => one; }

        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

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

        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a).EvalNumerical().RealPart;
            var bParsed = Parse(b).EvalNumerical().RealPart;
            return (aParsed > bParsed) ? aParsed : bParsed;
        }

        private static Number.Real BordersCheck(Number.Real a) {
            if(a >= 0) {
                return a;
            }

            throw new ArgumentException("Number must be >= 0");
        }
    }

    public class MinTimes:Algebra {
        private static MinTimes instance;
        public static MinTimes Instance {
            get {
                instance ??= new MinTimes();
                return instance;
            }
        }

        protected MinTimes() { }

        readonly Number.Real zero = Number.Real.PositiveInfinity;
        readonly Number.Real one = 1;

        override public Number.Real Zero { get => zero; }
        override public Number.Real One { get => one; }

        override public Entity Calculate(Entity expr) {
            Entity res = Parse(expr);
            if(res is not Entity.Matrix) {
                res = res.EvalNumerical().RealPart;
            }
            return res;
        }

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

        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a).EvalNumerical().RealPart;
            var bParsed = Parse(b).EvalNumerical().RealPart;
            return (aParsed < bParsed) ? aParsed : bParsed;
        }

        private static Number.Real BordersCheck(Number.Real a) { 
            if (a > 0) {
                return a;
            }

            throw new ArgumentException("Number must be > 0");
        }
    }

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
