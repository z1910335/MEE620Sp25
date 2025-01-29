//============================================================================
// DoublePendSim.cs   Class for creating a simulation of a double pendulum.
//============================================================================
using System;

public partial class DoublePendSim : Simulator
{
    // physical parameters
    // double L1;   // Length of rod 1
    // double L2;   // Length of rod 2
    // double m1;   // mass of pendulum 1
    // double m2;   // mass of pendulum 2


    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public DoublePendSim() : base(4)
    {
        L1 = 0.9;
        L2 = 0.7;
        m1 = 1.4;
        m2 = 1.1;

        // Default initial conditions
        x[0] = 0.0;    // shoulder angle
        x[1] = 0.0;    // elbow angle
        x[2] = 0.0;    // generalized speed 1
        x[3] = 0.0;    // generalized speed 2

        SetRHSFunc(RHSFuncDoublePend);
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

}