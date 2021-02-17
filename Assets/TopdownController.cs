using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.RVO;

public class TopdownController : MonoBehaviour
{
    public TopdownMotor motorWant;
    public RVOController con;

    public void UpdateInput(Vector2 v)
    {
        motorWant.normalizedXMovement = v.x;
        motorWant.normalizedYMovement = v.y;
    }

    private void Update()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");

        motorWant.normalizedXMovement = H;
        motorWant.normalizedYMovement = V;

        con.velocity = motorWant.GetVelocity;
    }
}
