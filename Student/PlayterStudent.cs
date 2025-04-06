//============================================================================
// PlayterSim.cs
// Simulation of a Playter Doll.
//
// This is where students write their code.
//============================================================================
using System;

public partial class PlayterSim : Simulator
{
    

    //------------------------------------------------------------------------
    // RHSFuncPlayter:  Evaluates the right sides of the differential
    //                   equations for the Playter Doll
    //------------------------------------------------------------------------
    private void RHSFuncPlayter(double[] xx, double t, double[] ff)
    {
        omegaX  = xx[0];
        omegaY  = xx[1];
        omegaZ  = xx[2];
        omegaFL = xx[3];
        omegaFR = xx[4];
        vx      = xx[5];
        vy      = xx[6];
        vz      = xx[7];

        q0      = xx[8];
        q1      = xx[9];
        q2      = xx[10];
        q3      = xx[11];
        thetaL  = xx[12];
        thetaR  = xx[13];
        xG      = xx[14];
        yG      = xx[15];
        zG      = xx[16];



        // COMMENT THESE OUT OR REMOVE WHEN READY
        ff[0] = ff[1] = ff[2] = 0.0;   // derivs of body angular velocities set to zero
        ff[3] = ff[4] = 0.0;           // derivs of arm angular velocities
        ff[5] = ff[6] = ff[7] = 0.0;   // derivs of cener of mass velocities
        ff[8] = ff[9] = ff[10] = ff[11] = 0.0;  // derivs of quaternion coords
        ff[12] = ff[13] = 0.0;         // derivs of arm angles
        ff[14] = ff[15] = ff[16] = 0.0;  // derivs of CG coordinates
    }

} // end class