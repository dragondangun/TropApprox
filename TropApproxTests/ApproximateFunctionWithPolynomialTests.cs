using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class ApproximateFunctionWithPolynomialTests {
        [TestMethod]
        public void ApproximateFunctionWithPolynomial_f_x2_M_0_x_n1_1() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 0;
            var vectorX = MathS.Vector(-1, 1);

            var expected = MathS.Vector(1);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, M, M);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_0_x_n1_0_1() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 0;
            var vectorX = MathS.Vector(-1, 0, 1);

            var expected = MathS.Vector(0.5);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, M, M);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_n1_1_x_n1_0_1() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 1;
            var vectorX = MathS.Vector(-1, 0, 1);

            var expected = MathS.Vector(0, 0, 0);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_n1_1_x_n2_n1_0_1_2() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 1;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Vector(1, 1, 1);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_n2_2_x_n2_n1_0_1_2() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 2;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Vector(-0.5, 0.5, 0.5, 0.5, -0.5);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_n2_2_x_n2_n1d5_n1_n0d5_0_0d5_1_1d5_2() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 2;
            var vectorX = MathS.Vector(-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2);

            var expected = MathS.Vector(-0.5, 0.25, 0.5, 0.25, -0.5);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_n3_3_x_n2_n1_0_1_2() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 3;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Vector(-2, -1, 0, 0, 0, -1, -2);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_n3_3_x_n2_n1d5_n1_n0d5_0_0d5_1_1d5_2() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 3;
            var vectorX = MathS.Vector(-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2);

            var expected = MathS.Vector(-2.125, -0.875, -0.125, 0.125, -0.125, -0.875, -2.125);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

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

        //[TestMethod]
        //public void ApproximateFunctionWithPolynomial_f_sinx_M_n2_2_x_3d14_4d71_6d28() {
        //    string function = "sin(x)";
        //    int M = 2;
        //    var vectorX = MathS.Vector(3.14159265, 4.71238898, 6.28318531);

        //    var expected = MathS.Vector(6.28318531, 3.14159265, -1, -6.28318531, -12.5663706);

        //    // Act
        //    var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

        //    // Assert
        //    bool result = true;
        //    for(int i = 0; i < given.RowCount; i++) {
        //        for(int j = 0; j < given.ColumnCount; j++) {
        //            var r = (Number.Real)expected[i, j];
        //            var l = (Number.Real)given[i, j];
        //            if(r > l || l > r) {
        //                result = false;
        //                break;
        //            }
        //        }
        //    }

        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void ApproximateFunctionWithPolynomial_f_sinx_M_n2_2_x_3d14_3d92_4d71_5d49_6d28() {
        //    string function = "sin(x)";
        //    int M = 2;
        //    var vectorX = MathS.Vector(3.14159265, 3.92699082, 4.71238898, 5.49778714, 6.28318531);

        //    var expected = MathS.Vector(6.322331, 3.18073834, -0.960854309, -6.24403962, -12.5272249);

        //    // Act
        //    var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M);

        //    // Assert
        //    bool result = true;
        //    for(int i = 0; i < given.RowCount; i++) {
        //        for(int j = 0; j < given.ColumnCount; j++) {
        //            var r = (Number.Real)expected[i, j];
        //            var l = (Number.Real)given[i, j];
        //            if(r > l || l > r) {
        //                result = false;
        //                break;
        //            }
        //        }
        //    }

        //    Assert.IsTrue(result);
        //}

        [TestMethod]
        public void ApproximateFunctionWithPolynomial_f_x2_M_n4_4_d_2_x_n2_n1_0_1_2() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 4;
            int d = 2;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Vector(-0.5, 0, 0.5, 0.5, 0.5, 0.5, 0.5, 0, -0.5);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M, d);

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
        public void ApproximateFunctionWithPolynomial_f_x2_M_n3_3_d_2_x_n2_n1_0_1_2() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 3;
            int d = 3;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Vector(1, 1, 1, 1, 1, 1, 1);

            // Act
            var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M, d);

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

        //[TestMethod]
        //public void ApproximateFunctionWithPolynomial_f_sinx_M_n4_4_d_2_x_3d14_3d92_4d71_5d49_6d28() {
        //    string function = "sin(x)";
        //    int M = 4;
        //    int d = 2;
        //    var vectorX = MathS.Vector(3.14159265, 3.92699082, 4.71238898, 5.49778714, 6.28318531);

        //    var expected = MathS.Vector(6.28318531, 4.71238898, 3.14159265, 1.25638863, -1, -3.45600035, -6.28318531, -9.42477796, -12.5663706);

        //    // Act
        //    var given = Approx.ApproximateFunctionWithPolynomial(function, vectorX, -M, M, d);

        //    // Assert
        //    bool result = true;
        //    for(int i = 0; i < given.RowCount; i++) {
        //        for(int j = 0; j < given.ColumnCount; j++) {
        //            var r = (Number.Real)expected[i, j];
        //            var l = (Number.Real)given[i, j];
        //            if(r > l || l > r) {
        //                result = false;
        //                break;
        //            }
        //        }
        //    }

        //    Assert.IsTrue(result);
        //}
    }
}
