using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace AffineDecomposition.Examples {

    [ExecuteAlways]
    public class TRSSender : MonoBehaviour {

        public Events events = new Events();

        void Update() {
            var m = (float4x4)transform.localToWorldMatrix;
            var trs = new float3x4(m.c0.xyz, m.c1.xyz, m.c2.xyz, m.c3.xyz).Decompose();

            var pos = trs.translate;
            var rot = trs.rotate;
            var scl = new float3(trs.stretch[0][0], trs.stretch[1][1], trs.stretch[2][2]);

            events.TranslateOnChanged.Invoke(pos);
            events.RotateOnChanged.Invoke(rot);
            events.ScaleOnChanged.Invoke(scl);
        }

        [System.Serializable]
        public class Events {
            public Vector3Event TranslateOnChanged = new Vector3Event();
            public QuaternionEvent RotateOnChanged = new QuaternionEvent();
            public Vector3Event ScaleOnChanged = new Vector3Event();

            [System.Serializable]
            public class Vector3Event : UnityEvent<Vector3> { }
            [System.Serializable]
            public class QuaternionEvent : UnityEvent<Quaternion> { }
        }
    }
}