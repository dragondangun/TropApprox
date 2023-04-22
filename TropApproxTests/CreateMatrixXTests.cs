using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class CreateMatrixXTests {
        [TestMethod]
        public void CreateMatrixX_M_0_x_n1_1() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 0;
            var vectorX = MathS.Vector(-1, 1);

            var expected = MathS.Matrices.Matrix(2, 1,
                0,
                0);

            // Act
            var given = Approx.CreateMatrixX(vectorX, M, M);

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
        public void CreateMatrixX_M_0_x_n1_0_1() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 0;
            var vectorX = MathS.Vector(-1, 0, 1);

            var expected = MathS.Matrices.Matrix(3, 1,
                0,
                0,
                0);

            // Act
            var given = Approx.CreateMatrixX(vectorX, M, M);

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
        public void CreateMatrixX_M_1_x_n1_0_1() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 1;
            var vectorX = MathS.Vector(-1, 0, 1);

            var expected = MathS.Matrices.Matrix(3, 3,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        public void CreateMatrixX_M_1_x_n2_n1_0_1_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 1;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 3,
                2, 0, -2,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1,
                -2, 0, 2);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        public void CreateMatrixX_M_2_x_n2_n1_0_1_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 2;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 5,
                4, 2, 0, -2, -4,
                2, 1, 0, -1, -2,
                0, 0, 0, 0, 0,
                -2, -1, 0, 1, 2,
                -4, -2, 0, 2, 4);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        public void CreateMatrixX_M_2_x_n2_n1d5_n1_n0d5_0_0d5_1_1d5_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 2;
            var vectorX = MathS.Vector(-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2);

            var expected = MathS.Matrices.Matrix(9, 5,
                4, 2, 0, -2, -4,
                3, 1.5, 0, -1.5, -3,
                2, 1, 0, -1, -2,
                1, 0.5, 0, -0.5, -1,
                0, 0, 0, 0, 0,
                -1, -0.5, 0, 0.5, 1,
                -2, -1, 0, 1, 2,
                -3, -1.5, 0, 1.5, 3,
                -4, -2, 0, 2, 4);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        public void CreateMatrixX_M_3_x_n2_n1_0_1_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 3;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 7,
                6, 4, 2, 0, -2, -4, -6,
                3, 2, 1, 0, -1, -2, -3,
                0, 0, 0, 0, 0, 0, 0,
                -3, -2, -1, 0, 1, 2, 3,
                -6, -4, -2, 0, 2, 4, 6);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        public void CreateMatrixX_M_3_x_n2_n1d5_n1_n0d5_0_0d5_1_1d5_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 3;
            var vectorX = MathS.Vector(-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2);

            var expected = MathS.Matrices.Matrix(9, 7,
                6, 4, 2, 0, -2, -4, -6,
                4.5, 3, 1.5, 0, -1.5, -3, -4.5,
                3, 2, 1, 0, -1, -2, -3,
                1.5, 1, 0.5, 0, -0.5, -1, -1.5,
                0, 0, 0, 0, 0, 0, 0,
                -1.5, -1, -0.5, 0, 0.5, 1, 1.5,
                -3, -2, -1, 0, 1, 2, 3,
                -4.5, -3, -1.5, 0, 1.5, 3, 4.5,
                -6, -4, -2, 0, 2, 4, 6);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        //public void CreateMatrixX_M_2_x_pi_1d5pi_2pi() {
        //    // Arenge 
        //    using var _ = Settings.DowncastingEnabled.Set(false);
        //    int M = 2;
        //    var vectorX = MathS.Vector(3.14, 1.5*3.14, 2*3.14);

        //    var expected = MathS.Matrices.Matrix(3, 5,
        //        -6.28, -3.14, 0, (double)3.14, (double)6.28,
        //        -9.42, -4.71, 0, (double)4.71, (double)9.42,
        //        -12.56, -6.28, 0, (double)6.28, (double)12.56);

        //    // Act
        //    var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        //public void CreateMatrixX_M_2_x_pi_1d25pi_1d5pi_1d75pi_2pi() {
        //    // Arenge 
        //    using var _ = Settings.DowncastingEnabled.Set(false);
        //    int M = 2;
        //    var pi = MathS.DecimalConst.pi;
        //    var vectorX = MathS.Vector(pi, (Entity)1.25 * pi, (Entity)1.5 * pi, (Entity)1.75 * pi, 2 * pi);

        //    var expected = MathS.Matrices.Matrix(5, 5,
        //        -2* pi, -pi, 0, pi, 2* pi,
        //        -(Entity)2.5*pi, -(Entity)1.25 *pi, 0, (Entity)1.25 *pi, (Entity)2.5 * pi,
        //        -3* pi, -(Entity)1.5 * pi, 0, (Entity)1.5 * pi, 3* pi,
        //        -(Entity)3.5 *pi, -(Entity)1.75 *pi, 0, (Entity)1.75 * pi, (Entity)3.5 * pi,
        //        -4* pi, -2* pi, 0, 2* pi, 4 * pi);

        //    // Act
        //    var given = Approx.CreateMatrixX(vectorX, -M, M);

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
        public void CreateMatrixX_M_2_d_2_x_n2_n1_0_1_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 4;
            int d = 2;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 9,
                4, 3, 2, 1, 0, -1, -2, -3, -4,
                2, 1.5, 1, 0.5, 0, -0.5, -1, -1.5, -2,
                0, 0, 0, 0, 0, 0, 0, 0, 0,
                -2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2,
                -4, -3, -2, -1, 0, 1, 2, 3, 4);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M, d);

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
        public void CreateMatrixX_M_3_d_2_x_n2_n1_0_1_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            int M = 3;
            int d = 3;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 7,
                2, 4/3.0, 2/3.0, 0, -2/3.0, -4/3.0, -2,
                1, 2/3.0, 1/3.0, 0, -1/3.0, -2/3.0, -1,
                0, 0, 0, 0, 0, 0, 0,
                -1, -2/3.0, -1/3.0, 0, 1/3.0, 2/3.0, 1,
                -2, -4/3.0, -2/3.0, 0, 2/3.0, 4/3.0, 2);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M, d);

            // Assert
            bool result = true;
            for(int i = 0; i < given.RowCount; i++) {
                for(int j = 0; j < given.ColumnCount; j++) {
                    var r = (Number.Real)expected[i, j];
                    var l = (Number.Real)given[i, j];
                    if(r - l > 0.0000001) {
                        result = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(result);
        }
    }
}