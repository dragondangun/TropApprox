﻿using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.MathS;

namespace TropApprox {
    [TestClass]
    public class AreMatriciesEqualTests {
        [TestMethod]
        public void AreMatriciesEqual_A_1_2_3_nr_1_2_3_nr_1_2_3_B_1_2_3_nr_1_2_3_nr_1_2_3_T() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Matrices.Matrix(3, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            Entity.Matrix B = MathS.Matrices.Matrix(3, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreMatriciesEqual_A_1_2_3_nr_1_2_3_nr_1_2_3_B_1_2_3_nr_1_2_3_nr_1_2_3_nr_1_2_3_F() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Matrices.Matrix(3, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            Entity.Matrix B = MathS.Matrices.Matrix(4, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreMatriciesEqual_A_1_2_3_nr_1_2_3_nr_1_2_3_nr_1_2_3_B_1_2_3_nr_1_2_3_nr_1_2_3_F() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Matrices.Matrix(4, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            Entity.Matrix B = MathS.Matrices.Matrix(3, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreMatriciesEqual_A_1_2_3_nr_1_2_3_nr_1_2_3_B_1_2_3_4_nr_1_2_3_4_nr_1_2_3_4_F() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Matrices.Matrix(3, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            Entity.Matrix B = MathS.Matrices.Matrix(3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreMatriciesEqual_A_1_2_3_4_nr_1_2_3_4_nr_1_2_3_4_B_1_2_3_nr_1_2_3_nr_1_2_3_F() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Matrices.Matrix(3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4);

            Entity.Matrix B = MathS.Matrices.Matrix(3, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreMatriciesEqual_A_1d1_2d1_3d1_nr_n1_n2_n3_nr_1_2_3_B_1d1_2d1_3d1_nr_n1_n2_n3_nr_1_2_3_T() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Matrices.Matrix(3, 3,
                1.1, 2.1, 3.1,
                -1, -2, -3,
                1, 2, 3);

            Entity.Matrix B = MathS.Matrices.Matrix(3, 3,
                1.1, 2.1, 3.1,
                -1, -2, -3,
                1, 2, 3);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreMatriciesEqual_A_1d1_2d1_3d1_nr_n1_n2_n3_nr_1_2_3_B_1d1_2d1_3d1_nr_n1_2_n3_nr_1_2_3_F() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Matrices.Matrix(3, 3,
                1.1, 2.1, 3.1,
                -1, -2, -3,
                1, 2, 3);

            Entity.Matrix B = MathS.Matrices.Matrix(3, 3,
                1.1, 2.1, 3.1,
                -1, 2, -3,
                1, 2, 3);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreMatriciesEqual_A_1_nr_2_nr_3d1_B_1_nr_2_nr_3d1_T() {
            // Arenge
            using var _ = Settings.DowncastingEnabled.Set(false);
            Entity.Matrix A = MathS.Vector(1, 2, 3.1);

            Entity.Matrix B = MathS.Vector(1, 2, 3.1);

            // Act
            bool result = TropApprox.TropicalMatrixOperations.AreMatriciesEqual(A, B);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
