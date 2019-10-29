using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Motion : MonoBehaviour
{

    [SerializeField]
    float Angle;

    [SerializeField]
    float power;

    [SerializeField]
    float gravity;

    [SerializeField]
    int iteration;

    [SerializeField]
    int ShowDotPerIteration;

    List<Vector3> trace_vectors;

    // Start is called before the first frame update
    void Start()
    {
        trace_vectors = new List<Vector3>(Mathf.RoundToInt(iteration / (float)ShowDotPerIteration));
    }

    private void Update() {
        Calculate();
    }

    private void Calculate() {
        trace_vectors.Clear();

        //Initial Velocity
        float ux = power * Mathf.Cos(Angle * Mathf.Deg2Rad);
        float uy = power * Mathf.Sin(Angle * Mathf.Deg2Rad);

        //Acceleration
        //float ax = 0, ay = gravity;

        //Time of Flight
        //float time = 2 * uy / (-gravity);


        //Velocity
        //float velocity_y = uy - (gravity * time);

        for (int i = 0; i < iteration; i++) {

            if (i % ShowDotPerIteration == 0) {
                float deltaTime = 0.02f;
                float time = deltaTime * i;
                float x = FindXPos(time, ux) + transform.position.x;
                float y = FindYPos(time, uy, gravity) + transform.position.y;

                //Debug.Log("X " + x +", Y " + y);
                trace_vectors.Add(new Vector3(x, y, 0));
            }
        }

    }

    float FindXPos(float time, float initial_velocity) {
        return initial_velocity * time;
    }

    float FindYPos(float time, float initial_velocity, float gravity)
    {
        return initial_velocity * time - (0.5f * gravity * Mathf.Pow(time, 2));
    }

    private void OnDrawGizmos()
    {
        if (trace_vectors != null)
        {
            Gizmos.color = Color.yellow;
            int t_count = trace_vectors.Count;
            for (int i = 0; i < t_count; i++)
            {
                Gizmos.DrawSphere(trace_vectors[i], 0.3f);
            }
        }
    }
}
