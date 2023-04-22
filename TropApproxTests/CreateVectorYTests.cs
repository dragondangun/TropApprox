using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class CreateVectorYTests {
        [TestMethod]
        public void CreateVectorY_y_x2_x_n1_1() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            var vectorX = MathS.Vector(-1, 1);

            var expected = MathS.Vector(1, 1);

            var function = "x^2";

            // Act
            var given = Approx.CreateVectorY(vectorX, function);

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
        public void CreateVectorY_y_x2_x_n1_0_1() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            var vectorX = MathS.Vector(-1, 0, 1);

            var expected = MathS.Vector(1, 0, 1);

            var function = "x^2";

            // Act
            var given = Approx.CreateVectorY(vectorX, function);

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
        public void CreateVectorY_y_x2_x_n2_n1_0_1_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Vector(4, 1, 0, 1, 4);

            var function = "x^2";

            // Act
            var given = Approx.CreateVectorY(vectorX, function);

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
        public void CreateVectorY_y_x2_x_n2_n1d5_n1_n0d5_0_0d5_1_1d5_2() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            var vectorX = MathS.Vector(-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2);

            var expected = MathS.Vector(4, 2.25, 1, 0.25, 0, 0.25, 1, 2.25, 4);

            var function = "x^2";

            // Act
            var given = Approx.CreateVectorY(vectorX, function);

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
        //public void CreateVectorY_y_sinx_x_pi_1d5pi_2pi() {
        //    // Arenge 
        //    using var _ = Settings.DowncastingEnabled.Set(false);
        //    var vectorX = MathS.Vector(Math.PI, 1.5 * Math.PI, 2 * Math.PI);

        //    var expected = MathS.Vector(0, -1, 0);

        //    var function = "sin(x)";

        //    // Act
        //    var given = Approx.CreateVectorY(vectorX, function);

        //    // Assert
        //    var resultV = given - expected;

        //    bool result = true;
        //    for(int i = 0; i < resultV.RowCount; i++) {
        //        if((MathS.Abs(resultV[i]) > 0.0001).EvalBoolean()) {
        //            result = false;
        //            break;
        //        }
        //    }

        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void CreateVectorY_y_sinx_x_pi_1d25pi_1d5pi_1d75pi_2pi() {
        //    // Arenge 
        //    using var _ = Settings.DowncastingEnabled.Set(false);
        //    var vectorX = MathS.Vector(Math.PI, 1.25 * Math.PI, 1.5 * Math.PI, 1.75 * Math.PI, 2 * Math.PI);

        //    var expected = MathS.Vector(0, -0.70710, -1, -0.70710, 0);

        //    var function = "sin(x)";

        //    // Act
        //    var given = Approx.CreateVectorY(vectorX, function);

        //    var resultV = given - expected;

        //    bool result = true;
        //    for(int i = 0; i < resultV.RowCount; i++) {
        //        if((MathS.Abs(resultV[i]) > 0.0001).EvalBoolean()) {
        //            result = false;
        //            break;
        //        }
        //    }

        //    // Assert
        //    Assert.IsTrue(result);
        //}
    }
}