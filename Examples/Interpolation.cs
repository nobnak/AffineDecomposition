using AffineDecomposition.Extensions;
using AffineDecomposition.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition.Examples {

    public class Interpolation : MonoBehaviour {

        public Link link = new Link();
        public Tuner tuner = new Tuner();

        private void Update() {
            if (link.keys.Count < 2) return;

            var t = tuner.speed * Time.realtimeSinceStartup;

            var it = (int)t;
            var t0 = Mathf.Clamp01(t - it);
            var i = it % link.keys.Count;
            var trFrom = link.keys[i];
            var trTo = link.keys[(i + 1) % link.keys.Count];

            var exFrom = ((float4x4)(trFrom.localToWorldMatrix)).Tofloat3x4();
            var afrom = exFrom.DecomposeToTRS();

            var exTo = ((float4x4)(trTo.localToWorldMatrix)).Tofloat3x4();
            var ato = exTo.DecomposeToTRS();

            var ainterp = AffineTransform.Lerp(afrom, ato, t0);
            transform.position = ainterp.translate;
            transform.rotation = ainterp.rotate;
            transform.localScale = ainterp.stretch.Diag();
        }

        [System.Serializable]
        public class Link {
            public List<Transform> keys = new List<Transform>();
        }
        [System.Serializable]
        public class Tuner {
            public float speed = 0f;
        }
    }

}