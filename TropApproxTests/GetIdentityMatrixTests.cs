using AngouriMath;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class GetIdentityMatrixTests {
        [TestMethod]
        public void GetIdentityMatrix_n_3() {
            // Arenge 
            int n = 3;

            var expected = MathS.Matrices.Matrix(n, n,
                Current.Algebra.One, Current.Algebra.Zero, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.One, Current.Algebra.Zero,
                Current.Algebra.Zero, Current.Algebra.Zero, Current.Algebra.One);

            // Act
            var given = TropicalMatrixOperations.GetIdentityMatrix(n);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void GetIdentityMatrix_n_1() {
            // Arenge 
            int n = 1;

            var expected = MathS.Matrices.Matrix(n, n,
                Current.Algebra.One);

            // Act
            var given = TropicalMatrixOperations.GetIdentityMatrix(n);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}