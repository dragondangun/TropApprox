using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class SolveTwoSidedEquationTests {
        [TestMethod]
        public void SolveTwoSidedEquationTest_A_3_zero_0_nr_1_1_0_zero_1_2_B_1_1_nr_3_2_nr_3_1_x0_5_3_1() {
            // Arrange
            using var _ = MathS.Settings.DowncastingEnabled.Set(false);
            Current.Algebra = MaxPlus.Instance;

            var A = MathS.Matrices.Matrix(3, 3,
                3, Current.Algebra.Zero, 0,
                1, 1, 0,
                Current.Algebra.Zero, 1, 2
                );

            var B = MathS.Matrices.Matrix(3, 2,
                1, 1,
                3, 2,
                3, 1
                );

            var x_0 = MathS.Vector(5, 3, 1);

            //var thetaExpected = MathS.Vector(4, 5, 4);
            var thetaExpected = MathS.Vector(3, 6, 5);
            //var sigmaExpected = MathS.Vector(3, 4);
            var sigmaExpected = MathS.Vector(4, 5);

            // Act
            Entity.Matrix theta;
            Entity.Matrix sigma;
            TropApprox.Optimization.SolveTwoSidedEquation(A, B, out theta, out sigma, x_0);

            // Assert
            bool isThetaCorrect = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(theta, thetaExpected);
            bool isSigmaCorrect = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(sigma, sigmaExpected);
            Assert.IsTrue(isThetaCorrect && isSigmaCorrect);
        }

        [TestMethod]
        public void SolveTwoSidedEquationTest_A_3_zero_0_nr_1_1_0_zero_1_3_B_1_1_nr_3_2_nr_1_1_x0_5_3_1() {
            // Arrange
            using var _ = MathS.Settings.DowncastingEnabled.Set(false);
            Current.Algebra = MaxPlus.Instance;

            var A = MathS.Matrices.Matrix(3, 3,
                3, Current.Algebra.Zero, 0,
                1, 1, 0,
                Current.Algebra.Zero, 1, 3
                );

            var B = MathS.Matrices.Matrix(3, 2,
                1, 1,
                3, 2,
                1, 1
                );

            var x_0 = MathS.Vector(5, 3, 1);

            //var thetaExpected = MathS.Vector(5, 5, 3);
            var thetaExpected = MathS.Vector(4, 6, 4);
            //var sigmaExpected = MathS.Vector(3.5, 4.5);
            var sigmaExpected = MathS.Vector(4.5, 5.5);

            // Act
            Entity.Matrix theta;
            Entity.Matrix sigma;
            TropApprox.Optimization.SolveTwoSidedEquation(A, B, out theta, out sigma, x_0);

            // Assert
            bool isThetaCorrect = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(theta, thetaExpected);
            bool isSigmaCorrect = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(sigma, sigmaExpected);
            Assert.IsTrue(isThetaCorrect && isSigmaCorrect);
        }
    }
}
