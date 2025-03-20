//============================================================================
using System;

public partial class DoublePendSim : Simulator
{
    // physical parameters
    // private double L1;   // Length of rod 1
    // private double L2;   // Length of rod 2
    // private double m1;   // mass of pendulum 1
    // private double m2;   // mass of pendulum 2


    // public DoublePendStudent() : base(4)
    // {
    //     L1 = 0.9;
    //     L2 = 0.7;
    //     m1 = 1.4;
    //     m2 = 1.1;

    //     // Default initial conditions
    //     x[0] = 0.0;    // shoulder angle
    //     x[1] = 0.0;    // elbow angle
    //     x[2] = 0.0;    // generalized speed 1
    //     x[3] = 0.0;    // generalized speed 2

    //     SetRHSFunc(RHSFuncDoublePend);
    // }
/*
    //------------------------------------------------------------------------
    // RHSFuncDoublePend:  Evaluates the right sides of the differential
    //                     equations for the double pendulum
    //------------------------------------------------------------------------
    private void RHSFuncDoublePend(double[] xx, double t, double[] ff)
    {
        double theta1 = xx[0];
        double theta2 = xx[1];
        double omega1 = xx[2];
        double omega2 = xx[3];

        // Evaluate right sides of differential equations of motion
        //********************************************************************
        // Students enter equations of motion below
        //********************************************************************
        double cosTheta = Math.Cos(theta2);
        double sinTheta = Math.Sin(theta2);

        double A = (m1+m2)*L1*L1*theta1;
        double B = m2*L1*L2*cosTheta;
        double C = B;
        double D = m2*L2*L2;
        double det = A*D-B*C;

        double R1 = m2*L1*L2*omega2*omega2*sinTheta - (m1+m2)*g*L1*Math.Sin(theta1);
        double R2 = -m2*L1*L2*omega1*omega1*sinTheta - m2*g*L2*Math.Sin(theta1+theta2);

        ff[0] = omega1;   // time derivative of state theta1
        ff[1] = omega2-omega1;   // time derivative of state theta2
        ff[2] = (D*R1-B*R2)/det;   // time derivative of state omega1
        ff[3] = (-C*R1*A*R2)/det;   // time derivative of state omega2
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
            double omega1 = x[2];
            double omega2 = x[3];

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

  //------------------------------------------------------------------------
    // Getters and Setters      [STUDENTS: DO NOT CHANGE THESE FUNCTIONS]
    //------------------------------------------------------------------------

    // Pendulum length ---------------------------
    public double Length1
    {
        set{
            if(value > 0.05){
                L1 = value;
            }
        }

        get{
            return L1;
        }
    }

    // Pendulum length ---------------------------
    public double Length2
    {
        set{
            if(value > 0.05){
                L2 = value;
            }
        }

        get{
            return L2;
        }
    }

    // Pendulum mass ---------------------------
    public double Mass1
    {
        set{
            if(value > 0.05){
                m1 = value;
            }
        }

        get{
            return m1;
        }
    }

    // Pendulum mass ---------------------------
    public double Mass2
    {
        set{
            if(value > 0.05){
                m2 = value;
            }
        }

        get{
            return m2;
        }
    }

    // Pendulum angle ----------------------------
    public double Angle1
    {
        set{
            x[0] = value;
        }

        get{
            return x[0];
        }
    }

    // Pendulum angle ----------------------------
    public double Angle2
    {
        set{
            x[1] = value;
        }

        get{
            return x[1];
        }
    }

    // Generalized Speed ---------
    public double GenSpeed1
    {
        set{
            x[2] = value;
        }

        get{
            return x[2];
        }
    }

    // Generalized Speed ---------
    public double GenSpeed2
    {
        set{
            x[3] = value;
        }

        get{
            return x[3];
        }
    }
*/
}
