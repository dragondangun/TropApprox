using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class ApproximateFunctionTests {
        [TestMethod]
        public void ApproximateFunction_f_x2_M_0_x_n1_1() {
            throw new NotImplementedException();

            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 0;
            var vectorX = MathS.Vector(-1, 1);

            var expected = MathS.Vector(1);

            // Act
            //var given = Approx.ApproximateFunction(function, vectorX, M, M);

            // Assert
            //bool result = true;
            //for(int i = 0; i < given.RowCount; i++) {
            //    for(int j = 0; j < given.ColumnCount; j++) {
            //        var r = (Number.Real)expected[i, j];
            //        var l = (Number.Real)given[i, j];
            //        if(r > l || l > r) {
            //            result = false;
            //            break;
            //        }
            //    }
            //}

            //Assert.IsTrue(result);
        }

        
    }
}
