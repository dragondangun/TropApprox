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
        Number.Real zero;
        Number.Real one;
        virtual public Number.Real Zero { get => zero; }
        virtual public Number.Real One { get => one; }

        public abstract Entity Calculate(Entity expr);
        protected abstract Entity Parse(Entity expr);
    }

    public class MaxPlus:Algebra {
        Number.Real zero = (Number.Real)double.NegativeInfinity;
        Number.Real one = 0;

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
            Sumf(var a, var b) => sum(a, b),
            Powf(var a, var b) => Parse(a) * b.EvalNumerical().RealPart,
            Mulf(Entity.Matrix matrixA, Entity.Matrix matrixB) => TropicalMatrixOperations.TropicalMatrixMultiplication(matrixA, matrixB),
            Mulf(var a, var b) => Parse(a) + Parse(b),
            Divf(var a, var b) => Parse(a) - Parse(b),
        };

        private Entity sum(Entity a, Entity b) {
            var aParsed = Parse(a);
            var bParsed = Parse(b);
            return (aParsed > bParsed).EvalBoolean() ? aParsed : bParsed;
        }
    }

    public static class Current {
        private static Algebra algebra;
        public static Algebra Algebra {
            set => algebra = value;
            get { 
                if(algebra is null) {
                    algebra = new MaxPlus();
                }
                return algebra;
            }
        }
    }
}
