using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.MathS;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class GetZeroMatrixTests {
        [TestMethod]
        public void GetZeroMatrix_3() {
            // Arrange
            using var _ = Settings.DowncastingEnabled.Set(false);
            int n = 3;

            var expected = MathS.Matrices.Matrix(n, n,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero
                );

            // Act
            var given = TropicalMatrixOperations.GetZeroMatrix(n);

            // Assert
            Assert.IsTrue(expected.IsEqualTo(given));
        }

        [TestMethod]
        public void GetZeroMatrix_3_5() {
            // Arrange
            using var _ = Settings.DowncastingEnabled.Set(false);
            int rowCount = 3;
            int columnCount = 5;

            var expected = MathS.Matrices.Matrix(rowCount, columnCount,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero
                );

            // Act
            var given = TropicalMatrixOperations.GetZeroMatrix(rowCount, columnCount);

            // Assert
            Assert.IsTrue(expected.IsEqualTo(given));
        }

        [TestMethod]
        public void GetZeroMatrix_5_3() {
            // Arrange
            using var _ = Settings.DowncastingEnabled.Set(false);
            int rowCount = 5;
            int columnCount = 3;

            var expected = MathS.Matrices.Matrix(rowCount, columnCount,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.Zero
                );

            // Act
            var given = TropicalMatrixOperations.GetZeroMatrix(rowCount, columnCount);

            // Assert
            Assert.IsTrue(expected.IsEqualTo(given));
        }
    }
}
