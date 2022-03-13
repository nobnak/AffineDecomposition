using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition.Model {

    public struct PolarDecompositionResult {

        public readonly float3x3 A;
        public readonly float3x3 U;
        public readonly float3x3 H;

        public readonly int totalIterations;

        public PolarDecompositionResult(float3x3 U, float3x3 H, float3x3 A, int totalIterations) {
            this.U = U;
            this.H = H;
            this.A = A;
            this.totalIterations = totalIterations;
        }
    }
}
