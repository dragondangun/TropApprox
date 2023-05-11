using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;
using TMO = TropApprox.TropicalMatrixOperations;

namespace TropApprox {
    public static class MatrixExtension {
        public static Entity.Matrix PseudoInverse(this Entity.Matrix matrix, Algebra algebra) => TMO.PseudoInverse(matrix, algebra);

        public static Entity.Matrix PseudoInverse(this Entity.Matrix matrix) => TMO.PseudoInverse(matrix, Current.Algebra);

        public static Entity.Matrix TropicalMatrixMultiplication(this Entity.Matrix matrixA, Entity.Matrix matrixB, Algebra algebra)
            => TMO.TropicalMatrixMultiplication(matrixA, matrixB, algebra);


        public static Entity.Matrix TropicalMatrixMultiplication(this Entity.Matrix matrixA, Entity.Matrix matrixB)
            => TMO.TropicalMatrixMultiplication(matrixA, matrixB, Current.Algebra);

        public static Entity.Matrix TropicalMatrixScalarMultiplication(this Entity.Matrix matrix, Number.Real scalar, Algebra algebra)
            => TMO.TropicalMatrixScalarMultiplication(matrix, scalar, algebra);
        }
    }
