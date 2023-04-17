using AngouriMath;
using TropApprox;


namespace TropApproxTests {
    [TestClass]
    public class KleeneStarTests {
        [TestMethod]
        public void KleeneStar_nInf_1_1_nr_nInf_nInf_1_nr_n2_n1_nInf() {
            // Arenge 
            var A = MathS.Matrices.Matrix(3, 3,
                Current.Algebra.Zero, 1, 1,
                Current.Algebra.Zero, Current.Algebra.Zero, 1,
                -2, -1, Current.Algebra.Zero);

            var expected = MathS.Matrices.Matrix(3, 3,
                0, 1, 2,
                -1, 0, 1,
                -2, -1, 0);

            // Act
            var given = TropicalMatrixOperations.KleeneStar(A);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}