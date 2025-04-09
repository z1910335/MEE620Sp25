//============================================================================
// Vex.cs     A simple struct for defining a 3D vector and vector
//                  operations    (Generated from ChatGPT)
//============================================================================

using System;

struct Vex
{
    public double x, y, z;

    // Constructor
    public Vex(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vex()
    {
        this.x = 0.0;
        this.y = 0.0;
        this.z = 0.0;
    }

    // Addition
    public static Vex operator +(Vex a, Vex b)
    {
        return new Vex(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    // Subtraction
    public static Vex operator -(Vex a, Vex b)
    {
        return new Vex(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    // Scalar multiplication
    public static Vex operator *(double scalar, Vex vector)
    {
        return new Vex(scalar * vector.x, scalar * vector.y, scalar * vector.z);
    }

    // Scalar multiplication (reverse order)
    public static Vex operator *(Vex vector, double scalar)
    {
        return scalar * vector;
    }

    // Dot product
    public static double Dot(Vex a, Vex b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    // Cross product
    public static Vex Cross(Vex a, Vex b)
    {
        return new Vex(a.y * b.z - a.z * b.y,
                            a.z * b.x - a.x * b.z,
                            a.x * b.y - a.y * b.x);
    }

    // Magnitude (length) of the vector
    public double Magnitude()
    {
        return Math.Sqrt(x * x + y * y + z * z);
    }

    // Normalization of the vector
    // public Vector3D Normalize()
    // {
    //     float magnitude = Magnitude();
    //     if (magnitude == 0)
    //         return this;
    //     else
    //         return new Vector3D(x / magnitude, y / magnitude, z / magnitude);
    // }

    // Override ToString method for readable output
    public override string ToString()
    {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}