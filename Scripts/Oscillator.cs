using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AffineDecomposition.Examples {

    [ExecuteAlways]
    public class Oscillator : MonoBehaviour {
        public const float TWO_PI = 2f * Mathf.PI;

        public Tuner tuner = new Tuner();

        private void Update() {
            var t = Time.realtimeSinceStartup;

            if (tuner.translateOsc > 0)
                transform.localPosition = Vector3.Lerp(
                    tuner.translateFrom, 
                    tuner.translateTo, 
                    Oscilate(tuner.translateOsc * t));

            if (tuner.rotateOsc > 0)
                transform.localRotation = Quaternion.Euler(Vector3.Lerp(
                    tuner.rotateFrom, 
                    tuner.rotateTo, 
                    Oscilate(tuner.rotateOsc * t)));

            if (tuner.scaleOsc > 0)
                transform.localScale = Vector3.Lerp(
                    tuner.scaleFrom, 
                    tuner.scaleTo, 
                    Oscilate(t * tuner.scaleOsc));
        }

        public static float Oscilate(float t) {
            return Mathf.Clamp01(0.5f * (Mathf.Cos(t * TWO_PI) + 1f));
        }

        [System.Serializable]
        public class Tuner {
            public Vector3 translateFrom = Vector3.zero;
            public Vector3 translateTo = Vector3.zero;
            [Range(0f, 1f)]
            public float translateOsc = 0f;

            public Vector3 rotateFrom = Vector3.zero;
            public Vector3 rotateTo = Vector3.zero;
            [Range(0f, 1f)]
            public float rotateOsc = 0f;

            public Vector3 scaleFrom = Vector3.one;
            public Vector3 scaleTo = Vector3.one;
            [Range(0f, 10f)]
            public float scaleOsc = 0f;
        }

    }
}
