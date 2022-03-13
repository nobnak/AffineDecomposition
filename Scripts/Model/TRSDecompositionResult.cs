using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition.Model {

    public struct TRSDecompositionResult {
        public readonly float3 translate;
        public readonly quaternion rotate;
        public readonly float3x3 stretch;

        public TRSDecompositionResult(float3 translate, quaternion rotate, float3x3 stretch) {
            this.translate = translate;
            this.rotate = rotate;
            this.stretch = stretch;
        }

        public override string ToString()
            => $"{GetType().Name}:\nt={translate}\nr={rotate}\ns={stretch}";
    }
}
