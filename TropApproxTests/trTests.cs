using AngouriMath;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class trTests {
        [TestMethod]
        public void tr_A_2_1_nInf_nr_5_2_n1_nr_3_0_1() {
            // Arenge 
            var A = MathS.Matrices.Matrix(3, 3,
                2, 1, Current.Algebra.Zero,
                5, 2, -1,
                3, 0, 1);

            var expected = 2;

            // Act
            var given = TropicalMatrixOperations.tr(A);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void tr_A_6_3_0_nr_7_6_1_nr_5_4_2() {
            // Arenge 
            var A = MathS.Matrices.Matrix(3, 3,
                6, 3, 0,
                7, 6, 1,
                5, 4, 2);

            var expected = 6;

            // Act
            var given = TropicalMatrixOperations.tr(A);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void tr_A_8_7_2_nr_11_8_5_nr_9_6_3() {
            // Arenge 
            var A = MathS.Matrices.Matrix(3, 3,
                8, 7, 2,
                11, 8, 5,
                9, 6, 3);

            var expected = 8;

            // Act
            var given = TropicalMatrixOperations.tr(A);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}