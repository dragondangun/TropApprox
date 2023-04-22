using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class TropicMatrixAdditionTests {
        [TestMethod]
        public void TropicMatrixAddition_A_n1_n2_nInf_nr_2_n1_n4_nr_0_n3_n2_B_0_n3_n6_nr_1_0_n5_nr_n1_n2_n4() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            var A = MathS.Matrices.Matrix(3, 3,
                -1, -2, Current.Algebra.Zero,
                2, -1, -4,
                0, -3, -2);

            var B = MathS.Matrices.Matrix(3, 3,
                0, -3, -6,
                1, 0, -5,
                -1, -2, -4);


            var expected = MathS.Matrices.Matrix(3, 3,
                0, -2, -6,
                2, 0, -4,
                0, -2, -2);

            // Act
            var given = TropicalMatrixOperations.TropicalMatrixAddition(A, B);

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