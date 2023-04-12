using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;

namespace TropApprox {
    public static class Algebras {
        public static double MaxPlus(Entity expr)
        => expr switch {
            Number.Real r => (double)r,
            Sumf(var a, var b) => (MaxPlus(a) > MaxPlus(b)) ? MaxPlus(a) : MaxPlus(b),
            Powf(var a, var b) => MaxPlus(a) * (double)b.EvalNumerical().RealPart,
            Mulf(var a, var b) => MaxPlus(a) + MaxPlus(b),
            Divf(var a, var b) => MaxPlus(a) - MaxPlus(b),
        };

        public delegate double Algebra(Entity expr);
        static Algebra currAlgebra;
        public static Algebra CurrAlgebra {
            get => currAlgebra;
            set => currAlgebra = value;
        }
    }
}
