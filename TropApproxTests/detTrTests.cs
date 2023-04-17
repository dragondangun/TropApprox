using AngouriMath;
using TropApprox;


namespace TropApproxTests {
    [TestClass]
    public class TrTests {
        [TestMethod]
        public void Tr_A_2_1_nInf_nr_5_2_n1_nr_3_0_1() {
            // Arenge 
            var A = MathS.Matrices.Matrix(3, 3,
                2, 1, Current.Algebra.Zero,
                5, 2, -1,
                3, 0, 1);


            double expected = 8;

            // Act
            var given = TropicalMatrixOperations.Tr(A);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void Tr_nInf_1_1_nr_nInf_nInf_1_nr_n2_n1_nInf() {
            // Arenge 
            var A = MathS.Matrices.Matrix(3, 3,
                Current.Algebra.Zero, 1, 1,
                Current.Algebra.Zero, Current.Algebra.Zero, 1,
                -2, -1, Current.Algebra.Zero);


            double expected = 0;

            // Act
            var given = TropicalMatrixOperations.Tr(A);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}