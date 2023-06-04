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
    /// <summary>
    /// Static class for tropical polynomials
    /// </summary>
    public static class TropicalPolynomial {
        /// <summary>
        /// Create polynomial from vector of coefficients and powers range
        /// </summary>
        /// <param name="coefs">
        /// Vector of coefficients
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of polynomial
        /// </param>
        /// <param name="MRight">
        /// Greatest power of polynomial
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials. <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial
        /// </returns>
        /// <exception cref="ArgumentException">
        /// d must be greater than 0 <br/>
        /// or <br/>
        /// Coefs length must be equal to powers length
        /// </exception>
        public static Entity CreatePolynomial(Entity.Matrix coefs, Number.Real MLeft, Number.Real MRight, int d = 1) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Create polynomial from IEnumerable of coefficients and powers range
        /// </summary>
        /// <param name="coefs">
        /// IEnumerble of coefficients
        /// </param>
        /// <param name="MLeft">
        /// Smallest power of polynomial
        /// </param>
        /// <param name="MRight">
        /// Greatest power of polynomial
        /// </param>
        /// <param name="d">
        /// Parameter for creation Puiseux polynomials. <br/>
        /// When <paramref name="d"/> is 1 (default) Laurent polynomials will be created. <br/>
        /// <see cref="See&#032;" href="https://en.wikipedia.org/wiki/Laurent_polynomial"/>
        /// </param>
        /// <returns>
        /// Tropical polynomial
        /// </returns>
        /// <exception cref="ArgumentException">
        /// d must be greater than 0 <br/>
        /// or <br/>
        /// Coefs length must be equal to powers length <br/>
        /// or <br/>
        /// MLeft must be not greater than MRight
        /// </exception>
        public static Entity CreatePolynomial(IEnumerable<Number.Real> coefs, Number.Real MLeft, Number.Real MRight, int d = 1) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Create polynomial from vector of coefficients and IEnumerable of powers
        /// </summary>
        /// <param name="coefs">
        /// Vector of coefficients 
        /// </param>
        /// <param name="powers">
        /// IEnumerable of powers
        /// </param>
        /// <returns>
        /// Tropical polynomial
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Coefs must be vector, not matrix. If it's row vector you should transpose it. <br/>
        /// or <br/>
        /// Coefs length must be equal to powers length
        /// </exception>
        public static Entity CreatePolynomial(Entity.Matrix coefs, IEnumerable<Number.Real> powers) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Create polynomial from IEnumerable of coefficients and IEnumerable of powers
        /// </summary>
        /// <param name="coefs">
        /// IEnumerable of coefficients
        /// </param>
        /// <param name="powers">
        /// IEnumerable of powers
        /// </param>
        /// <returns>
        /// Tropical polynomial
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Coefs length must be equal to powers length
        /// </exception>
        public static Entity CreatePolynomial(IEnumerable<Number.Real> coefs, IEnumerable<Number.Real> powers) {
            using var _ = Settings.DowncastingEnabled.Set(false);
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

        /// <summary>
        /// Get value of <paramref name="polynomial"/> in <paramref name="point"/> in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="polynomial">
        /// Polynomial in <paramref name="point"/> of which you want to find value
        /// </param>
        /// <param name="point">
        /// Point in which you want find value of <paramref name="polynomial"/>
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which you want to find value of <paramref name="polynomial"/>
        /// </param>
        /// <returns>
        /// Value of <paramref name="polynomial"/> in <paramref name="point"/>
        /// </returns>
        public static Number.Real GetPolynomialValue(Entity polynomial, Number.Real point, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var variable = Var("x");

            return (Number.Real)algebra.Calculate(polynomial.Substitute(variable, point));
        }

        /// <summary>
        /// Get value of <paramref name="polynomial"/> in <paramref name="point"/> in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="polynomial">
        /// Polynomial in <paramref name="point"/> of which you want to find value
        /// </param>
        /// <param name="point">
        /// Point in which you want find value of <paramref name="polynomial"/>
        /// </param>
        /// <returns>
        /// Value of <paramref name="polynomial"/> in <paramref name="point"/>
        /// </returns>
        public static Number.Real GetPolynomialValue(Entity polynomial, Number.Real point) => GetPolynomialValue(polynomial, point, Current.Algebra);

        /// <summary>
        /// Create tropical rational function
        /// </summary>
        /// <param name="P">
        /// Polynomial <paramref name="P"/> in rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <param name="Q">
        /// Polynomial <paramref name="Q"/> in rational function <paramref name="P"/>/<paramref name="Q"/>
        /// </param>
        /// <returns>
        /// Tropical rational function
        /// </returns>
        public static Entity CreateRationalFunction(Entity P, Entity Q) => $"({P})/({Q})";

        /// <summary>
        /// Get value of rational function in <paramref name="point"/> in terms of tropical <paramref name="algebra"/>
        /// </summary>
        /// <param name="rationalFunction">
        /// Tropical rational function in <paramref name="point"/> of which you want to find value
        /// </param>
        /// <param name="point">
        /// Point in which you want find value of <paramref name="rationalFunction"/>
        /// </param>
        /// <param name="algebra">
        /// Algebra in terms of which you want to find value of <paramref name="rationalFunction"/>
        /// </param>
        /// <returns>
        /// Value of <paramref name="rationalFunction"/> in <paramref name="point"/>
        /// </returns>
        public static Number.Real GetRationalFunctionValue(Entity rationalFunction, Number.Real point, Algebra algebra)
            => GetPolynomialValue(rationalFunction, point, algebra);

        /// <summary>
        /// Get value of rational function in <paramref name="point"/> in terms of <see cref="Current.Algebra"/> tropical algebra
        /// </summary>
        /// <param name="rationalFunction">
        /// Tropical rational function in <paramref name="point"/> of which you want to find value
        /// </param>
        /// <param name="point">
        /// Point in which you want find value of <paramref name="rationalFunction"/>
        /// </param>
        /// <returns>
        /// Value of <paramref name="rationalFunction"/> in <paramref name="point"/>
        /// </returns>
        public static Number.Real GetRationalFunctionValue(Entity rationalFunction, Number.Real point)
            => GetPolynomialValue(rationalFunction, point, Current.Algebra);
    }
}
