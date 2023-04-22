using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class KleeneStarTests {
        [TestMethod]
        public void KleeneStar_nInf_1_1_nr_nInf_nInf_1_nr_n2_n1_nInf() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);

            var A = MathS.Matrices.Matrix(3, 3,
                Current.Algebra.Zero, 1, 1,
                Current.Algebra.Zero, Current.Algebra.Zero, 1,
                -2, -1, Current.Algebra.Zero);

            var expected = MathS.Matrices.Matrix(3, 3,
                0, 1, 2,
                -1, 0, 1,
                -2, -1, 0);

            // Act
            var given = TropicalMatrixOperations.KleeneStar(A);

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