//============================================================================
// EulerKinemSim.cs   Given an angular velocity, integrate differential
//        equations for the Euler angles which achieves the provided angular
//        velocity
//============================================================================
using System;

public partial class EulerKinemSim : Simulator
{
    // physical parameters
    // double omegaXB;         // B.x component of angular velocity
    // double omegaYB;         // B.y component of angular velocity
    // double omegaZB;         // B.z component of angular velocity


    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public EulerKinemSim() : base(3)
    {
        omegaXB = 1.0;   // default value
        omegaYB = 0.0;
        omegaZB = 0.0;

        // Default initial conditions
        x[0] = 0.0;    // theta1
        x[1] = 0.0;    // theta2
        x[2] = 0.0;    // theta3

        SetRHSFunc(RHSFuncYPR);
    }


    //------------------------------------------------------------------------
    // SetEulerType
    //------------------------------------------------------------------------
    public void SetEulerType(string mstr)
    {
        switch(mstr){
            case "YPR":
                SetRHSFunc(RHSFuncYPR);
                break;
            case "RYP":
                SetRHSFunc(RHSFuncRYP);
                break;
            default:
                SetRHSFunc(RHSFuncYPR);
                break;
        }
    }

    //------------------------------------------------------------------------
    // Getters/Setters
    //------------------------------------------------------------------------

    // OmegaX ------------------------
    public double RollRate
    {
        set{
            omegaXB = value;
        }
    }

    // OmegaY ------------------------
    public double YawRate
    {
        set{
            omegaYB = value;
        }
    }

    // OmegaZ ------------------------
    public double PitchRate
    {
        set{
            omegaZB = value;
        }
    }

    // Theta1 ------------------------
    public double Theta1
    {
        set{
            x[0] = value;
        }

        get{
            return(x[0]);
        }
    }

    // Theta2 ------------------------
    public double Theta2
    {
        set{
            x[1] = value;
        }

        get{
            return(x[1]);
        }
    }

    // Theta2 ------------------------
    public double Theta3
    {
        set{
            x[2] = value;
        }

        get{
            return(x[2]);
        }
    }
}