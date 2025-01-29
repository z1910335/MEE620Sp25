//============================================================================
// PendCartSim.cs   Class for creating a simulation of a the cart/pendulum.
//============================================================================
using System;

public class PendCartSim : Simulator
{
    // physical parameters
    double L;    // Length of rod
    double mc;   // mass of cart
    double mp;   // mass of pendulum


    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public PendCartSim() : base(4)
    {
        L = 1.5;
        mc = 1.9;
        mp = 2.8;

        // Default initial conditions
        x[0] = 0.0;    // generalized coord: cart position
        x[1] = 0.0;    // generalized coord: pendulum
        x[2] = 0.0;    // generalized speed: cart
        x[3] = 0.0;    // generalized speed: pendulum

        SetRHSFunc(RHSFuncPendCart);
    }

    //------------------------------------------------------------------------
    // RHSFuncPendCart:  Evaluates the right sides of the differential
    //                   equations for the pendulum/cart
    //------------------------------------------------------------------------
    private void RHSFuncPendCart(double[] xx, double t, double[] ff)
    {
        double xCart = xx[0];
        double theta = xx[1];
        double u1 = xx[2];
        double u2 = xx[3];

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;   // time derivative of state xCart
        ff[1] = 0.0;   // time derivative of state theta
        ff[2] = 0.0;   // time derivative of state u1
        ff[3] = 0.0;   // time derivative of state u2
    }


    //------------------------------------------------------------------------
    // Getters/Setters
    //------------------------------------------------------------------------

    // Pendulum length -----------------------------
    public double PendulumLength
    {
        set{
            if(value >= 0.1){
                L = value;
            }
        }

        get{
            return L;
        }
    }

    // Pendulum mass -------------------------------
    public double PendulumMass
    {
        set{
            if(value >= 0.01){
                mp = value;
            }
        }

        get{
            return mp;
        }
    }

    // Cart mass -----------------------------------
    public double CartMass
    {
        set{
            if(value >= 0.01){
                mc = value;
            }
        }

        get{
            return mc;
        }
    }

    // Cart position ------------------------------
    public double Position
    {
        set{
            if(value > 4.0)
                value = 4.0;
            if(value < -4.0)
                value = -4.0;

            x[0] = value;
        }

        get{
            return x[0];
        }
    }

    // Pendulum angle ----------------------------
    public double Angle
    {
        set{
            x[1] = value;
        }

        get{
            return x[1];
        }
    }

    // Generalized Speed Cart ---------------------
    public double GenSpeedCart
    {
        set{
            x[2] = value;
        }

        get{
            return x[2];
        }
    }

    // Generalized Speed Pendulum ---------=------
    public double GenSpeedPend
    {
        set{
            x[3] = value;
        }

        get{
            return x[3];
        }
    }

    // Kinetic energy ----------------------------
    public double KineticEnergy
    {
        get{
            double theta = x[1];
            double u1 = x[2];
            double u2 = x[3];

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