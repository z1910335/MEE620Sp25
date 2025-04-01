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
    double phi;    // angle of arm swing plane relative to vertical
    double cosPhi;
    double sinPhi;

    enum ShoulderDynamics{
        Free,        // Free to respond to the dynamics of the doll
        Prescribed,  // Prescribed by user input
    }
    ShoulderDynamics shoulderDyn;

    LinSysEq sys;

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

        phi = 0.0;
        cosPhi = Math.Cos(phi);
        sinPhi = Math.Sin(phi);

        shoulderDyn = ShoulderDynamics.Free;

        SetRHSFunc(RHSFuncPlayter);

        Reinitialize();

        
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
    }  // end Reinitialize

    //------------------------------------------------------------------------
    // Getters/Setters
    //------------------------------------------------------------------------

    // arm mass -----------------------------
    public double ArmMass
    {
        get { return mA; }
        set { 
            if(value > 0.01 && value < 2.0)
                mA = value; 
        }
    }

    // Radius of Gyration  ---------------
    public double RadiusOfGyration
    {
        get { return rho;}

        set{
            if(value > 0.05)
                rho = value;
        }
    }

    // GammaY - Ratio of moments of inertia --------
    public double GammaY
    {
        get { return gammaY; }

        set {
            if (value > 0.01)
                gammaY = value;
        }
    }

    // GammaZ - Ratio of moments of inertia --------
    public double GammaZ
    {
        get { return gammaZ; }

        set {
            if (value > 0.01)
                gammaZ = value;
        }
    }

    // ShoulderHeight -------------------
    public double ShoulderHeight
    {
        get{ return h;}

        set{ h = value;}
    }

    // ArmLength -----------------------
    public double ArmLength
    {
        get{ return L; }

        set{
            if(value > 0.1)
                L = value;
        }
    }

    // Phi ---------------------------
    public double Phi
    {
        get{ return phi; }

        set{
            phi = value;
            cosPhi = Math.Cos(phi);
            sinPhi = Math.Sin(phi);
        }
    }

}// end class