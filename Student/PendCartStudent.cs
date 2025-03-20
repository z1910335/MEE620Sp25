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


        double cosTheta = Math.Cos(theta);
        double sinTheta = Math.Sin(theta);

        double A = mp*L*L;
        double B = mp*L*cosTheta;
        double C = mp*L*cosTheta;
        double D = mp+mc;
        double det = A*D-B*C;

        double R1 = mp*L*g*sinTheta;
        double R2 = -mp*L*omega*omega*sinTheta;

        ff[0] = u;   // time derivative of state xCart
        ff[1] = omega;   // time derivative of state theta
        ff[2] = (D*R1-B*R2)/det;   // time derivative of state u
        ff[3] =  (-C*R1+A*R2)/det;  // time derivative of state omega | (-C*R1+A*R2)/det   -(g/L)*Math.Sin(theta)

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

            double KE_cart = 0.5 * mc * u * u;
            double KE_pend = 0.5 * mp * (u * u + L * L * omega * omega) + mp * u * L * omega * Math.Cos(theta);
        
            return KE_cart + KE_pend;
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

            //########## YOU NEED TO CALCULATE THIS ##########
        
            return -mp*g*L*Math.Cos(theta);
        }
    }

    // Center of Mass, horizontal coordinate ---------------
    public double MassCenterX
    {
        get{
            double xCart = x[0];
            double theta = x[1];

            //########## YOU NEED TO CALCULATE THIS ###########

            return (mc * xCart + mp * (xCart + L * Math.Sin(theta))) / (mc + mp);
        }
    }

    // Center of Mass, vertical coordinate ------------------
    public double MassCenterY
    {
        get{
            double xCart = x[0];
            double theta = x[1];

            //########## YOU NEED TO CALCULATE THIS ###########
        
            return (mp * (-L*Math.Cos(theta))) / (mc + mp);
        }
    }

}