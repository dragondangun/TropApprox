using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropApprox;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class GetVectorOfOnesTests {
        [TestMethod]
        public void GetVectorOfOnes_5() {
            // Arrange
            using var _ = Settings.DowncastingEnabled.Set(false);
            int n = 5;

            var expected = MathS.Vector(Current.Algebra.One, Current.Algebra.One, Current.Algebra.One, Current.Algebra.One, Current.Algebra.One);
            
            // Act
            var given = TropicalMatrixOperations.GetVectorOfOnes(n);

            // Assert
            Assert.IsTrue(TropicalMatrixOperations.AreMatriciesEqual(given, expected)); 
        }

        [TestMethod]
        public void GetVectorOfOnes_5_MinPlus() {
            // Arrange
            using var _ = Settings.DowncastingEnabled.Set(false);
            Current.Algebra = MinPlus.Instance;
            int n = 5;

            var expected = MathS.Vector(Current.Algebra.One, Current.Algebra.One, Current.Algebra.One, Current.Algebra.One, Current.Algebra.One);

            // Act
            var given = TropicalMatrixOperations.GetVectorOfOnes(n);

            // Assert
            Assert.IsTrue(TropicalMatrixOperations.AreMatriciesEqual(given, expected));
        }
    }
}
