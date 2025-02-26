//============================================================================
// EulerKinemSim.cs   Given an angular velocity, integrate differential
//        equations for the Euler angles which achieves the provided angular
//        velocity.  STUDENT INTERFACE.
//============================================================================
using System;

public partial class EulerKinemSim : Simulator
{
    // physical parameters
    double omegaXB;         // B.x component of angular velocity
    double omegaYB;         // B.y component of angular velocity
    double omegaZB;         // B.z component of angular velocity

    //------------------------------------------------------------------------
    // RHSFuncYPR: Define kinematic differential equations for Yaw-Pitch-Roll
    //             Euler angles
    //------------------------------------------------------------------------
    private void RHSFuncYPR(double[] xx, double t, double[] ff)
    {
        double c1 = Math.Cos(xx[0]);       // cosine of theta1
        double s1 = Math.Sin(xx[0]);       // sine of theta1
        double tan1 = Math.Tan(xx[0]);     // tangent of theta1
        double c2 = Math.Cos(xx[1]);       // cosine of theta2
        double s2 = Math.Sin(xx[1]);       // sine of theta2
        double tan2 = Math.Tan(xx[1]);     // tangent of theta2
        double c3 = Math.Cos(xx[2]);       // cosine of theta3
        double s3 = Math.Sin(xx[2]);       // sine of theta3
        double tan3 = Math.Tan(xx[2]);     // tangent of theta4

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.5;   // time derivative of theta1
        ff[1] = 0.7;   // time derivative of theta2
        ff[2] = 0.9;   // time derivative of theta3
    }

    //------------------------------------------------------------------------
    // RHSFuncRYP: Define kinematic differential equations for Roll-Yaw-Pitch
    //             Euler angles
    //------------------------------------------------------------------------
    private void RHSFuncRYP(double[] xx, double t, double[] ff)
    {
        double c1 = Math.Cos(xx[0]);       // cosine of theta1
        double s1 = Math.Sin(xx[0]);       // sine of theta1
        double tan1 = Math.Tan(xx[0]);     // tangent of theta1
        double c2 = Math.Cos(xx[1]);       // cosine of theta2
        double s2 = Math.Sin(xx[1]);       // sine of theta2
        double tan2 = Math.Tan(xx[1]);     // tangent of theta2
        double c3 = Math.Cos(xx[2]);       // cosine of theta3
        double s3 = Math.Sin(xx[2]);       // sine of theta3
        double tan3 = Math.Tan(xx[2]);     // tangent of theta4

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.5;   // time derivative of theta1
        ff[1] = 0.7;   // time derivative of theta2
        ff[2] = 0.9;   // time derivative of theta3
    }

}