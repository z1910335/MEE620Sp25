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

    // generalized speeds
    double omegaX;
    double omegaY;
    double omegaZ;
    double omegaFL;
    double omegaFR;
    double vx;
    double vy;
    double vz;

    // generalized coordinates
    double q0;      // quaternion coords
    double q1;
    double q2;
    double q3;
    double thetaL;  // left arm angle
    double thetaR;  // right arm angle
    double xG;      // coordinates of body's center of mass
    double yG;
    double zG;

    enum ShoulderDynamics{
        Free,        // Free to respond to the dynamics of the doll
        Prescribed,  // Prescribed by user input
    }
    ShoulderDynamics shoulderDyn;

    double shKp = 100.0;   // proportional gain for shoulder PD controller
    double shKd = 20.0;    // derivative gain for shoulder PD controller

    int ndbg;
    double[] dbgVal;

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
        k = 0.2;    // shoulder spring stiffness
        c = 0.05;   // shoulder damping


        phi = 0.0;
        cosPhi = Math.Cos(phi);
        sinPhi = Math.Sin(phi);

        shoulderDyn = ShoulderDynamics.Free;

        ndbg = 16;
        dbgVal = new double[ndbg];

        SetRHSFunc(RHSFuncPlayter);

        StudentInit();
        RunTest();
        Reinitialize();
        //StudentInit();
    }

    //------------------------------------------------------------------------
    // Reinitialize: reset initial condition when simulation get restarted.
    //      Initial condition gets set to zero spin by default.
    //------------------------------------------------------------------------
    private void Reinitialize()
    {
        SetSpinIC(1.0);
        // // Generalized Speeds
        // x[0] = 0.0;      // omegaX
        // x[1] = 0.0;      // omegaY
        // x[2] = 0.0;      // omegaZ
        // x[3] = 0.0;      // omegaFL
        // x[4] = 0.0;      // omegaFR
        // x[5] = 0.0;      // vx
        // x[6] = 0.0;      // vy
        // x[7] = 0.0;      // vz

        // // Generalized Coordinates
        // x[8]  = 1.0;      // q0
        // x[9]  = 0.0;      // q1
        // x[10] = 0.0;      // q2
        // x[11] = 0.0;      // q3
        // x[12] = 0.0;      // thetaL
        // x[13] = 0.0;      // thetaR
        // x[14] = 0.0;      // xG
        // x[15] = 0.0;      // yG
        // x[16] = 0.0;      // zG
    }  // end Reinitialize

    //------------------------------------------------------------------------
    // SetSpinIC - Set initial conditions corresponding to a pure spin
    //      about the doll's center of mass. The center of mass of the body
    //      will have to be given a velocity so that the doll's center of mass
    //      is stationary. 
    //------------------------------------------------------------------------
    public void SetSpinIC(double omX, double omY=0.0, double omZ=0.0,
        double omFL=0.0, double omFR=0.0, double thL=0.0, double thR=0.0)
    {
        x[0] = omX;
        x[1] = omY;
        x[2] = omZ;
        x[3] = omFL;
        x[4] = omFR;

        x[8] = 1.0;
        x[9] = x[10] = x[11] = 0.0;
        x[12] = thL;
        x[13] = thR;
        x[14] = x[15] = x[16] = 0.0;

        // need to fix these ####################
        x[5] = x[6] = x[7] = 0.0;

        // reset the debug data
        // for(int i=0; i<ndbg; ++i){
        //     dbgVal[i] = 0.0;
        // }
    }

    //------------------------------------------------------------------------
    // RunTest: Run test that will allow users to test intermediate
    //          steps in writing equations of motion.
    //------------------------------------------------------------------------
    private void RunTest()
    {
        phi=1.0;
        cosPhi = Math.Cos(phi);
        sinPhi = Math.Sin(phi);

        double[] xt = new double[17];

        // Generalized Speeds
        xt[0] = 0.7;      // omegaX
        xt[1] = -0.2;      // omegaY
        xt[2] = 0.3;      // omegaZ
        xt[3] = 0.5;      // omegaFL
        xt[4] = -0.22;      // omegaFR
        xt[5] = 0.4;      // vx
        xt[6] = 0.3;      // vy
        xt[7] = -0.15;      // vz

        // Generalized Coordinates
        xt[8]  = 0.6;      // q0
        xt[9]  = -0.42;      // q1
        xt[10] = 0.24;      // q2
        xt[11] = Math.Sqrt(1.0-xt[8]*xt[8]-xt[9]*xt[9]-xt[10]*xt[10]);      // q3
        xt[12] = 0.7;      // thetaL
        xt[13] = 0.1;      // thetaR
        xt[14] = 0.1;      // xG
        xt[15] = 0.2;      // yG
        xt[16] = 0.3;      // zG

        double[] dumf = new double[17];

        RHSFuncPlayter(xt, 2.6, dumf);

        phi=1.0;
        cosPhi = Math.Cos(phi);
        sinPhi = Math.Sin(phi);
    }


    //------------------------------------------------------------------------
    // Getters/Setters
    //------------------------------------------------------------------------

    // OmegaX ------------------------------
    public double OmegaX
    {
        get{ return x[0];}

        //set{ x[0] = value; }
    }

    // OmegaY ------------------------------
    public double OmegaY
    {
        get{ return x[1];}

        //set{ x[1] = value; }
    }

    // OmegaZ ------------------------------
    public double OmegaZ
    {
        get{ return x[2];}

        //set{ x[2] = value; }
    }

    // OmegaFL ------------------------------
    public double OmegaFL
    {
        get{ return x[3];}

        //set{ x[3] = value; }
    }

    // OmegaFR ------------------------------
    public double OmegaFR
    {
        get{ return x[4];}

        //set{ x[0] = value; }
    }

    // Vx -----------------------------------
    public double Vx
    {
        get{ return x[5];}
    }

    // Vy -----------------------------------
    public double Vy
    {
        get{ return x[6];}
    }

    // Vz -----------------------------------
    public double Vz
    {
        get{ return x[7];}
    }

    // Q0 -----------------------------------
    public double Q0
    {
        get{ return x[8];}
    }

    // Q1 -----------------------------------
    public double Q1
    {
        get{ return x[9];}
    }

    // Q2 -----------------------------------
    public double Q2
    {
        get{ return x[10];}
    }

    // Q3 -----------------------------------
    public double Q3
    {
        get{ return x[11];}
    }

    // ThetaL -----------------------------------
    public double ThetaL
    {
        get{ return x[12];}

        set{ x[12] = value; }
    }

    // ThetaR -----------------------------------
    public double ThetaR
    {
        get{ return x[13];}

        set{ x[13] = value; }
    }

    // XG -----------------------------------
    public double XG
    {
        get{ return x[14];}

        set{ x[14] = value; }
    }

    // YG -----------------------------------
    public double YG
    {
        get{ return x[15];}
    }

    // ZG -----------------------------------
    public double ZG
    {
        get{ return x[16];}
    }

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

    // SetArmFree --------------------
    public void SetArmFree()
    {
        shoulderDyn = ShoulderDynamics.Free;
    }

    // SetArmPrescribed --------------
    public void SetArmPrescribed()
    {
        shoulderDyn = ShoulderDynamics.Prescribed;
    }

    // SetDebugVal
    public void SetDebugVal(int idx, double val)
    {
        if(subStep != 0)
            return;
        
        if(idx < 0 || idx >= ndbg)
            return;

        dbgVal[idx] = val;
    }

    // GetDebugVal
    public double GetDebugVal(int idx)
    {
        if(idx < 0 || idx >= ndbg)
            return(0.0);

        return dbgVal[idx];
    }

        public bool GetArmPrescribed()
    {
        return (shoulderDyn == ShoulderDynamics.Prescribed);
    }


}// end class