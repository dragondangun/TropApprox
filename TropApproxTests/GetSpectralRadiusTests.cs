using AngouriMath;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class GetSpectralRadiusTests {
        [TestMethod]
        public void GetSpectralRadiusTests_A_n1_n2_nInf_nr_2_n1_n4_nr_0_n3_n2() {
            // Arenge 
            var A = MathS.Matrices.Matrix(3, 3,
                2, 1, Current.Algebra.Zero,
                5, 2, -1,
                3, 0, 1);


            double expected = 3;

            // Act
            var given = TropicalMatrixOperations.GetSpectralRadius(A);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}