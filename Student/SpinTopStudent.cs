//============================================================================
// SpinTopStudent: Student Interface for deriving equations of motion for 
//                 a spinning top.
//============================================================================
using System;

public partial class SpinTopSim : Simulator
{
    // Physical parameter that your code has access to.
    // double h;    // distance of CG from contact rotation point
    // double m;    // mass of top
    // double IGa;  // moment of inertia about its spin axis
    // double IGp;  // moment of inertia about its CG, perpendicular to spin axis
    // double ICp;  // moment of inertia about contact point, perp to spin axis

    //------------------------------------------------------------------------
    // AuxFuncBody: This function is used to calculate angular velocity, 
    //     and vertical component of angular momentum of the simulation
    //     formulated in the BODY FRAME
    //------------------------------------------------------------------------
    private void AuxFuncBody()
    {
        double psi = x[0];
        double phi = x[1];
        double theta = x[2];
        double omegaX = x[3];
        double omegaY = x[4];
        double omegaZ = x[5];

        double cosPhi = Math.Cos(phi);
        double sinPhi = Math.Sin(phi);
        double tanPhi = Math.Tan(phi);
        double cosTheta = Math.Cos(theta);
        double sinTheta = Math.Sin(theta);

        ke = 0.0;
        pe = 0.0;
        angMoY = 0.0;
    }

    //------------------------------------------------------------------------
    // RHSFuncSpinTopLean:  Evaluates the right sides of the differential
    //                 equations for the spinning derived in the LEAN FRAME.
    //------------------------------------------------------------------------
    private void RHSFuncSpinTopLean(double[] xx, double t, double[] ff)
    {
        double psi = xx[0];
        double phi = xx[1];
        double theta = xx[2];
        double psiDot = xx[3];
        double phiDot = xx[4];
        double thetaDot = xx[5];

        double cosPhi = Math.Cos(phi);
        double sinPhi = Math.Sin(phi);
        double tanPhi = Math.Tan(phi);

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;   // time deriv of state psi
        ff[1] = 0.0;   // time deriv of state phi
        ff[2] = 0.0;   // time deriv of state theta
        ff[3] = 0.0;   // time deriv of state psiDot
        ff[4] = 0.0;   // time deriv of state phiDot
        ff[5] = 0.0;   // time deriv of state thetaDot

    }

    //------------------------------------------------------------------------
    // AuxFuncLean: This function is used to calculate angular velocity, 
    //     vertical component of angular momentum of the system formulated
    //     in the LEAN FRAME.
    //------------------------------------------------------------------------
    private void AuxFuncLean()
    {
        //********* Students write your expressions for kinetic energy,
        //          potential energy, vertical component of angular momentum,

        double psi = x[0];
        double phi = x[1];
        double theta = x[2];
        double psiDot = x[3];
        double phiDot = x[4];
        double thetaDot = x[5];

        double cosPsi = Math.Cos(psi);
        double sinPsi = Math.Sin(psi);
        double cosPhi = Math.Cos(phi);
        double sinPhi = Math.Sin(phi);
        double tanPhi = Math.Tan(phi);


        ke = 0.0;
        pe = 0.0;
        angMoY = 0.0;

        // ****** Ignore stuff below for now ********
        // Components of support force in P (precession) frame
        cPx = 0.0;
        cPy = 0.0;
        cPz = 0.0;
    }

    //------------------------------------------------------------------------
    // CalcSimplePrecession: Given the initial lean angle phi, and rotation
    //       rate thetaDot, calculate the initial precession rate psiDot so
    //       that the spinning top exhibits simple precession without 
    //       nutation. For some inputs, there is no simple precession 
    //       solution. In that case return the value "closest" to simple 
    //       precession.
    //------------------------------------------------------------------------
    private double CalcSimplePrecession(double phi, double thetaDot)
    {
        double cosPhi = Math.Cos(phi);
        double sinPhi = Math.Sin(phi);

        // ****** Students write your code here *******

        precessICFound = false;  // change to true if found
        return 0.0;
    }
}