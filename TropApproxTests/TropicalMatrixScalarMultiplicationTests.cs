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

        [TestMethod]
        public void TropicalMatrixScalarMultiplication_A_n1_n2_zero_nr_5_2_n1_nr_3_0_1_B_n3() {
            using var _ = Settings.DowncastingEnabled.Set(false);

            var A = MathS.Matrices.Matrix(3, 3,
                2, 1, Current.Algebra.Zero,
                5, 2, -1,
                3, 0, 1);

            Number.Real B = -3;

            var expected = MathS.Matrices.Matrix(3, 3,
                -1, -2, Current.Algebra.Zero,
                2, -1, -4,
                0, -3, -2);

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
        public void TropicalMatrixScalarMultiplication_A_n1_n2_zero_nr_5_2_n1_nr_3_0_1_B_n3r() {
            using var _ = Settings.DowncastingEnabled.Set(false);

            var A = MathS.Matrices.Matrix(3, 3,
                2, 1, Current.Algebra.Zero,
                5, 2, -1,
                3, 0, 1);

            var SpectralRadius = TropicalMatrixOperations.GetSpectralRadius(A);

            var SpectralRadiusInversed = (Number.Real)Current.Algebra.Calculate($"({SpectralRadius})^(-1)");

            var expected = MathS.Matrices.Matrix(3, 3,
                -1, -2, Current.Algebra.Zero,
                2, -1, -4,
                0, -3, -2);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, SpectralRadiusInversed);

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