//============================================================================
// LinSysEq.cs
// Solves a system of linear algebra equations with back substitution
//============================================================================
using System;

public class LinSysEq
{
    private int n = 3;      // maximum number of equations and unknowns
    private int nact = 3;   // number of active equations
    private double[,] A;    // coefficient matrix
    private double[] b;     // right hand side
    private double[,] M;    // augmented matrix
    private double[] x;     // solution
    private int[] cidx;     // array of column indices
    private int[] ridx;     // array of row indices

    int i;
    int j;

    //------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------
    public LinSysEq(int nn = 3)
    {

        Resize(nn);
    }

    //--------------------------------------------------------------------
    // resize: resize the matrices to hold the right number of equations
    //         and unknowns.
    //--------------------------------------------------------------------
    public void Resize(int nn)
    {
        n = nact = nn;

        b = new double[n];
        A = new double[n,n];
        M = new double[n,n+1];
        x = new double[n];
        cidx = new int[n];
        ridx = new int[n];

        for(i=0; i<n; ++i){
            cidx[i] = i;
            ridx[i] = i;
        }
    }
}