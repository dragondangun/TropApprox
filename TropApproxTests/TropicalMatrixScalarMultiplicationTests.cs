using AngouriMath;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class TropicalMatrixScalarMultiplicationTests {
        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_nr_0_B_1() {
            var A = MathS.Matrices.Matrix(2, 1,
                0,
                0);
            var B = MathS.Vector(1);

            var expected = MathS.Vector(1, 1);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_nr_0_nr_0_B_0() {
            var A = MathS.Matrices.Matrix(3, 1,
                0,
                0,
                0);
            var B = MathS.Vector(0);

            var expected = MathS.Vector(0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_nr_0_nr_0_B_1() {
            var A = MathS.Vector(0, 0, 0);
            var B = MathS.Vector(0.5);

            var expected = MathS.Vector(0.5, 0.5, 0.5);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_B_0_nr_0_nr_0() {
            var A = MathS.Vector(0);
            var B = MathS.Vector(0, 0, 0);

            var expected = MathS.Vector(0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}