using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

public class RVOMove : MonoBehaviour
{
    public Transform target;

    public float nextWayPointDist = 3.0f;

    Path path;

    int currentWaypoint = 0;

    bool reachEndOfPath = false;

    Seeker seeker;

    RVOController rvoCon;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rvoCon = GetComponent<RVOController>();

        InvokeRepeating("UpdatePath", 0, 0.5f);

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (path != null)
        {
            path.Release(this);
        }

        path = p;

        p.Claim(this);

        currentWaypoint = 0;

        if (p.error)
        {
            //currentWaypoint = 0;
            return;
        }
    }

    void Update()
    {

        if (path == null)
        {
            return;
        }
        //
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;

            return;
        }
        else
        {
            reachEndOfPath = false;
        }
        //
        rvoCon.SetTarget(path.vectorPath[currentWaypoint], 4, 4);
        //
        Vector3 RvoDelta = (rvoCon.CalculateMovementDelta(Time.deltaTime));
        
        transform.position += RvoDelta;

        //
        TryIncreaseWaypoint();
    }


    private void TryIncreaseWaypoint()
    {
        float distanceSqr = (transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude;

        if (distanceSqr <= nextWayPointDist * nextWayPointDist)
        {
            currentWaypoint++;
        }
    }
}
