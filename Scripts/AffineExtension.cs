using AffineDecomposition.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition {

    public static class AffineExtension {
        public const float SIGMA = 10f * float.Epsilon;

        public static Affine Decompose(this float3x4 m) {
            var translate = m.c3;
            
            var A = new float3x3(m.c0, m.c1, m.c2);
            var polar = A.PolarDecompose();

            var rotate = math.quaternion(polar.U);
            var stretch = polar.H;
            return new Affine(translate, rotate, stretch);
        }

        //Higham, N.J.Computing the Polar Decomposition—with Applications.Siam J Sci Stat Comp 7, 1160–1174 (1986).
        //https://doi.org/10.1137/0907079
        public static PolarDecompositionResult PolarDecompose(this float3x3 A, int iterationLimit = 100) {
            int i;
            var U = A;
            var closeToConvergence = false;

            for (i = 0; i < iterationLimit; i++) {
                var y = math.inverse(U);
                var r = 1f;

                if (!closeToConvergence) {
                    var a = math.sqrt(U.Norm_1() * U.Norm_inf());
                    var b = math.sqrt(y.Norm_1() * y.Norm_inf());
                    r = math.sqrt(b / a);
                }

                var U1 = (r * U + math.transpose(y) / r) / 2;

                var diffnorm1 = (U1 - U).Norm_1();
                if (closeToConvergence) {
                    if (diffnorm1 <= SIGMA) break;
                } else {
                    closeToConvergence = (diffnorm1 <= (10f * SIGMA) * U.Norm_1());
                }
                U = U1;
            }

            var H = math.mul(math.transpose(U), A);
            H = (H + math.transpose(H)) / 2;

            return new PolarDecompositionResult(U, H, A, i);
        }

        //Shepperd, S.W.Quaternion from Rotation Matrix.J Guid Control 1, 223–224 (2019).
        //https://doi.org/10.2514/3.55767b
        public static float4 ToQuaternion(this float3x3 U) {
            var m00 = U[0][0]; var m10 = U[1][0]; var m20 = U[2][0]; 
            
            var m01 = U[0][1]; var m11 = U[1][1]; var m21 = U[2][1];
            
            var m02 = U[0][2]; var m12 = U[1][2]; var m22 = U[2][2];

            float t;
            float4 q;
            if (m22 < 0) { 
                if (m00 > m11) { 
                    t = 1 + m00 - m11 - m22; 
                    q = new float4(t, m01 + m10, m20 + m02, m12 - m21); 
                } else { 
                    t = 1 - m00 + m11 - m22; 
                    q = new float4(m01 + m10, t, m12 + m21, m20 - m02); 
                } 
            } else {
                if (m00 < -m11) { 
                    t = 1 - m00 - m11 + m22; 
                    q = new float4(m20 + m02, m12 + m21, t, m01 - m10);
                } else { 
                    t  = 1 + m00 + m11 + m22;
                    q = new float4(m12 - m21, m20 - m02, m01 - m10, t);
                } 
            }
            q *= 0.5f / math.sqrt(t);
            return q;
        }

        public static float Norm_1(this float3x3 m) {
            return math.max(math.max(
                math.csum(math.abs(m[0])),
                math.csum(math.abs(m[1]))),
                math.csum(math.abs(m[2])));
        }
        public static float Norm_inf(this float3x3 m) {
            return math.transpose(m).Norm_1();
        }

        public static Vector4 ToVector4(this Quaternion q) => new Vector4(q.x, q.y, q.z, q.w); 
        
        public static float3x4 ToFloat3x4(this Affine af) {
            var rs = math.mul(new float3x3(af.rotate), af.stretch);
            return new float3x4(rs.c0, rs.c1, rs.c2, af.translate);
        }
        public static float4x4 ToFloat4x4(this Affine af) {
            var rs = math.mul(new float3x3(af.rotate), af.stretch);
            return new float4x4(rs, af.translate);
        }

        public static float3x4 ToFloat3x4(this float4x4 m)
            => new float3x4(m.c0.xyz, m.c1.xyz, m.c2.xyz, m.c3.xyz);

        public static float3 Diag(this float3x3 m)
            => new float3(m[0][0], m[1][1], m[2][2]);
    }
}