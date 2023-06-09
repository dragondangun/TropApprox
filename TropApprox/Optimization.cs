﻿using AngouriMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using static AngouriMath.MathS;
using TMO = TropApprox.TropicalMatrixOperations;

namespace TropApprox {
    public static class Optimization {

        public static void SolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            out Number.Real Delta,
            Algebra algebra,
            Entity.Matrix? x_0 = null 
        ) 
        {
            using var _ = Settings.DowncastingEnabled.Set(false);
            int i = 0;

            List<Entity.Matrix> x = new() {
                x_0 ?? GetInitialVector(A.ColumnCount, algebra),
            };

            List<Entity.Matrix> y = new();

            Number.Real sqrtDelta;

            while(true) {
                var Ax_i = TMO.TropicalMatrixMultiplication(A, x[i], algebra);
                var Ax_iPIed = TMO.PseudoInverse(Ax_i, algebra);
                var Ax_iPIedB = TMO.TropicalMatrixMultiplication(Ax_iPIed, B, algebra);
                var important = TMO.PseudoInverse(Ax_iPIedB, algebra);
                var BImportant = TMO.TropicalMatrixMultiplication(B, important, algebra);
                var BImportantPIed = TMO.PseudoInverse(BImportant, algebra);
                var BImportantPIedA = TMO.TropicalMatrixMultiplication(BImportantPIed, A, algebra);
                Delta = (Number.Real)TMO.TropicalMatrixMultiplication(BImportantPIedA, x[i], algebra)[0, 0];
                sqrtDelta = (Number.Real)algebra.Calculate($"{Delta}^(1/2)");

                var y_iplus1 = TMO.TropicalMatrixScalarMultiplication(important, sqrtDelta, algebra);
                y.Add(y_iplus1);

                if(StopCondition(Delta, y_iplus1, y, algebra)) {
                    break;
                }

                var By_i = TMO.TropicalMatrixMultiplication(B, y[i], algebra);
                var By_iPIed = TMO.PseudoInverse(By_i, algebra);
                var By_iPIedA = TMO.TropicalMatrixMultiplication(By_iPIed, A, algebra);
                important = TMO.PseudoInverse(By_iPIedA, algebra);
                var AImportant = TMO.TropicalMatrixMultiplication(A, important, algebra);
                var AImportantPIed = TMO.PseudoInverse(AImportant, algebra);
                var AImportantPIedB = TMO.TropicalMatrixMultiplication(AImportantPIed, B, algebra);
                Delta = (Number.Real)TMO.TropicalMatrixMultiplication(AImportantPIedB, y[i], algebra)[0, 0];
                sqrtDelta = (Number.Real)algebra.Calculate($"{Delta}^(1/2)");

                var x_iplus1 = TMO.TropicalMatrixScalarMultiplication(important, sqrtDelta, algebra);
                x.Add(x_iplus1);

                if(StopCondition(Delta, x_iplus1, x, algebra)) {
                    break;
                }

                ++i;
            }

            theta = x[^1];
            sigma = y[^1];
        }

        public static void SolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            out Number.Real Delta,
            Entity.Matrix? x_0 = null
        )
        => SolveTwoSidedEquation(A, B, out theta, out sigma, out Delta, Current.Algebra, x_0);

        public static void SolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            Entity.Matrix? x_0 = null
        )
        => SolveTwoSidedEquation(A, B, out theta, out sigma, out _, Current.Algebra, x_0);

        public static void SolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            Algebra algebra,
            Entity.Matrix? x_0 = null
        )
        => SolveTwoSidedEquation(A, B, out theta, out sigma, out _, algebra, x_0);

        public static void PipeSolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            out Number.Real Delta,
            Algebra algebra,
            Entity.Matrix? x_0 = null
        ) 
        {
            using var _ = Settings.DowncastingEnabled.Set(false);
            int i = 0;

            List<Entity.Matrix> x = new() {
                x_0 ?? GetInitialVector(A.ColumnCount, algebra),
            };

            List<Entity.Matrix> y = new();

            Number.Real sqrtDelta;

            while(true) {
                var important = A.TropicalMatrixMultiplication(x[i], algebra).PseudoInverse(algebra).TropicalMatrixMultiplication(B, algebra).PseudoInverse(algebra);
                Delta = (Number.Real)(B.TropicalMatrixMultiplication(important, algebra).PseudoInverse(algebra)
                    .TropicalMatrixMultiplication(A, algebra).TropicalMatrixMultiplication(x[i], algebra))[0, 0];
                sqrtDelta = (Number.Real)algebra.Calculate($"{Delta}^(1/2)");

                var y_iplus1 = important.TropicalMatrixScalarMultiplication(sqrtDelta, algebra);
                y.Add(y_iplus1);

                if(StopCondition(Delta, y_iplus1, y, algebra)) {
                    break;
                }

                important = B.TropicalMatrixMultiplication(y[i], algebra).PseudoInverse(algebra).TropicalMatrixMultiplication(A, algebra).PseudoInverse(algebra);
                Delta = (Number.Real)(A.TropicalMatrixMultiplication(important, algebra).PseudoInverse(algebra).
                    TropicalMatrixMultiplication(B, algebra).TropicalMatrixMultiplication(y[i], algebra))[0, 0];
                sqrtDelta = (Number.Real)algebra.Calculate($"{Delta}^(1/2)");

                var x_iplus1 = important.TropicalMatrixScalarMultiplication(sqrtDelta, algebra);
                x.Add(x_iplus1);

                if(StopCondition(Delta, x_iplus1, x, algebra)) {
                    break;
                }

                ++i;
            }

            theta = x[^1];
            sigma = y[^1];
        }

        public static void PipeSolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            out Number.Real Delta,
            Entity.Matrix? x_0 = null
        )
        => PipeSolveTwoSidedEquation(A, B, out theta, out sigma, out Delta, Current.Algebra, x_0);

        public static void PipeSolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            Entity.Matrix? x_0 = null
        )
        => PipeSolveTwoSidedEquation(A, B, out theta, out sigma, out _, Current.Algebra, x_0);

        public static void PipeSolveTwoSidedEquation
        (
            Entity.Matrix A, Entity.Matrix B,
            out Entity.Matrix theta, out Entity.Matrix sigma,
            Algebra algebra,
            Entity.Matrix? x_0 = null
        )
        => PipeSolveTwoSidedEquation(A, B, out theta, out sigma, out _, algebra, x_0);

        private static bool StopCondition(Number.Real Delta, Entity.Matrix vector, List<Entity.Matrix> vectorCollection, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            // We can't compare as ``Delta == algebra.One`` cause of internal logic of AngouriMath.
            // Read more in official AngouriMath's discord server: https://discord.com/channels/642350046213439489/897053152044535808/1097626035761197056
            if(!(Delta > algebra.One || Delta < algebra.One)) {
                return true;
            }
            // We can't optimize with hash table, cause of internal logic of AngouriMath.
            // We can get two matrices with the same elements, but hash will be different.
            // Read more in official AngouriMath's discord server: https://discord.com/channels/642350046213439489/897053152044535808/1097626035761197056
            for(int j = 0; j < vectorCollection.Count - 1; j++) {
                if(vector.IsEqualTo(vectorCollection[j])) {
                    return true;
                }
            }

            return false;
        }

        private static Entity.Matrix GetInitialVector(int size, Algebra algebra, Number.Real initialValue) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            algebra.Calculate(initialValue);

            var result = MathS.ZeroVector(size);
            for(int i = 0; i < size; i++) {
                result = result.WithElement(i, initialValue);
            }
            return result;
        }

        private static Entity.Matrix GetInitialVector(int size, Algebra algebra) {
            using var _ = Settings.DowncastingEnabled.Set(false);
            if(algebra.One > 0 || algebra.One < 0) {
                return TMO.GetVectorOfOnes(size, algebra);
            }

            try {
                algebra.Calculate(1);
            }
            catch(ArgumentException) {
                throw new NotImplementedException("If you semiring where 1 is not in the using set, use overloading with initial value");
            }

            var result = MathS.ZeroVector(size);
            for(int i = 0; i < size; i++) {
                result = result.WithElement(i, 1);
            }
            return result;
        }

        private static Entity.Matrix GetInitialVector(int size) => GetInitialVector(size, Current.Algebra);

        private static Entity.Matrix GetInitialVector(int size, Number.Real initialValue) => GetInitialVector(size, Current.Algebra, initialValue);
    }
}
