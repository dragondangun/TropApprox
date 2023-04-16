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

        [TestMethod]
        public void TropicalMatrixMultiplication_A_n4_n1_0_n1_n4_B_4_2_0_n2_n4_nr_2_1_0_n1_n2_nr_0_0_0_0_0_nr_n2_n1_0_1_2_nr_n4_n2_0_2_4() {
            var A = MathS.Matrices.Matrix(1, 5,
                -4, -1, 0, -1, -4);

            var B = MathS.Matrices.Matrix(5, 5,
                4, 2, 0, -2, -4,
                2, 1, 0, -1, -2,
                0, 0, 0, 0, 0,
                -2, -1, 0, 1, 2,
                -4, -2, 0, 2, 4);

            var expected = MathS.Matrices.Matrix(1, 5,
                1, 0, 0, 0, 1);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void TropicalMatrixMultiplication_A_n4_n2d25_n1_n0d25_0_n0d25_n1_n2d25_n4_B_4_2_0_n2_n4_nr_3_1d5_0_n1d5_n3_nr_2_1_0_n1_n2_nr_1_0d5_0_n0d5_n1_nr_0_0_0_0_0_nr_n1_n0d5_0_0d5_1_nr_n2_n1_0_1_2_nr_n3_n1d5_0_1d5_3_n4_n2_0_2_4() {
            var A = MathS.Matrices.Matrix(1, 9,
                -4, -2.25, -1, -0.25, 0, -0.25, -1, -2.25, -4);

            var B = MathS.Matrices.Matrix(9, 5,
                4, 2, 0, -2, -4,
                3, 1.5, 0, -1.5, -3,
                2, 1, 0, -1, -2,
                1, 0.5, 0, -0.5, -1,
                0, 0, 0, 0, 0,
                -1, -0.5, 0, 0.5, 1,
                -2, -1, 0, 1, 2,
                -3, -1.5, 0, 1.5, 3,
                -4, -2, 0, 2, 4);

            var expected = MathS.Matrices.Matrix(1, 5,
                1, 0.25, 0, 0.25, 1);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }



        [TestMethod]
        public void TropicalMatrixMultiplication_A_4_2_0_n2_n4_nr_3_1d5_0_n1d5_n3_nr_2_1_0_n1_n2_nr_1_0d5_0_n0d5_n1_nr_0_0_0_0_0_nr_n1_n0d5_0_0d5_1_nr_n2_n1_0_1_2_nr_n3_n1d5_0_1d5_3_n4_n2_0_2_4_B_n1_nr_n0d25_nr_0_nr_n0d25_nr_n1() {
            var A = MathS.Matrices.Matrix(9, 5,
                4, 2, 0, -2, -4,
                3, 1.5, 0, -1.5, -3,
                2, 1, 0, -1, -2,
                1, 0.5, 0, -0.5, -1,
                0, 0, 0, 0, 0,
                -1, -0.5, 0, 0.5, 1,
                -2, -1, 0, 1, 2,
                -3, -1.5, 0, 1.5, 3,
                -4, -2, 0, 2, 4);

            var B = MathS.Vector(-1, -0.25, 0, -0.25, -1);

            var expected = MathS.Vector(3, 2, 1, 0.25, 0, 0.25, 1, 2, 3);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixMultiplication(A, B);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}