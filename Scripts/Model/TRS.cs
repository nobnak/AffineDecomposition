using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition.Model {

    public struct TRS {

        public readonly float3 translate;
        public readonly quaternion rotate;
        public readonly float3 scale;

        public TRS(float3 translate, quaternion rotate, float3 scale) {
            this.translate = translate;
            this.rotate = rotate;
            this.scale = scale;
        }

        #region interface

        #region static
        public static TRS Identity = new TRS(float3.zero, quaternion.identity, new float3(1f));

        public static implicit operator float4x4 (TRS trs) 
            => float4x4.TRS(trs.translate, trs.rotate, trs.scale);
        public static implicit operator TRS(Transform t)
            => new TRS(t.localPosition, t.localRotation, t.localScale);

        public static TRS Lerp(TRS a, TRS b, float t)
            => new TRS(
                math.lerp(a.translate, b.translate, t),
                math.slerp(a.rotate, b.rotate, t),
                math.lerp(a.scale, b.scale, t)
                );
        #endregion

        #region Object
        public override string ToString()
            => $"<{GetType().Name} : pos={translate}, rot={rotate}, scl={scale}>\n{(float4x4)this}";
        #endregion

        public TRS Apply(Transform tr) {
            tr.localPosition = translate;
            tr.localRotation = rotate;
            tr.localScale = scale;
            return this;
        }
        #endregion
    }
}
