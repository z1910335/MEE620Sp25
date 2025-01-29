//============================================================================
// PendCartStudent: This is where students should work to get their
//      pendulum/cart simulation working.
//============================================================================
using System;

public partial class PendCartSim : Simulator
{
    // physical parameters
    double L;    // Length of rod
    double mc;   // mass of cart
    double mp;   // mass of pendulum

    //------------------------------------------------------------------------
    // RHSFuncPendCart:  Evaluates the right sides of the differential
    //                   equations for the pendulum/cart
    //------------------------------------------------------------------------
    private void RHSFuncPendCart(double[] xx, double t, double[] ff)
    {
        double xCart = xx[0];
        double theta = xx[1];
        double u     = xx[2];
        double omega = xx[3];

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //

        ff[0] = 0.0;   // time derivative of state xCart
        ff[1] = 0.0;   // time derivative of state theta
        ff[2] = 0.0;   // time derivative of state u
        ff[3] = 0.0;   // time derivative of state omega
    }

    //------------------------------------------------------------------------
    // Getters for specific simulation output
    //------------------------------------------------------------------------

    // Kinetic energy ----------------------------
    public double KineticEnergy
    {
        get{
            double xCart = x[0];
            double theta = x[1];
            double u     = x[2];
            double omega = x[3];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }

    // Potential energy ------------------------------
    public double PotentialEnergy
    {
         get{
            double xCart = x[0];
            double theta = x[1];
            double u     = x[2];
            double omega = x[3];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }

    // Center of Mass, horizontal coordinate ---------------
    public double MassCenterX
    {
        get{
            double xCart = x[0];
            double theta = x[1];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }

    // Center of Mass, vertical coordinate ------------------
    public double MassCenterY
    {
        get{
            double xCart = x[0];
            double theta = x[1];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }

}