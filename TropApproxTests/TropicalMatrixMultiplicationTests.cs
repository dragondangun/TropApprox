using AngouriMath;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class TropicalMatrixMultiplicationTests {
        [TestMethod]
        public void TropicalMatrixMultiplication_A_n1_n1_B_0_nr_0() {
            var A = MathS.Matrices.Matrix(1, 2,
                -1, -1);
            var B = MathS.Vector(0, 0);

            var expected = MathS.Matrices.Matrix(1, 1,
                -1);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_n1_0_n1_B_0_nr_0_nr_0() {
            var A = MathS.Matrices.Matrix(1, 3,
                -1, 0, -1);
            var B = MathS.Vector(0, 0, 0);

            var expected = MathS.Vector(0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_n1_n1_B_1_nr_1() {
            var A = MathS.Matrices.Matrix(1, 2,
                -1, -1);
            var B = MathS.Vector(1, 1);

            var expected = MathS.Vector(0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_0_0_0_B_1_nr_0_nr_1() {
            var A = MathS.Matrices.Matrix(1, 3,
                0, 0, 0);
            var B = MathS.Vector(1, 0, 1);

            var expected = MathS.Vector(1);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_n1_0_n1_B_1_0_n1_nr_0_0_0_nr_n1_0_1() {
            var A = MathS.Matrices.Matrix(1, 3,
                -1, 0, -1);
            var B = MathS.Matrices.Matrix(3,3,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1);

            var expected = MathS.Matrices.Matrix(1, 3, 
                0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_1_0_n1_nr_0_0_0_nr_n1_0_1_B_0_nr_0_nr_0() {
            var A = MathS.Matrices.Matrix(3, 3,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1);

            var B = MathS.Matrices.Matrix(3, 1,
                0,
                0,
                0);
            
            var expected = MathS.Matrices.Matrix(3, 1,
                1,
                0,
                1);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_n1_0_n1_B_1_nr_0_nr_1() {
            var A = MathS.Matrices.Matrix(1, 3,
                -1, 0, -1);

            var B = MathS.Vector(1, 0, 1);

            var expected = MathS.Vector(0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_n4_n1_0_n1_n4_B_2_0_n2_nr_1_0_n1_nr_0_0_0_nr_n1_0_1_n2_0_2() {
            var A = MathS.Matrices.Matrix(1, 5,
                -4, -1, 0, -1, -4);

            var B = MathS.Matrices.Matrix(5, 3,
                2, 0, -2,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1,
                -2, 0, 2);

            var expected = MathS.Matrices.Matrix(1, 3,
                0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_2_0_n2_nr_1_0_n1_nr_0_0_0_nr_n1_0_1_n2_0_2_B_0_nr_0_nr_0() {
            var A = MathS.Matrices.Matrix(5, 3,
                2, 0, -2,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1,
                -2, 0, 2);

            var B = MathS.Vector(0, 0, 0);

            var expected = MathS.Vector(2, 1, 0, 1, 2);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}