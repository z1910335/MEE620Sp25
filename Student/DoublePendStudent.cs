//============================================================================
using System;

public partial class DoublePendSim : Simulator
{
    // physical parameters
    double L1;   // Length of rod 1
    double L2;   // Length of rod 2
    double m1;   // mass of pendulum 1
    double m2;   // mass of pendulum 2

    //------------------------------------------------------------------------
    // RHSFuncDoublePend:  Evaluates the right sides of the differential
    //                     equations for the double pendulum
    //------------------------------------------------------------------------
    private void RHSFuncDoublePend(double[] xx, double t, double[] ff)
    {
        double theta1 = xx[0];
        double theta2 = xx[1];
        double u1 = xx[2];
        double u2 = xx[3];

        // Evaluate right sides of differential equations of motion
        //********************************************************************
        // Students enter equations of motion below
        //********************************************************************
        ff[0] = 0.0;   // time derivative of state theta1
        ff[1] = 0.0;   // time derivative of state theta2
        ff[2] = 0.0;   // time derivative of state u1
        ff[3] = 0.0;   // time derivative of state u2
    }

    //******************************************************************
    // Students enter energy calculations here. 
    //******************************************************************
    // Kinetic energy ----------
    public double KineticEnergy
    {
        get{
            double theta1 = x[0];
            double theta2 = x[1];
            double u1 = x[2];
            double u2 = x[3];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }

    // Potential energy
    public double PotentialEnergy
    {
         get{
            double theta1 = x[0];
            double theta2 = x[1];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }
}