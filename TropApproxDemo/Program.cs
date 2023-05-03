// See https://aka.ms/new-console-template for more information

using static AngouriMath.Entity;
using TropApprox;
using AngouriMath;
using static AngouriMath.MathS;

using var _ = Settings.DowncastingEnabled.Set(false);

// Convex function approximation

string function = "x^2";
int M = 2;
var vectorX = MathS.Vector(-2, -1, 0, 1, 2);

var coefs = Approx.ApproximateFunction(function, vectorX, -M, M);

var pol = TropicalPolynomial.CreatePolynomial(coefs, -M, M);

// Opt solution

// Без ограничений

var A = MathS.Matrices.Matrix(3, 3,
    2, 1, Current.Algebra.Zero,
    5, 2, -1,
    3, 0, 1);

var SpectralRadius = TropicalMatrixOperations.GetSpectralRadius(A);

var SpectralRadiusInversed = (Number.Real)Current.Algebra.Calculate($"({SpectralRadius})^(-1)");

var ASpectralRadiusInversed = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, SpectralRadiusInversed);

Number.Real? ATrValue = null;

Entity.Matrix KleeneStarA = null;
try {
    KleeneStarA = TropicalMatrixOperations.KleeneStar(ASpectralRadiusInversed, out ATrValue);
}
catch(ArgumentException ae) {
    // Этот блок не вызывается
    Console.WriteLine(ae.Message);
    Environment.Exit(1); // Аварийно прекращаем работу
}

Console.WriteLine(ATrValue.ToString());

Console.WriteLine(KleeneStarA.ToString(true));


// Решение на ограничения

var C = MathS.Matrices.Matrix(3, 3,
                Current.Algebra.Zero, 1, 1,
                Current.Algebra.Zero, Current.Algebra.Zero, 1,
                -2, -1, Current.Algebra.Zero);

Number.Real? СTrValue = null; // Пустое значение
Entity.Matrix? KleeneStarС = null;
try {
    KleeneStarС = TropicalMatrixOperations.KleeneStar(C, out СTrValue);
}
catch(ArgumentException ae) {
    // Этот блок не вызывается
    Console.WriteLine(ae.Message);
    Environment.Exit(1);
}

Console.WriteLine(СTrValue.ToString()); // 0

Console.WriteLine(KleeneStarС.ToString(true));

// Решение с ограничениями

var AC = TropicalMatrixOperations.TropicalMatrixMultiplication(A, C);
var ACsquare = TropicalMatrixOperations.
    TropicalMatrixMultiplication(AC, C);
var AsquareC = TropicalMatrixOperations.TropicalMatrixMultiplication(A, AC);
var ACA = TropicalMatrixOperations.TropicalMatrixMultiplication(AC, A);

Number.Real trAC = TropicalMatrixOperations.tr(AC);
Number.Real trACsquare = TropicalMatrixOperations.tr(ACsquare);
Number.Real trAsquareC = TropicalMatrixOperations.tr(AsquareC);
Number.Real trACA = TropicalMatrixOperations.tr(ACA);

Number.Real right = (Number.Real)Current.Algebra.Calculate($"({trAC})+({trACsquare})+({trAsquareC})^(1/2)+{trACA}^(1/2)");

Number.Real theta = (Number.Real)Current.Algebra.Calculate($"({SpectralRadius})+({right})");

Console.WriteLine($"{theta}"); // 6

Number.Real thetaInversed = (Number.Real)Current.Algebra.Calculate($"({theta})^(-1)");

var thetaInversedA = TropicalMatrixOperations.TropicalMatrixScalarMultiplication(A, thetaInversed);

var thetaInversedAplusC = TropicalMatrixOperations.TropicalMatrixAddition(thetaInversedA, C);

Number.Real? TrThetaInversedAplusC = null;
Entity.Matrix? KleeneStarThetaInversedAplusC = null;

try {
    KleeneStarThetaInversedAplusC = TropicalMatrixOperations.KleeneStar(thetaInversedAplusC, out TrThetaInversedAplusC);
}
catch(ArgumentException ae) {
    Console.WriteLine(ae.Message);
    Environment.Exit(1);
}
Console.WriteLine(KleeneStarThetaInversedAplusC.ToString(true));

Console.WriteLine("Press any key to exit");
Console.ReadKey();
