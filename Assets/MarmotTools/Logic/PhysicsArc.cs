using UnityEngine;

namespace MyMarmot.Tools
{
    public static class PhysicsArc
    {
        public static void LaunchRigidbodyArc3D(this Physics target, Rigidbody rigidbody, float height, Vector3 coord, bool isDebug = false)
        {
            
            float Gravity = Physics.gravity.y;
            rigidbody.useGravity = true;
            rigidbody.velocity = CalculateData(Gravity, rigidbody, height, coord).initialVelocity;

            if (isDebug)
            {
                DrawPath(Gravity, rigidbody, height, coord);
            }
        }

        static LaunchData CalculateData(float gravity, Rigidbody rigidbody, float height, Vector3 coord)
        {
            float displacementY = coord.y - rigidbody.position.y;
            Vector3 displacementXZ = new Vector3(coord.x - rigidbody.position.x, 0, coord.z - rigidbody.position.z);
            float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
            Vector3 velocityXZ = displacementXZ / time;

            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }

        static void DrawPath(float gravity, Rigidbody rigidbody, float height, Vector3 coord)
        {
            if (rigidbody != null)
            {
                LaunchData launchData = CalculateData(gravity, rigidbody, height, coord);

                Vector3 previousDrawPoint = rigidbody.position;

                int resolution = 30;

                for (int i = 1; i <= resolution; i++)
                {
                    float simulationTIme = i / (float)resolution * launchData.timeToTarget;
                    Vector3 displacement = launchData.initialVelocity * simulationTIme + Vector3.up * gravity * simulationTIme * simulationTIme / 2f;

                    Vector3 drawPoint = rigidbody.position + displacement;

                    Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
                    previousDrawPoint = drawPoint;
                }
            }
        }

        struct LaunchData
        {
            public readonly Vector3 initialVelocity;
            public readonly float timeToTarget;

            public LaunchData(Vector3 initialVelocity, float timeToTarget)
            {
                this.initialVelocity = initialVelocity;
                this.timeToTarget = timeToTarget;
            }
        }
    }
}