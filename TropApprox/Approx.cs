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

            Algebras.CurrAlgebra ??= Algebras.MaxPlus;

            for(int i = 0; i < K; i++) {
                for(int j = 0, m = MLeft; m <= MRight; j++, m++) {
                    double b = (double)((Number.Real)vectorX[i]);
                    string str = $"({b.ToString(CultureInfo.InvariantCulture)})^({m}/{d})";
                    Entity expr = str;
                    var element = Algebras.CurrAlgebra(expr);
                    result = result.WithElement(i, j, element);
                }
            }

            return result;
        }

        public static Entity.Matrix CreateVectorY(Entity.Matrix vectorX, string function) {
            var K = vectorX.RowCount;

            var result = MathS.ZeroVector(K);

            Algebras.CurrAlgebra ??= Algebras.MaxPlus;

            double xValue;
            var xVariable = Var("x");

            for(int i = 0; i < K; i++) {
                xValue = (double)((Number.Real)vectorX[i]);
                var val = ((double)function.Substitute(xVariable, xValue).EvalNumerical().RealPart); ;
                result = result.WithElement(i, val);
            }

            return result;
        }

        
    }
}
