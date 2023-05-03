using AngouriMath;
using AngouriMath.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using static AngouriMath.MathS;
using AngouriMath.Extensions;

namespace TropApprox {
    public static class TropicalPolynomial {
        public static Entity CreatePolynomial(Entity.Matrix coefs, Number.Real MLeft, Number.Real MRight, int d = 1) {
            if(d <= 0) {
                throw new ArgumentException("d must be > 0");
            }

            List<Number.Real> coefsList = new() {
                Capacity = coefs.RowCount,
            };

            for(int i = 0; i < coefs.RowCount; i++) {
                coefsList.Add((Number.Real)coefs[i]);
            }

            var length = MRight - MLeft + 1;

            if((int)length != coefsList.Count) {
                throw new ArgumentException("Coefs length must be equal to powers length!");
            }

            List<Number.Real> powers = new() {
                Capacity = (int)length
            };

            if(d == 1) {
                for(var p = MLeft; p <= MRight; p += 1) {
                    powers.Add(p);
                }
            }
            else {
                for(var p = MLeft; p <= MRight; p += 1) {
                    powers.Add(p / d);
                    //powers.Add((p / d).EvalNumerical().RealPart);
                }
            }

            return CreatePolynomial(coefsList, powers);
        }

        //public static Entity CreatePolynomial(Entity.Matrix coefs, Number.Real MLeft, Number.Real MRight, uint d = 1) {
        //    if(MRight < MLeft) {
        //        throw new ArgumentException("MLeft must be <= MRight");
        //    }

        //    if(d == 0) {
        //        throw new ArgumentException("d must be > 0");
        //    }

        //    if(!coefs.IsVector) {
        //        throw new ArgumentException("Coefs must be vector, not matrix. If it's row vector you should transpose it.");
        //    }

        //    var length = MRight - MLeft + 1;

        //    if((int)length != coefs.RowCount) {
        //        throw new ArgumentException("Coefs length must be equal to powers length!");
        //    }

        //    List<Number.Real> powers = new() {
        //        Capacity = (int)length
        //    };

        //    if(d == 1) {
        //        for(var p = MLeft; p <= MRight; p += 1) {
        //            powers.Add(p);
        //        }
        //    }
        //    else {
        //        for(var p = MLeft; p <= MRight; p += 1) {
        //            powers.Add(p / d);
        //            //powers.Add((p / d).EvalNumerical().RealPart);
        //        }
        //    }
            
        //    List<Number.Real> coefsList = new() {
        //        Capacity = (int)length
        //    };

        //    for(int i = 0; i < coefs.RowCount; i++) {
        //        coefsList.Add((Number.Real)coefs[i]);
        //    }

        //    return CreatePolynomial(coefsList, powers);
        //}

        public static Entity CreatePolynomial(IEnumerable<Number.Real> coefs, Number.Real MLeft, Number.Real MRight, int d = 1) {
            if(MRight < MLeft) {
                throw new ArgumentException("MLeft must be <= MRight");
            }

            if(d <= 0) {
                throw new ArgumentException("d must be > 0");
            }

            var length = MRight - MLeft + 1;

            if((int)length != coefs.Count()) {
                throw new ArgumentException("Coefs length must be equal to powers length!");
            }

            List<Number.Real> powers = new() {
                Capacity = (int)length
            };

            if(d == 1) {
                for(var p = MLeft; p <= MRight; p += 1) {
                    powers.Add(p);
                }
            }
            else {
                for(var p = MLeft; p <= MRight; p += 1) {
                    powers.Add(p / d);
                    //powers.Add((p / d).EvalNumerical().RealPart);
                }
            }

            return CreatePolynomial(coefs, powers);
        }

        public static Entity CreatePolynomial(Entity.Matrix coefs, IEnumerable<Number.Real> powers) {
            if(!coefs.IsVector) {
                throw new ArgumentException("Coefs must be vector, not matrix. If it's row vector you should transpose it.");
            }
            
            if(powers?.Count() != coefs.RowCount) {
                throw new ArgumentException("Coefs length must be equal to powers length!");
            }

            List<Number.Real> coefsList = new() {
                Capacity = coefs.RowCount,
            };

            for(int i = 0; i < coefs.RowCount; i++) {
                coefsList.Add((Number.Real)coefs[i]);
            }

            return CreatePolynomial(coefsList, powers);
        }

        public static Entity CreatePolynomial(IEnumerable<Number.Real> coefs, IEnumerable<Number.Real> powers) {
            var coefsList = coefs.ToList();
            var powersList = powers.ToList();

            if(coefsList.Count != powersList.Count) {
                throw new ArgumentException("Coefs length must be equal to powers length!");
            }

            StringBuilder sb = new StringBuilder() {
                Capacity = coefsList.Count * 12,
            };
            
            for(int i = 0; i < coefsList.Count; i++) {
                sb.Append($"({coefsList[i]})*x^({powersList[i]})+");
            }

            --sb.Length;

            return sb.ToString();
        }

        public static Number.Real GetPolynomialValue(Entity polynomial, Number.Real point, Algebra algebra) {
            var variable = Var("x");

            return (Number.Real)algebra.Calculate(polynomial.Substitute(variable, point));
        }

        public static Number.Real GetPolynomialValue(Entity polynomial, Number.Real point) => GetPolynomialValue(polynomial, point, Current.Algebra);
    }
}
