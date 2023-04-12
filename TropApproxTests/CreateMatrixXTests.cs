using AngouriMath;
using TropApprox;

namespace TropApproxTests {
    [TestClass]
    public class CreateMatrixXTests {
        [TestMethod]
        public void CreateMatrixX_M_0_x_n1_1() {
            // Arenge 
            int M = 0;
            var vectorX = MathS.Vector(-1, 1);

            var expected = MathS.Matrices.Matrix(2, 1,
                0,
                0);

            // Act
            var given = Approx.CreateMatrixX(vectorX, M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_0_x_n1_0_1() {
            // Arenge 
            int M = 0;
            var vectorX = MathS.Vector(-1, 0, 1);

            var expected = MathS.Matrices.Matrix(3, 1,
                0,
                0,
                0);

            // Act
            var given = Approx.CreateMatrixX(vectorX, M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_1_x_n1_0_1() {
            // Arenge 
            int M = 1;
            var vectorX = MathS.Vector(-1, 0, 1);

            var expected = MathS.Matrices.Matrix(3, 3,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }
        
        [TestMethod]
        public void CreateMatrixX_M_1_x_n2_n1_0_1_2() {
            // Arenge 
            int M = 1;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 3,
                2, 0, -2,
                1, 0, -1,
                0, 0, 0,
                -1, 0, 1,
                -2, 0, 2);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_2_x_n2_n1_0_1_2() {
            // Arenge 
            int M = 2;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 5,
                4, 2, 0, -2, -4,
                2, 1, 0, -1, -2,
                0, 0, 0, 0, 0,
                -2, -1, 0, 1, 2,
                -4, -2, 0, 2, 4);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_2_x_n2_n1d5_n1_n0d5_0_0d5_1_1d5_2() {
            // Arenge 
            int M = 2;
            var vectorX = MathS.Vector(-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2);

            var expected = MathS.Matrices.Matrix(9, 5,
                4, 2, 0, -2, -4,
                3, (double)1.5, 0, -1.5, -3,
                2, 1, 0, -1, -2,
                1, (double)0.5, 0, -0.5, -1,
                0, 0, 0, 0, 0,
                -1, -0.5, 0, (double)0.5, 1,
                -2, -1, 0, 1, 2,
                -3, -1.5, 0, (double)1.5, 3,
                -4, -2, 0, 2, 4);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_3_x_n2_n1_0_1_2() {
            // Arenge 
            int M = 3;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 7,
                6, 4, 2, 0, -2, -4, -6,
                3, 2, 1, 0, -1, -2, -3,
                0, 0, 0, 0, 0, 0, 0,
                -3, -2, -1, 0, 1, 2, 3,
                -6, -4, -2, 0, 2, 4, 6);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_3_x_n2_n1d5_n1_n0d5_0_0d5_1_1d5_2() {
            // Arenge 
            int M = 3;
            var vectorX = MathS.Vector(-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2);

            var expected = MathS.Matrices.Matrix(9, 7,
                6, 4, 2, 0, -2, -4, -6,
                (double)4.5, 3, (double)1.5, 0, -1.5, -3, -4.5,
                3, 2, 1, 0, -1, -2, -3,
                (double)1.5, 1, (double)0.5, 0, -0.5, -1, -1.5,
                0, 0, 0, 0, 0, 0, 0,
                -1.5, -1, -0.5, 0, (double)0.5, 1, (double)1.5,
                -3, -2, -1, 0, 1, 2, 3,
                -4.5, -3, -1.5, 0, (double)1.5, 3, (double)4.5,
                -6, -4, -2, 0, 2, 4, 6);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_2_x_pi_1d5pi_2pi() {
            // Arenge 
            int M = 2;
            var vectorX = MathS.Vector(3.14, 1.5*3.14, 2*3.14);

            var expected = MathS.Matrices.Matrix(3, 5,
                -6.28, -3.14, 0, (double)3.14, (double)6.28,
                -9.42, -4.71, 0, (double)4.71, (double)9.42,
                -12.56, -6.28, 0, (double)6.28, (double)12.56);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_2_x_pi_1d25pi_1d5pi_1d75pi_2pi() {
            // Arenge 
            int M = 2;
            var vectorX = MathS.Vector(3.14, 1.25 * 3.14, 1.5 * 3.14, 1.75 * 3.14, 2 * 3.14);

            var expected = MathS.Matrices.Matrix(5, 5,
                -6.28, -3.14, 0, (double)3.14, (double)6.28,
                -2.5*3.14, -1.25*3.14, 0, (double)1.25*3.14, (double)2.5 * 3.14,
                -9.42, -4.71, 0, (double)4.71, (double)9.42,
                -3.5*3.14, -1.75*3.14, 0, (double)1.75*3.14, (double)3.5 * 3.14,
                -12.56, -6.28, 0, (double)6.28, (double)12.56);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_2_d_2_x_n2_n1_0_1_2() {
            // Arenge 
            int M = 4;
            int d = 2;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 9,
                4, 3, 2, 1, 0, -1, -2, -3, -4,
                2, (double)1.5, 1, (double)0.5, 0, -0.5, -1, -1.5, -2,
                0, 0, 0, 0, 0, 0, 0, 0, 0,
                -2, -1.5, -1, -0.5, 0, (double)0.5, 1, (double)1.5, 2,
                -4, -3, -2, -1, 0, 1, 2, 3, 4);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M, d);

            // Assert
            Assert.AreEqual(expected, given);
        }

        [TestMethod]
        public void CreateMatrixX_M_3_d_2_x_n2_n1_0_1_2() {
            // Arenge 
            int M = 3;
            int d = 3;
            var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

            var expected = MathS.Matrices.Matrix(5, 7,
                2, 4/3d, 2/3d, 0, -2/3d, -4/3d, -2,
                1, 2/3d, 1/3d, 0, -1/3d, -2/3d, -1,
                0, 0, 0, 0, 0, 0, 0,
                -1, -2/3d, -1/3d, 0, 1/3d, 2/3d, 1,
                -2, -4/3d, -2/3d, 0, 2/3d, 4/3d, 2);

            // Act
            var given = Approx.CreateMatrixX(vectorX, -M, M, d);

            // Assert
            Assert.AreEqual(expected, given);
        }
    }
}