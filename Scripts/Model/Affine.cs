using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition.Model {

    public struct Affine {
        public readonly float3 translate;
        public readonly quaternion rotate;
        public readonly float3x3 stretch;

        public Affine(float3 translate, quaternion rotate, float3x3 stretch) {
            this.translate = translate;
            this.rotate = rotate;
            this.stretch = stretch;
        }

        #region interface

        #region object
        public override string ToString()
            => $"{GetType().Name}:\nt={translate}\nr={rotate}\ns={stretch}";
        #endregion

        #region static
        public static float3x3 Lerp(float3x3 a, float3x3 b, float t) 
            => new float3x3(
                math.lerp(a.c0, b.c0, t),
                math.lerp(a.c1, b.c1, t),
                math.lerp(a.c2, b.c2, t)
                );

        public static Affine Lerp(Affine a, Affine b, float t)
            => new Affine(
                math.lerp(a.translate, b.translate, t),
                math.slerp(a.rotate, b.rotate, t),
                Lerp(a.stretch, b.stretch, t));

        public static implicit operator Matrix4x4 (Affine a) => a.ToFloat4x4();
        #endregion

        #endregion
    }
}
