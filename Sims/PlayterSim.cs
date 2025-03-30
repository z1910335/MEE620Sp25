//============================================================================
// PlayterSim.cs
// Simulation of a Playter Doll.
//
// STUDENTS SHOULD NOT CHANGE ANYTHING IN THIS FILE
//============================================================================
using System;

public partial class PlayterSim : Simulator
{
    // Parameters
    double mA;     // dimensionless arm mass
    double rho;    // dimless radius if gyration for moment of inertia, I_Gx
    double gammaY; // ratio I_Gy/I_Gx
    double gammaZ; // ratio I_Gz/I_Gx
    double h;      // dimless vertical (b_y) distance of shoulder from body CG
    double L;      // dimless distance of arm mass from shoulder
    double k;      // dimless torsional stiffness of shoulder spring
    double c;      // dimless torsional damping coeff in shoulder damper

    enum ShoulderDynamics{
        Free,        // Free to respond to the dynamics of the doll
        Prescribed,  // Prescribed by user input
    }
    ShoulderDynamics shoulderDyn;

    //------------------------------------------------------------------------
    // Constructor 
    //------------------------------------------------------------------------
    public PlayterSim() : base(17)
    {
        // parameter values for doll in Playter's thesis
        mA = 0.107;
        rho = 2.21;
        gammaY = 0.091;
        gammaZ = 1.05;
        h = 1.56;
        L = 1.65;

        shoulderDyn = ShoulderDynamics.Free;
    }


    //------------------------------------------------------------------------
    // Reinitialize: reset initial condition when simulation get restarted
    //------------------------------------------------------------------------
    private void Reinitialize()
    {

        // Generalized Speeds
        x[0] = 1.0;      // omegaX
        x[1] = 0.0;      // omegaY
        x[2] = 0.01;     // omegaZ
        x[3] = 0.0;      // omegaFL
        x[4] = 0.0;      // omegaFR
        x[5] = 0.0;      // vx
        x[6] = 0.0;      // vy
        x[7] = 0.0;      // vz

        // Generalized Coordinates
        x[8]  = 1.0;      // q0
        x[9]  = 0.0;      // q1
        x[10] = 0.0;      // q2
        x[11] = 0.0;      // q3
        x[12] = 0.0;      // thetaL
        x[13] = 0.0;      // thetaR
        x[14] = 0.0;      // xG
        x[15] = 0.0;      // yG
        x[16] = 0.0;      // zG
    }
}