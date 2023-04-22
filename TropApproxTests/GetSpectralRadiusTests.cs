using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class GetSpectralRadiusTests {
        [TestMethod]
        public void GetSpectralRadiusTests_A_n1_n2_nInf_nr_2_n1_n4_nr_0_n3_n2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            var A = MathS.Matrices.Matrix(3, 3,
                2, 1, Current.Algebra.Zero,
                5, 2, -1,
                3, 0, 1);


            Number.Real expected = 3;

            // Act
            Number.Real given = (Number.Real)TropicalMatrixOperations.GetSpectralRadius(A);

            // Assert
            //Assert.AreEqual(expected, given);
        }
    }
}