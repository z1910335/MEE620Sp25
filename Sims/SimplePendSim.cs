//============================================================================
// SimplePendSim.cs   Class for creating a simulation of a simple pendulum.
//============================================================================
using System;

public class SimplePendSim : Simulator
{
    // physical parameters
    double L;   // Pendulum length

    //------------------------------------------------------------------------
    // Constructor.    [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public SimplePendSim() : base(2)
    {
        L = 1.0;

        // Default initial conditions
        x[0] = 0.0;    // generalized coord: pendulum angle (radians)
        x[1] = 0.0;    // generalized speed

        SetRHSFunc(RHSFuncSimplePend);
    }

    //------------------------------------------------------------------------
    // RHSFuncSimplePend:  Evaluates the right sides of the differential
    //                     equations for the simple pendulum
    //------------------------------------------------------------------------
    private void RHSFuncSimplePend(double[] xx, double t, double[] ff)
    {
        double theta = xx[0];    // generalized coordinate, pend angle
        double omega = xx[1];        // generalized speed

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;   // time derivative of state theta
        ff[1] = 0.0;   // time derivative of state u
    }

    //------------------------------------------------------------------------
    // Getters and Setters
    //------------------------------------------------------------------------

    // Pendulum length ---------------------------
    public double Length
    {
        set{
            if(value > 0.05){
                L = value;
            }
        }

        get{
            return L;
        }
    }

    // Pendulum angle ----------------------------
    public double Angle
    {
        set{
            x[0] = value;
        }

        get{
            return x[0];
        }
    }

    // Generalized Speed ---------
    public double GenSpeed
    {
        set{
            x[1] = value;
        }

        get{
            return x[1];
        }
    }

    //******************************************************************
    // Students enter energy calculations here.
    //******************************************************************
    // Kinetic energy ----------
    public double KineticEnergy
    {
        get{
            double theta = x[0];
            double omega = x[1];

            return 0.0;
        }
    }

    // Potential energy
    public double PotentialEnergy
    {
         get{
            double theta = x[0];
            double omega = x[1];

            return 0.0; 
        }
    }
}