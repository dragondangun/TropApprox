using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class TropicalMatrixScalarMultiplicationTests {
        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_nr_0_B_1() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var A = MathS.Matrices.Matrix(2, 1,
                0,
                0);
            var B = MathS.Vector(1);

            var expected = MathS.Vector(1, 1);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            bool result = true;
            for(int i = 0; i < given.RowCount; i++) {
                for(int j = 0; j < given.ColumnCount; j++) {
                    var r = (Number.Real)expected[i, j];
                    var l = (Number.Real)given[i, j];
                    if(r > l || l > r) {
                        result = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_nr_0_nr_0_B_0() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var A = MathS.Matrices.Matrix(3, 1,
                0,
                0,
                0);
            var B = MathS.Vector(0);

            var expected = MathS.Vector(0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            bool result = true;
            for(int i = 0; i < given.RowCount; i++) {
                for(int j = 0; j < given.ColumnCount; j++) {
                    var r = (Number.Real)expected[i, j];
                    var l = (Number.Real)given[i, j];
                    if(r > l || l > r) {
                        result = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_nr_0_nr_0_B_1() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var A = MathS.Vector(0, 0, 0);
            var B = MathS.Vector(0.5);

            var expected = MathS.Vector(0.5, 0.5, 0.5);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            bool result = true;
            for(int i = 0; i < given.RowCount; i++) {
                for(int j = 0; j < given.ColumnCount; j++) {
                    var r = (Number.Real)expected[i, j];
                    var l = (Number.Real)given[i, j];
                    if(r > l || l > r) {
                        result = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_0_B_0_nr_0_nr_0() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            var A = MathS.Vector(0);
            var B = MathS.Vector(0, 0, 0);

            var expected = MathS.Vector(0, 0, 0);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, B);

            // Assert
            bool result = true;
            for(int i = 0; i < given.RowCount; i++) {
                for(int j = 0; j < given.ColumnCount; j++) {
                    var r = (Number.Real)expected[i, j];
                    var l = (Number.Real)given[i, j];
                    if(r > l || l > r) {
                        result = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(result);
        }
    }
}