using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class ApproximateFunctionTests {
        [TestMethod]
        public void ApproximateFunction_f_x2_M_0_x_n1_1() {
            using var _ = Settings.DowncastingEnabled.Set(false);
            string function = "x^2";
            int M = 0;
            var vectorX = MathS.Vector(-1, 1);

            Entity.Matrix theta = MathS.Vector(1);
            Entity.Matrix sigma = MathS.Vector(0);

            // Act
            Approx.ApproximateFunction(function, vectorX, M, M, out Entity.Matrix expectedTheta, out Entity.Matrix expectedSigma);

            // Assert
            Assert.IsTrue(theta.IsEqualTo(expectedTheta) && sigma.IsEqualTo(expectedSigma));
        }

        
    }
}
