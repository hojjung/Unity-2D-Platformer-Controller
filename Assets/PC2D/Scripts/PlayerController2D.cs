using UnityEngine;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class PlayerController2D : MonoBehaviour
{
    private PlatformerMotor2D _motor;
    private bool _restored = true;
    private bool _enableOneWayPlatforms;
    private bool _oneWayPlatformsAreWalls;
    private bool _bPressing = false;
    float _pressTime;

    // Use this for initialization
    void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
    }

    // before enter en freedom state for ladders
    void FreedomStateSave(PlatformerMotor2D motor)
    {
        if (!_restored) // do not enter twice
            return;

        _restored = false;
        _enableOneWayPlatforms = _motor.enableOneWayPlatforms;
        _oneWayPlatformsAreWalls = _motor.oneWayPlatformsAreWalls;
    }
    // after leave freedom state for ladders
    void FreedomStateRestore(PlatformerMotor2D motor)
    {
        if (_restored) // do not enter twice
            return;

        _restored = true;
        _motor.enableOneWayPlatforms = _enableOneWayPlatforms;
        _motor.oneWayPlatformsAreWalls = _oneWayPlatformsAreWalls;
    }

    public void UpdateJoyStick(Vector2 pos)//점프 대시 버튼 별개
    {
        // use last state to restore some ladder specific values
        if (_motor.motorState != PlatformerMotor2D.MotorState.FreedomState)
        {
            // try to restore, sometimes states are a bit messy because change too much in one frame
            FreedomStateRestore(_motor);
        }

        // XY freedom movement
        if (_motor.motorState == PlatformerMotor2D.MotorState.FreedomState)
        {
            _motor.normalizedXMovement = pos.x;
            _motor.normalizedYMovement = pos.y;

            return; // do nothing more
        }

        // X axis movement
        if (Mathf.Abs(pos.x) > PC2D.Globals.INPUT_THRESHOLD)
        {
            _motor.normalizedXMovement = pos.x;
        }
        else
        {
            _motor.normalizedXMovement = 0;

            if (pos.y < -0.8f)
            {
                print("JumpOffInput");
                _motor.StartJumpOff();
            }
        }

        if (pos.y != 0)
        {
            bool up_pressed = pos.y > 0;

            if (_motor.IsOnLadder())
            {
                if (
                    (up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Top)
                    ||
                    (!up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Bottom)
                 )
                {
                    // do nothing!
                }
                // if player hit up, while on the top do not enter in freeMode or a nasty short jump occurs
                else
                {
                    // example ladder behaviour

                    _motor.FreedomStateEnter(); // enter freedomState to disable gravity
                    _motor.EnableRestrictedArea();  // movements is retricted to a specific sprite bounds

                    // now disable OWP completely in a "trasactional way"
                    FreedomStateSave(_motor);
                    _motor.enableOneWayPlatforms = false;
                    _motor.oneWayPlatformsAreWalls = false;

                    // start XY movement
                    _motor.normalizedXMovement = pos.x;
                    _motor.normalizedYMovement = pos.y;
                }
            }
        }
        else if (pos.y < -PC2D.Globals.FAST_FALL_THRESHOLD)
        {
            _motor.fallFast = false;
        }
    }

    void Update()
    {
        if (_bPressing)
        {
            _pressTime += Time.deltaTime;
        }

        _motor.jumpingHeld = _bPressing;
    }

    public void OnPointerDown()
    {
        _bPressing = true;
    }
    public void OnPointerUp()
    {
        _bPressing = false;
        _pressTime = 0;
    }

    public void JumpClick()
    {
        _motor.Jump();
        _motor.DisableRestrictedArea();
    }

    public void Dash()
    {
        _motor.Dash();
    }
}
