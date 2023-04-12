using AngouriMath;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class PseudoInverseVectorTests {
        [TestMethod]
        public void PseudoInverseVector_1_1() {
            var vector = MathS.Vector(1, 1);

            var expected = MathS.Matrices.Matrix(1, 2,
                -1,
                -1);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_1_0_1() {
            var vector = MathS.Vector(1, 0, 1);

            var expected = MathS.Matrices.Matrix(1, 3,
                -1,
                0,
                -1);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_4_nr_1_nr_0_nr_1_nr_4() {
            var vector = MathS.Vector(4, 1, 0, 1, 4);

            var expected = MathS.Matrices.Matrix(1, 5,
                -4, -1, 0, -1, -4);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_4_2d25_1_0d25_0_0d25_1_2d25_4() {
            var vector = MathS.Vector(4, 2.25, 1, 0.25, 0, 0.25, 1, 2.25, 4);

            var expected = MathS.Matrices.Matrix(1, 9,
                -4,
                -2.25,
                -1,
                -0.25,
                0,
                -0.25,
                -1,
                -2.25,
                -4);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_0_n1_0() {
            var vector = MathS.Vector(0, -1, 0);

            var expected = MathS.Matrices.Matrix(1, 3,
                0,
                1,
                0);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_0_nsqrt2div2_n1_nsqrt2div2_0() {
            var vector = MathS.Vector(0, -0.707106, -1, -0.707106, 0);

            var expected = MathS.Matrices.Matrix(1, 5,
                0,
                0.707106,
                1,
                0.707106,
                0);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_n1() {
            var vector = MathS.Vector(-1);

            var expected = MathS.Matrices.Matrix(1, 1,
                1);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_0() {
            var vector = MathS.Vector(0);

            var expected = MathS.Vector(0);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_0_0_0() {
            var vector = MathS.Vector(0, 0, 0);

            var expected = MathS.Matrices.Matrix(1, 3,
                0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_0_nr_0_nr_0() {
            var vector = MathS.Matrices.Matrix(1, 3,
                0, 0, 0);

            var expected = MathS.Vector(0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void PseudoInverseVector_1_nr_0_nr_1() {
            var vector = MathS.Vector(1, 0, 1);

            var expected = MathS.Matrices.Matrix(1, 3, 
                -1, 0, -1);

            // Act
            var given = TropicalMatrixOperations.PseudoInverse(vector);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}