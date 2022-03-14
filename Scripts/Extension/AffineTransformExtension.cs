using AffineDecomposition.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition.Extensions {

    public static class AffineTransformExtension {

        public static float3x4 ToFloat3x4(this AffineTransform af) {
            var rs = math.mul(new float3x3(af.rotate), af.stretch);
            return new float3x4(rs.c0, rs.c1, rs.c2, af.translate);
        }
        public static float4x4 ToFloat4x4(this AffineTransform af)
            => new float4x4(math.mul(new float3x3(af.rotate), af.stretch), af.translate);

        public static float3x4 Tofloat3x4(this float4x4 m)
            => new float3x4(m.c0.xyz, m.c1.xyz, m.c2.xyz, m.c3.xyz);

        public static float3 Diag(this float3x3 m)
            => new float3(m[0][0], m[1][1], m[2][2]);
    }
}
