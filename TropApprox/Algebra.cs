using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;

namespace TropApprox {

    public abstract class Algebra {
        double zero;
        double one;
        virtual public double Zero { get => zero; }
        virtual public double One { get => one; }

        public abstract double Calculate(Entity expr);

        //public delegate double Algebra(Entity expr);
        //static Algebra currAlgebra;
        //public static Algebra CurrAlgebra {
        //    get => currAlgebra;
        //    set => currAlgebra = value;
        //}
    }

    public class MaxPlus:Algebra {
        double zero = double.NegativeInfinity;
        double one = 0;

        override public double Zero { get => zero; }
        override public double One { get => one; }

        override public double Calculate(Entity expr)
        => expr switch {
            Number.Real r => (double)r,
            Sumf(var a, var b) => (Calculate(a) > Calculate(b)) ? Calculate(a) : Calculate(b),
            Powf(var a, var b) => Calculate(a) * (double)b.EvalNumerical().RealPart,
            Mulf(var a, var b) => Calculate(a) + Calculate(b),
            Divf(var a, var b) => Calculate(a) - Calculate(b),
        };
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
