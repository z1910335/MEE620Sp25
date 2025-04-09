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

    double fac;
    double sum;
    int i;
    int j;
    int k;

    //------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------
    public LinSysEq(int nn = 3)
    {

        Resize(nn);
    }

    //------------------------------------------------------------------------
    // resize: resize the matrices to hold the right number of equations
    //         and unknowns.
    //------------------------------------------------------------------------
    public void Resize(int nn)
    {
        n = nact = nn;

        b = new double[n];
        A = new double[n,n];
        M = new double[n,n+1];
        x = new double[n];
        cidx = new int[n];
        ridx = new int[n];

        UseAllEquations();
    }

    //------------------------------------------------------------------------
    // UseAllEquations:   Use all rows and columns in the solution
    //------------------------------------------------------------------------
    public void UseAllEquations()
    {
        for(i=0; i<n; ++i){
            cidx[i] = i;
            ridx[i] = i;
        }
    }

    //------------------------------------------------------------------------
    // IgnoreRowColumn: Ignore specified row and column indices in solution
    //------------------------------------------------------------------------
    public void IgnoreRowColumn(int rowIdx, int colIdx)
    {
        //############# Need to do this
    }

    //------------------------------------------------------------------------
    // RestoreRowColumn: Restore specified row and column indices for 
    //                   consideration
    //------------------------------------------------------------------------
    public void RestoreRowColumn(int rowIdx, int colIdx)
    {
        //################### NEED TO DO THIS
    }

    //------------------------------------------------------------------------
    // SolveGauss: Solve by Gauss elimination
    //------------------------------------------------------------------------
    public void SolveGauss()
    {
        for(i=0; i<nact; ++i){
            for(j=0; j<nact; ++j){
                M[ridx[i],cidx[j]] = A[ridx[i],cidx[j]];
            }
            M[ridx[i],n] = b[ridx[i]];
        }

        // perform Gauss elimination
        for(i=0; i<(nact-1); ++i){
            PivotRow(i);
            for(j=i+1; j<nact; ++j){
                fac = M[ridx[j],cidx[i]] / M[ridx[i],cidx[i]];
                for(k=i; k<nact; ++k){
                    M[ridx[j],cidx[k]] -= fac*M[ridx[i],cidx[k]];
                }
                M[ridx[j],n] -= fac*M[ridx[i],n];
            }
        }

        // perform back substitution
        for(i=nact-1; i>=0; --i){
            sum = M[ridx[i],n];
            for(j=nact-1; j>i; --j){
                sum -= M[ridx[i],cidx[j]] * x[cidx[j]];
            }
            x[cidx[i]] = sum/M[ridx[i],cidx[i]];
        }
    }

    //------------------------------------------------------------------------
    // PivotRow: Execute partial pivoting
    //------------------------------------------------------------------------
    private void PivotRow(int jj)
    {
        double maxElem = Math.Abs(M[ridx[jj], cidx[jj]]);
        double dum;
        int rowIdx = jj;  // row with biggest pivot element (intial guess)
        int ii, holder;

        for(ii=jj+1; ii<nact; ++ii){    // find largest element in jth column
            dum = Math.Abs(M[ridx[ii], cidx[jj]]);
            if(dum > maxElem){
                maxElem = dum;
                rowIdx = ii;
            }
        }

        if(rowIdx != jj){    // swap rows if there's one with a larger pivot 
            holder = ridx[jj];
            ridx[jj] = ridx[rowIdx];
            ridx[rowIdx] = holder;
        }
    }

    //------------------------------------------------------------------------
    // Check
    //------------------------------------------------------------------------
    public double Check()
    {
        sum = 0.0;
        double sum2 = 0.0;

        for(i=0;i<nact;++i){
            sum = 0.0;
            for(j=0; j<nact;++j){
                sum += A[ridx[i],cidx[j]] * x[cidx[j]];
            }
            double delta = sum - b[ridx[i]];
            sum2 += delta*delta;
        }

        return(Math.Sqrt(sum2/(1.0*nact)));
    }

    //------------------------------------------------------------------------
    // get/set
    //------------------------------------------------------------------------

    // SetA -------------------
    public void SetA(int ii, int jj, double val)
    {
        if(ii < 0 || ii >= n)
            return;

        if(jj < 0 || ii >= n)
            return;

        A[ii,jj] = val;
    }

    // SetB -------------------
    public void SetB(int ii, double val)
    {
        if(ii < 0 || ii >= n)
            return;

        b[ii] = val;
    }

    // Sol
    public double Sol(int ii)
    {
        if(ii < 0 || ii >= n)
            return 0.0;

        return x[ii];
    }
}