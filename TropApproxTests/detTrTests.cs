using AngouriMath;
using TropApprox;
using static AngouriMath.Entity;
using static AngouriMath.MathS;

namespace TropApproxTests {
    [TestClass]
    public class TrTests {
        [TestMethod]
        public void Tr_A_2_1_nInf_nr_5_2_n1_nr_3_0_1() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);

            var A = MathS.Matrices.Matrix(3, 3,
                2, 1, Current.Algebra.Zero,
                5, 2, -1,
                3, 0, 1);


            Number.Real expected = 8;

            // Act
            Number.Real given = (Number.Real)TropicalMatrixOperations.Tr(A);

            // Assert
            bool result = !((expected > given) || (given > expected));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Tr_nInf_1_1_nr_nInf_nInf_1_nr_n2_n1_nInf() {
            // Arenge 
            using var _ = Settings.DowncastingEnabled.Set(false);
            var A = MathS.Matrices.Matrix(3, 3,
                Current.Algebra.Zero, 1, 1,
                Current.Algebra.Zero, Current.Algebra.Zero, 1,
                -2, -1, Current.Algebra.Zero);


            Number.Real expected = 0;

            // Act
            Number.Real given = (Number.Real)TropicalMatrixOperations.Tr(A);

            // Assert
            bool result = !((expected > given) || (given > expected));
            Assert.IsTrue(result);
        }
    }
}