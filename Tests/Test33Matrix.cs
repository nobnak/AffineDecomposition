using AffineDecomposition.Extensions;
using AffineDecomposition.Model;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace AffineDecomposition.Tests {
    public class Test33Matrix {
        private const float SIGMA = 2f * float.Epsilon;

        // A Test behaves as an ordinary method
        [Test]
        public void Test33MatrixSimplePasses() {
            var m = float3x3.zero;
            for (var r = 0; r < 3; r++) {
                var sign = ((r % 2) == 0) ? 1 : -1;
                for (var c = 0; c < 3; c++) {
                    m[c][r] = sign * (c + r * 3);
                }
            }
            m.AreEqual(new float3x3(0, 1, 2, -3, -4, -5, 6, 7, 8));

            Assert.AreEqual(3, math.csum(new float3(0, 1, 2)));
            Assert.AreEqual(15f, m.Norm_1());
            Assert.AreEqual(21f, m.Norm_inf());
        }

        [Test]
        public void TestPolar() {
            var rot = float3x3.EulerXYZ(0f, 0f, 0.5f * math.PI);
            var scale = float3x3.Scale(10f, -200f, 3000f);
            var A = math.mul(rot, scale);

            var res = A.PolarDecompose();

            Debug.Log($"Result : loops={res.totalIterations}\nU: {res.U}\nH: {res.H}\nA: {res.A}");
            A.AreEqual(math.mul(res.U, res.H));
        }

        [Test, Performance]
        public void TestPerformance() {
            var rot = float3x3.EulerXYZ(0f, 0f, 0.5f * math.PI);
            var scale = float3x3.Scale(10f, -200f, 3000f);
            var A = math.mul(rot, scale);

            Measure.Method(() => {
                var res = A.PolarDecompose();
            })
                .IterationsPerMeasurement(1000)
                .MeasurementCount(10)
                .Run();
        }

        [Test]
        public void TestQuaternion() {
            var q0 = Quaternion.Euler(0f, 0f, 90f);
            var rot0 = Matrix4x4.Rotate(q0);

            var rot1 = float3x3.EulerXYZ(0f, 0f, 0.5f * math.PI);
            var q1 = math.quaternion(rot1);
            var q2 = rot1.ToQuaternion();

            ((float4)(q0.ToVector4())).AreEqual(q1.value);
            rot1.AreEqual(new float3x3((float4x4)rot0));
            q1.AreEqual(q2);
        }

        [Test]
        public void TestAffineDecomposition() {
            var tr = new float3(1, -2, 3);
            var rot = quaternion.Euler(0f, 0f, 0.5f * math.PI);
            var sc = new float3(1, 20, 300);
            var Linear = math.mul(new float3x3(rot), float3x3.Scale(sc));
            var Affine = new float3x4(Linear.c0, Linear.c1, Linear.c2, tr);

            var trs = Affine.DecomposeToTRS();
            tr.AreEqual(trs.translate);
            rot.AreEqual(trs.rotate);
            float3x3.Scale(sc).AreEqual(trs.stretch);
        }

        [Test]
        public void TestAffineTransform() {
            var tr = new float3(1, -2, 3);
            var rt = quaternion.EulerXYZ(0f, 0f, 0.5f * math.PI);
            var sc = float3x3.Scale(1, 10, 100);
            var a = new AffineTransform(tr, rt, sc);

            var a34 = a.ToFloat3x4();
            math.mul(new float3x3(rt), sc).AreEqual(new float3x3(a34.c0, a34.c1, a34.c2));
            tr.AreEqual(a34.c3);

            var a44 = a.ToFloat4x4();
            new float4(0, 0, 0, 1).AreEqual(new float4(a44[0][3], a44[1][3], a44[2][3], a44[3][3]));
        }
    }

    public static class TestUtils {
        private const float DELTA = 1e-3f;

        public static void AreEqual(this float3 a, float3 b, float delta = DELTA) {
            for (var i = 0; i < 3; i++)
                Assert.AreEqual(a[i], b[i], delta, $"at i={i}");
        }
        public static void AreEqual(this float4 a, float4 b, float delta = DELTA) {
            for (var i = 0; i < 4; i++)
                Assert.AreEqual(a[i], b[i], delta, $"at i={i}");
        }
        public static void AreEqual(this quaternion a, quaternion b, float delta = DELTA) => AreEqual(a.value, b.value, delta);

        public static void AreEqual(this float3x3 a, float3x3 b) {
            for (var x = 0; x < 3; x++)
                for (var y = 0; y < 3; y++)
                    a[x].AreEqual(b[x], 1e-3f);
        }
        
    }
}
