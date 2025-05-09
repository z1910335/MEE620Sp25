//============================================================================
// PlayterStudent.cs
// Simulation of a Playter Doll.
//
// This is where students write their code.
//============================================================================
using System;
using System.Diagnostics; // Added to used Debug.Writeline in Godot

public partial class PlayterSim : Simulator
{
    // // Parameters
    // double mA;     // dimensionless arm mass
    // double rho;    // dimless radius if gyration for moment of inertia, I_Gx
    // double gammaY; // ratio I_Gy/I_Gx
    // double gammaZ; // ratio I_Gz/I_Gx
    // double h;      // dimless vertical (b_y) distance of shoulder from body CG
    // double L;      // dimless distance of arm mass from shoulder
    // double k;      // dimless torsional stiffness of shoulder spring
    // double c;      // dimless torsional damping coeff in shoulder damper
    // double phi;    // angle of arm swing plane relative to vertical
    // double cosPhi;
    // double sinPhi;

    // // generalized speeds
    // double omegaX;
    // double omegaY;
    // double omegaZ;
    // double omegaFL;  // time derivative of thetaL
    // double omegaFR;  // time derivative of thetaR
    // double vx;
    // double vy;
    // double vz;

    // // generalized coordinates
    // double q0;      // quaternion coords
    // double q1;
    // double q2;
    // double q3;
    // double thetaL;  // left arm angle
    // double thetaR;  // right arm angle
    // double xG;      // coordinates of body's center of mass
    // double yG;
    // double zG;

    // Some extra stuff a student might want (feel free to define more.)
    LinSysEq sys;    // linear algebraic equation solver
    double[,] Amat;  // some arrays that might be handy 
    double[] Bmat;



    //------------------------------------------------------------------------
    // StudentInit: Student might want to allocate arrays and the like 
    //              before simulation begins
    //------------------------------------------------------------------------
    private void StudentInit()
    {
        sys = new LinSysEq(8);
        Amat = new double[8,8];  // an 8 by 8 array of doubles
        Bmat = new double[8];    // an 8 by 1 array of doubles
        
    }

    //------------------------------------------------------------------------
    // RHSFuncPlayter:  Evaluates the right sides of the differential
    //                   equations for the Playter Doll
    //------------------------------------------------------------------------
    
    private void RHSFuncPlayter(double[] xx, double t, double[] ff)
    {
      
        omegaX  = xx[0];
        omegaY  = xx[1];
        omegaZ  = xx[2];
        omegaFL = xx[3];
        omegaFR = xx[4];
        vx      = xx[5];
        vy      = xx[6];
        vz      = xx[7];

        q0      = xx[8];
        q1      = xx[9];
        q2      = xx[10];
        q3      = xx[11];
        thetaL  = xx[12];
        thetaR  = xx[13];
        xG      = xx[14];
        yG      = xx[15];
        zG      = xx[16];

        double rho2 = rho*rho;
        // Equation 6:
        Vex angVel = new Vex(omegaX, omegaY, omegaZ);
        // Equation 14:
        Vex angMo = new Vex(rho2*omegaX, rho2*gammaY*omegaY, rho2*gammaZ*omegaZ);
        // Equation 16:
        Vex angVelCrosAngMo = Vex.Cross(angVel, angMo);

        // Equation 4:
        ff[0] = -angVelCrosAngMo.x/rho2;
        ff[1] = -angVelCrosAngMo.y/(rho2*gammaY);
        ff[2] = -angVelCrosAngMo.y/(rho2*gammaZ);
        // SetDebugVal(0, omegaX);
        // SetDebugVal(1, omegaY);
        // SetDebugVal(2, omegaZ);

        // Equation 5:
        ff[8] = 0.5*(-q1*omegaX - q2*omegaY - q3*omegaZ);
        ff[9] = 0.5*(q0*omegaX - q3*omegaY + q2*omegaZ);
        ff[10] = 0.5*(q3*omegaX + q0*omegaY - q1*omegaZ);
        ff[11] = 0.5*(-q2*omegaX + q1*omegaY + q0*omegaZ);

        // Equation 17 | Section 5.1 — Velocity of center of mass (G) in body frame:
        Vex vNx = new Vex(q0*q0 + q1*q1 - q2*q2 - q3*q3,
                        2*q1*q2 + 2*q0*q3,
                        2*q1*q3 - 2*q0*q2);  // nx in body frame

        Vex vNy = new Vex(2*q1*q2 - 2*q0*q3,
                        q0*q0 - q1*q1 + q2*q2 - q3*q3,
                        2*q2*q3 + 2*q0*q1);  // ny in body frame

        Vex vNz = new Vex(2*q1*q3 + 2*q0*q2,
                        2*q2*q3 - 2*q0*q1,
                        q0*q0 - q1*q1 - q2*q2 + q3*q3);  // nz in body frame

        // Equation 19
        // Velocity vector of CG in body frame:
        Vex vG = vx * vNx + vy * vNy + vz * vNz;

        // Equation 23:
        // Acceleration of CG in body frame (derivative of vG):
        Vex aG = ff[5] * vNx + ff[6] * vNy + ff[7] * vNz;

        // Section 6.1 — Configuration: Position of arm masses in body frame
        // Equation 24:
        // Left shoulder to CG:
        Vex rSLG = new Vex(1.0, h, 0.0);  // Left shoulder offset
        // Right shoulder to CG:
        Vex rSRG = new Vex(-1.0, h, 0.0);  // Right shoulder offset
        // Arm rotation angles:
        double cosThL = Math.Cos(thetaL);
        double sinThL = Math.Sin(thetaL);
        double cosThR = Math.Cos(thetaR);
        double sinThR = Math.Sin(thetaR);
        // Left arm: mass relative to shoulder
        Vex rFLSL = new Vex(
            L * cosThL,
            L * sinThL * cosPhi,
            L * sinThL * sinPhi
        );
        // Right arm: mass relative to shoulder
        Vex rFRSR = new Vex(
            -L * cosThR,
            L * sinThR * cosPhi,
            L * sinThR * sinPhi
        );

        // Total arm mass positions relative to body CG:
        Vex rFLG = rSLG + rFLSL;
        Vex rFRG = rSRG + rFRSR;


        // Section 3.2.2 — Angular velocity of left and right arms in body frame
        Vex sz = new Vex(0.0, -sinPhi, cosPhi);  // shoulder axis in body frame

        Vex omegaL = omegaFL * sz;  // angular velocity vector of left arm
        Vex omegaR = omegaFR * sz;  // angular velocity vector of right arm

        // Equation 25:
        Vex vSL = vG + Vex.Cross(angVel, rSLG);
        Vex vSR = vG + Vex.Cross(angVel, rSRG);
        // Equation 26:
        Vex vFL = vSL + Vex.Cross(omegaL, rFLSL);
        Vex vFR = vSR + Vex.Cross(omegaR, rFRSR);

        // Section 6.2 — Velocity of left and right arm masses in body frame
        // Equation 27:
        // Velocity of left arm:
        Vex vFL_full = vG + Vex.Cross(angVel, rFLG) + Vex.Cross(omegaL, rFLSL);
        // Velocity of Right arm:
        Vex vFR_full = vG + Vex.Cross(angVel, rSRG) + Vex.Cross(omegaR, rFRSR);

        // Equation 29 (used in 28):
        Vex omegaPx = new Vex(1.0, 0.0, 0.0);  // bx
        Vex omegaPy = new Vex(0.0, 1.0, 0.0);  // by
        Vex omegaPz = new Vex(0.0, 0.0, 1.0);  // bz

        // Equation 31 (partial angular velocity directions):
        Vex vFLpx = Vex.Cross(omegaPx, rFLG);     // dv/domegaX
        Vex vFLpy = Vex.Cross(omegaPy, rFLG);     // dv/domegaY
        Vex vFLpz = Vex.Cross(omegaPz, rFLG);     // dv/domegaZ
        Vex vFLpFL = Vex.Cross(sz, rFLSL);        // dv/domegaFL

        // Equation 29/31 for right arm:
        Vex vFRpx = Vex.Cross(omegaPx, rFRG);     // dv/domegaX
        Vex vFRpy = Vex.Cross(omegaPy, rFRG);     // dv/domegaY
        Vex vFRpz = Vex.Cross(omegaPz, rFRG);     // dv/domegaZ
        Vex vFRpFR = Vex.Cross(sz, rFRSR);        // dv/domegaFR

        // Equation 30 (full expansion):
        Vex vFL_explicit = vx * vNx + vy * vNy + vz * vNz +
                   omegaX * vFLpx + omegaY * vFLpy + omegaZ * vFLpz + omegaFL * vFLpFL;

        // Equation 30 for right arm (full expansion):
        Vex vFR_explicit = vx * vNx + vy * vNy + vz * vNz +
                        omegaX * vFRpx + omegaY * vFRpy + omegaZ * vFRpz + omegaFR * vFRpFR;

        // Compare full and explicit left arm velocity
        // SetDebugVal(11, vFL_full.y);
        // SetDebugVal(12, vFL_explicit.y);
        // SetDebugVal(13, vFL_explicit.z);
        // SetDebugVal(14, vFR_full.y);
        // SetDebugVal(15, vFR_explicit.y);

        // Section 6.3.1 — Acceleration of left and right arm masses

        Vex alphaB = new Vex(ff[0], ff[1], ff[2]); // angular acceleration of the body
        Vex alphaL = ff[3] * sz; // angular acceleration of left arm
        Vex alphaR = ff[4] * sz; // angular acceleration of right arm

        Vex a1L = Vex.Cross(alphaB, rFLG); // alphaB cross rFLG
        Vex a2L = Vex.Cross(angVel, Vex.Cross(angVel, rFLG)); // omegaB cross (omegaB cross rFLG)
        Vex a3L = Vex.Cross(alphaL, rFLSL); // alphaL cross rFLSL
        Vex a4L = Vex.Cross(omegaL, Vex.Cross(omegaL, rFLSL)); // omegaL cross (omegaL cross rFLSL)

        Vex a1R = Vex.Cross(alphaB, rFRG); // alphaB cross rFRG
        Vex a2R = Vex.Cross(angVel, Vex.Cross(angVel, rFRG)); // omegaB cross (omegaB cross rFRG)
        Vex a3R = Vex.Cross(alphaR, rFRSR); // alphaR cross rFRSR
        Vex a4R = Vex.Cross(omegaR, Vex.Cross(omegaR, rFRSR)); // omegaR cross (omegaR cross rFRSR)

        Vex aFL = aG + a1L + a2L + a3L + a4L; // total acceleration of left arm
        Vex aFR = aG + a1R + a2R + a3R + a4R; // total acceleration of right arm

        // SetDebugVal(4, aFL.y);
        // SetDebugVal(5, aFL.z);
        // SetDebugVal(6, aFR.y);
        // SetDebugVal(7, aFR.z);

        // Angular momentum equation: partial angular velocities of body
        Vex[] dhgPartials = new Vex[8];
        dhgPartials[0] = new Vex(1.0, 0.0, 0.0); // omegaX
        dhgPartials[1] = new Vex(0.0, 1.0, 0.0); // omegaY
        dhgPartials[2] = new Vex(0.0, 0.0, 1.0); // omegaZ
        dhgPartials[3] = new Vex(0.0, 0.0, 0.0); // omegaFL
        dhgPartials[4] = new Vex(0.0, 0.0, 0.0); // omegaFR
        dhgPartials[5] = new Vex(0.0, 0.0, 0.0); // vx
        dhgPartials[6] = new Vex(0.0, 0.0, 0.0); // vy
        dhgPartials[7] = new Vex(0.0, 0.0, 0.0); // vz

        // Left arm mass point partial velocities
        Vex[] aFLPartials = new Vex[8];
        aFLPartials[0] = Vex.Cross(omegaPx, rFLG); // omegaX
        aFLPartials[1] = Vex.Cross(omegaPy, rFLG); // omegaY
        aFLPartials[2] = Vex.Cross(omegaPz, rFLG); // omegaZ
        aFLPartials[3] = Vex.Cross(sz, rFLSL); // omegaFL
        aFLPartials[4] = new Vex(0.0, 0.0, 0.0); // omegaFR
        aFLPartials[5] = vNx; // vx
        aFLPartials[6] = vNy; // vy
        aFLPartials[7] = vNz; // vz

        // Right arm mass point partial velocities
        Vex[] aFRPartials = new Vex[8];
        aFRPartials[0] = Vex.Cross(omegaPx, rFRG); // omegaX
        aFRPartials[1] = Vex.Cross(omegaPy, rFRG); // omegaY
        aFRPartials[2] = Vex.Cross(omegaPz, rFRG); // omegaZ
        aFRPartials[3] = new Vex(0.0, 0.0, 0.0); // omegaFL
        aFRPartials[4] = Vex.Cross(sz, rFRSR); // omegaFR
        aFRPartials[5] = vNx; // vx
        aFRPartials[6] = vNy; // vy
        aFRPartials[7] = vNz; // vz

        // Body CG translational partial velocities
        Vex[] aGPartials = new Vex[8];
        aGPartials[0] = new Vex(0.0, 0.0, 0.0); // omegaX
        aGPartials[1] = new Vex(0.0, 0.0, 0.0); // omegaY
        aGPartials[2] = new Vex(0.0, 0.0, 0.0); // omegaZ
        aGPartials[3] = new Vex(0.0, 0.0, 0.0); // omegaFL
        aGPartials[4] = new Vex(0.0, 0.0, 0.0); // omegaFR
        aGPartials[5] = vNx; // vx
        aGPartials[6] = vNy; // vy
        aGPartials[7] = vNz; // vz

        //double[,] Amat = new double[8,8]; // filled earlier
        double[] Qmat = new double[8]; // fill with nonlinear terms
        double[] PLmat = new double[8]; // fill with torque multipliers for left arm
        double[] PRmat = new double[8]; // for right arm
        double tqL = 0; // left arm torque input
        double tqR = 0; // right arm torque input

        // Kinematic Equations for each row of A matrix
        Vex[] KE = new Vex[8];

        // Angular momentum rows (0–2)
        Vex omegaCrossH = Vex.Cross(new Vex(omegaX, omegaY, omegaZ), angMo);
        Vex torqueL = Vex.Cross(rFLG, aFL * mA);
        Vex torqueR = Vex.Cross(rFRG, aFR * mA);

        KE[0] = new Vex(ff[0], 0, 0) + new Vex(omegaCrossH.x, 0, 0) + torqueL + torqueR;
        KE[1] = new Vex(0, ff[1], 0) + new Vex(0, omegaCrossH.y, 0) + torqueL + torqueR;
        KE[2] = new Vex(0, 0, ff[2]) + new Vex(0, 0, omegaCrossH.z) + torqueL + torqueR;

        // Left and Right arm rows
        KE[3] = aFL;
        KE[4] = aFR;

        // Body CG rows
        KE[5] = ff[5] * vNx;
        KE[6] = ff[6] * vNy;
        KE[7] = ff[7] * vNz;

        // Fill Amat[i,j] from mass × dot(KE[i], partial_j)
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                Vex pj = (j == 0) ? new Vex(1.0, 0.0, 0.0) :
                         (j == 1) ? new Vex(0.0, 1.0, 0.0) :
                         (j == 2) ? new Vex(0.0, 0.0, 1.0) :
                         (j == 3) ? Vex.Cross(sz, rFLSL) :
                         (j == 4) ? Vex.Cross(sz, rFRSR) :
                         (j == 5) ? vNx :
                         (j == 6) ? vNy : vNz;

                double mass = (i < 3) ? rho * rho :
                            (i == 3 || i == 4) ? mA : 1.0;

                Amat[i,j] = mass * Vex.Dot(KE[i], pj);
            }
        }

        Vex omegaVec = new Vex(omegaX, omegaY, omegaZ); // angular velocity of the body
        Vex HG = new Vex(rho2 * omegaX, rho2 * gammaY * omegaY, rho2 * gammaZ * omegaZ); // angular momentum
        Vex omegaCrossHG = Vex.Cross(omegaVec, HG); // gyroscopic torque contribution

        Vex torqueFromLeft = Vex.Cross(rFLG, aFL * mA); // torque from left arm on the body
        Vex torqueFromRight = Vex.Cross(rFRG, aFR * mA); // torque from right arm on the body

        Vex Q_rot = omegaCrossHG + torqueFromLeft + torqueFromRight; // total rotational generalized forces

        Qmat[0] = Q_rot.x;
        Qmat[1] = Q_rot.y;
        Qmat[2] = Q_rot.z;
        for (int i = 3; i < 8; i++) Qmat[i] = 0.0;

        // Check if the arm is free or prescribed
        // I added a getter to PlayterSim.cs to get this to work!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        bool armFree = !GetArmPrescribed();
        if (armFree) {
            // Section 7 | Equation 41
            tqL = -mA * L * L * (k * thetaL + c * omegaFL);   // left arm
            tqR =  mA * L * L * (k * thetaR + c * omegaFR);   // right arm (opposite sign)
        } else {
            tqL = 0.0;
            tqR = 0.0;
        }

        for (int i = 0; i < 8; i++) {
            PLmat[i] = 0.0;
            PRmat[i] = 0.0;
        }

        // Equation 43: Only partial angular velocities aligned with sz survive
        PLmat[3] = Vex.Dot(Vex.Cross(sz, rFLSL), sz);  // usually 1.0
        PRmat[4] = Vex.Dot(Vex.Cross(sz, rFRSR), sz);  // usually 1.0

        // fill up Qmat[3] and Qmat[4] from spring-damper torque model
        // generalized forces projected into omegaFL and omegaFR
        Vex fL = -mA * aFL;  // total force acting on left arm mass
        Vex fR = -mA * aFR;  // total force acting on right arm mass

        Qmat[3] = -mA * Vex.Dot(aFL, vFLpFL);  // Generalized force Q_L
        Qmat[4] = -mA * Vex.Dot(aFR, vFRpFR);  // Generalized force Q_R

        SetDebugVal(12, Qmat[3]);  // Show torque contribution from left arm
        SetDebugVal(13, Qmat[4]);  // Show torque contribution from right arm


        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                sys.SetA(i, j, Amat[i,j]);  // fill the A matrix
            }
            sys.SetB(i, -Qmat[i] + PLmat[i]*tqL + PRmat[i]*tqR);  // compute B
        }

        sys.SolveGauss();

        // checking for NaN
        for (int i = 0; i < 8; i++) {
            double sol = sys.Sol(i);
            if (double.IsNaN(sol) || double.IsInfinity(sol)) {
                SetDebugVal(0, 999.0);
                Debug.WriteLine("NaN or Inf detected in sys.Sol(" + i + "): " + sol);
            }
        }
        
        for (int i = 0; i < 8; i++) {
            ff[i] = sys.Sol(i);
            }


        SetDebugVal(0, Amat[0,0]);
        SetDebugVal(1, Amat[0,1]);
        SetDebugVal(2, Amat[0,2]);

        SetDebugVal(3, Amat[3,3]);
        SetDebugVal(4, Amat[4,4]);
        SetDebugVal(5, Amat[3,1]);

        SetDebugVal(6, Amat[5,5]);
        SetDebugVal(7, Amat[6,6]);
        SetDebugVal(8, Amat[7,7]);

        SetDebugVal(9, Qmat[0]);
        SetDebugVal(10, Qmat[1]);
        SetDebugVal(11, Qmat[2]);

        // SetDebugVal(12, -Qmat[3] + PLmat[3]*tqL + PRmat[3]*tqR);
        // SetDebugVal(13, -Qmat[4] + PLmat[4]*tqL + PRmat[4]*tqR);

        SetDebugVal(14, tqL);
        SetDebugVal(15, tqR);




        // // COMMENT THESE OUT OR REMOVE WHEN READY
        // //ff[0] = ff[1] = ff[2] = 0.0;   // derivs of body angular velocities set to zero
        // ff[3] = ff[4] = 0.0;           // derivs of arm angular velocities
        // ff[5] = ff[6] = ff[7] = 0.0;   // derivs of cener of mass velocities
        // //ff[8] = ff[9] = ff[10] = ff[11] = 0.0;  // derivs of quaternion coords
        // ff[12] = ff[13] = 0.0;         // derivs of arm angles
        // ff[14] = ff[15] = ff[16] = 0.0;  // derivs of CG coordinates
    }
} // end class