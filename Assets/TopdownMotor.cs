using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownMotor : MonoBehaviour
{
    public float normalizedXMovement { get; set; }

    public float normalizedYMovement { get; set; }

    public float moveSpeed = 10;

    public float timeToWallSlideSpeed = 3;

    public LayerMask staticEnvLayerMask;

    public LayerMask movingPlatformLayerMask;

    private LayerMask _collisionMask;

    private static RaycastHit2D[] _hits = new RaycastHit2D[4];

    private static Collider2D[] _overlappingColliders = new Collider2D[4];

    private Collider2D _collider2D { get; set; }

    private Vector3 _previousLoc;

    private Vector3 _toTransform;

    private Vector2 _velocity;

    public Vector2 GetVelocity
    {
        get
        {
            return _velocity;
        }
    }

    private float _minDistanceFromEnv = 0.02f;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _collisionMask = staticEnvLayerMask | movingPlatformLayerMask;
    }

    public void SetDestMove(Vector2 dest)
    {
        Vector3 Dir = transform.position - (Vector3)dest;

        float Mag = Dir.magnitude;

        var hit = GetClosestHit(_collider2D.bounds.center,Dir, _minDistanceFromEnv);

        if (hit.collider != null)
        {
            Vector3 Another = Vector3.Cross(Dir, hit.normal);
            Another = Vector3.Cross(hit.normal, Another);
            _velocity = Another;
        }
    }

    void Update()
    {
        _toTransform = transform.position - _collider2D.bounds.center;

        UpdateVelocity();

        SetFacing();

        MovePosition(_collider2D.bounds.center + (Vector3)_velocity * Time.smoothDeltaTime);
    }

    private void OnDrawGizmos()
    {
        if(_collider2D)
        Gizmos.DrawLine(_collider2D.bounds.center, _collider2D.bounds.center + ((Vector3)_velocity * 1f));
    }

    private void UpdateVelocity()
    {
        ApplyMovement();
        WallSlide();
    }

    private void ApplyMovement()
    {
        _velocity.x = normalizedXMovement;
        _velocity.y = normalizedYMovement;
        _velocity.Normalize();
        _velocity *= moveSpeed;
    }

    private void SetFacing()
    {

    }

    private void WallSlide()
    {
        Vector3 Input = Vector3.zero;
        Input.x = normalizedXMovement;
        Input.y = normalizedYMovement;

        Vector3 toNewPos = _velocity;

        float distance = toNewPos.magnitude;

        RaycastHit2D hit = GetClosestHit(_collider2D.bounds.center, Input, 0.05f);

        if (hit.collider != null)
        {
            Vector3 Another = Vector3.Cross(_velocity, hit.normal);
            Another = Vector3.Cross(hit.normal, Another);
            _velocity = Another;
        }
    }

    private void MovePosition(Vector3 newPos)
    {
        if (newPos == _collider2D.bounds.center)
        {
            _previousLoc = _collider2D.bounds.center;
            return;
        }

        Vector3 toNewPos = newPos - _collider2D.bounds.center;
        float distance = toNewPos.magnitude;

        RaycastHit2D hit = GetClosestHit(_collider2D.bounds.center, toNewPos / distance, distance);

        _previousLoc = _collider2D.bounds.center;

        if (hit.collider != null)
        {
            transform.position = ( _toTransform + (Vector3)hit.centroid + (Vector3)hit.normal * _minDistanceFromEnv);
        }
        else
        {
            transform.position = ( _toTransform + newPos);
        }
        
        // at the end if there is a restricted area, force the motor inside
        // TODO handle rotation, unrotate transform.position, check, rotate
        //if (IsRestricted())
        //{
        //    Vector2 pos;
        //    pos.x = Mathf.Clamp(transform.position.x, _restrictedAreaBL.x, _restrictedAreaTR.x);
        //    pos.y = Mathf.Clamp(transform.position.y, _restrictedAreaBL.y, _restrictedAreaTR.y);
        //    transform.position = pos;
        //}
    }

    private RaycastHit2D GetClosestHit(
      Vector2 origin,
      Vector3 direction,
      float distance,
      bool useBox = true
      )
    {
        //_collider2D.enabled = false;

        RaycastHit2D Result;

        if (useBox)
        {
            Result =Physics2D.BoxCast(
                origin,
                _collider2D.bounds.size,
                0f,
                direction,
                distance,
                _collisionMask);
        }
        else
        {
            Result = Physics2D.Raycast(origin, direction, distance, _collisionMask);
        }

        //_collider2D.enabled = true;

        return Result;
    }

    private float Accelerate(float speed, float acceleration, float limit)
    {
        // acceleration can be negative or positive to note acceleration in that direction.
        speed += acceleration * Time.fixedDeltaTime;

        if (acceleration > 0)
        {
            if (speed > limit)
            {
                speed = limit;
            }
        }
        else
        {
            if (speed < limit)
            {
                speed = limit;
            }
        }

        return speed;
    }

    private float Decelerate(float speed, float deceleration, float limit)
    {
        // deceleration is always positive but assumed to take the velocity backwards.
        if (speed < 0)
        {
            speed += deceleration * Time.fixedDeltaTime;

            if (speed > limit)
            {
                speed = limit;
            }
        }
        else if (speed > 0)
        {
            speed -= deceleration * Time.fixedDeltaTime;

            if (speed < limit)
            {
                speed = limit;
            }
        }

        return speed;
    }

}
