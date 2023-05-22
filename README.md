There's DLL for tropical math, based on AngouriMath. 

Solution structure:
* TropApprox -- DLL development
* TropApproxDemo -- Console application for demonstration of TropApprox's possibilities
* TropApproxTests -- tests for TropApprox's functions

TropApprox DLL can:
* calculate in terms of tropical math 
    * MaxPlus-, MinPlus-, MaxTimes- and MinTimes- algebras are built in,
* do next matrix operations:
    * find track,
    * find tropical determinant analogue,
    * find spectral radius,
    * create zero matrix,
    * create identity matrix,
    * add matrices,
    * multiplicate matrices,
    * multiplicate scalar and matrix,
    * check on equality matrices,
* approximate
    * convex functions with tropical polynomial (in terms of MaxPLus- and MinPlus- algebras),
    * elementary continuous functions with tropical rational function (in terms of MaxPlus-algebra),
* solve two sided vector equations or find pseudo-solution.

Also TropApprox have extension class for AngouriMath's Entity.Matrix that allow to write more fluent-styled code

[![DOI](https://zenodo.org/badge/625579483.svg)](https://zenodo.org/badge/latestdoi/625579483)