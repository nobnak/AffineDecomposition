using AffineDecomposition.Model;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AffineDecomposition.Examples {

    public class Interpolation : MonoBehaviour {

        public Link link = new Link();
        public Tuner tuner = new Tuner();

        Affine ainterp;

        private void OnEnable() {
            ainterp = (Affine)link.keys[0].localToWorldMatrix;
        }
        private void Update() {
            if (link.keys.Count < 2) return;

            var t = Time.time * tuner.speed;

            var it = (int)t;
            var t0 = Mathf.Clamp01(t - it);
            var i = it % link.keys.Count;
            var trFrom = link.keys[i];
            var trTo = link.keys[(i + 1) % link.keys.Count];

            var afrom = (Affine)trFrom.localToWorldMatrix;
            var ato = (Affine)trTo.localToWorldMatrix;
            
            var cfrom = trFrom.GetComponent<Renderer>().sharedMaterial.color;
            var cto = trTo.GetComponent<Renderer>().sharedMaterial.color;
            var cinterp = Color.Lerp(cfrom, cto, t0);
            GetComponent<Renderer>().sharedMaterial.color = cinterp;

            ainterp = Affine.Lerp(afrom, ato, t0);
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