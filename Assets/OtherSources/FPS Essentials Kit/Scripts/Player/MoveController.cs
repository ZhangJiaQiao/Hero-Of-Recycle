using UnityEngine;

/// <summary>
/// Player motion status list.
/// </summary>
public enum MoveState
{
    Idle,
    Crouched,
    Walking,
    Running,
    Flying
}

/// <summary>
/// Class responsible for player movement.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MoveController : MonoBehaviour
{
    [HideInInspector]
    public bool isClimbing;

    [HideInInspector]
    public bool canVault;

    public MoveState moveState = MoveState.Idle;
    public float walkingSpeed = 5;
    public float crouchSpeed = 2;

    [Range(1, 3)]
    public float runMultiplier = 2; // Speed when sprinting = walkingSpeed * runMultiplier

    [Space()]
    public float jumpForce = 10;
    public bool airControll = true;

    [Space()]
    public bool stamina = true;

    [Tooltip("Decrement and increment the value of stamina per second")]
    public float drainRate = 5.0f;

    public AudioClip breathClip;

    [Space()]
    [HideInInspector]
    public int weight = 0;

    [Tooltip("Limits the collider to only climb slopes that are less steep (in degrees) than the indicated value.")]
    public float slopeLimit = 60;

	public AudioManager audioManager;
    public Camera playerCamera;
    public CameraAnimations cameraAnim;

    private Vector3 groundContactNormal;
	private float staminaAmount = 100;
    private float currentSpeed, stickToGroundHelperDistance = 0.5f, fallSpeed = 50, shellOffset = 0.1f;
    private bool previouslyGrounded, isGrounded, jump, jumping;
	private bool crouched, running, aiming;

	public bool isCrouched
	{
		get
		{
			return crouched;
		}
	}

	public bool isJumping
	{
		get
		{
			return jumping;
		}
	}

	private Rigidbody rigidBody
	{
		get 
		{ 
			return GetComponent<Rigidbody>(); 
		}
	}

	private CapsuleCollider capsuleCol
	{
		get 
		{ 
			return GetComponent<CapsuleCollider>();
		}
	}

    private HealthController healthController
    {
        get
        {
            return GetComponent<HealthController>();
        }
    }

	public bool Grounded
	{
		get 
		{ 
			return isGrounded; 
		}
	}

	public bool isAiming
	{
		set
		{
			aiming = value;
		}

		get 
		{
			return aiming;
		}
	}

	// Update is called once per frame
	private void Update ()
    {
        if (!isClimbing)
        {
            currentSpeed = UpdateCurrentSpeed();
            CheckCrouchedState();
            SetPlayerState();

            if (stamina)
            {
                UpdateStaminaAmount();
            }

            if (isGrounded)
            {
                if (Input.GetButtonDown("Jump") && !jump && !crouched)
                {
                    jump = true;
                }

                if (Input.GetButtonDown("Crouch") && !crouched)
                {
                    crouched = true;
                }
                else if ((Input.GetButtonDown("Crouch") || Input.GetButtonDown("Jump") || Input.GetButtonDown("Run"))
                    && crouched && DistanceUpwards() >= 1.6f)
                {
                    crouched = false;
                }
            }
        }
        else
        {
            jump = false;
            moveState = MoveState.Idle;
        } 
    }

    private void SetPlayerState ()
    {
        float currentSpeed = rigidBody.velocity.magnitude;
        float minMoveSpeed = RealWalkingSpeed() * 0.85f;
		bool idle = GetInput() == Vector2.zero;

        if (isGrounded && !jumping)
        {
			if (currentSpeed < minMoveSpeed * runMultiplier && currentSpeed > crouchSpeed && !CheckRunning() && !idle)
            {
                // Walking
                moveState = MoveState.Walking;
            }
			else if (currentSpeed > minMoveSpeed * RunMultiplierWithStamina() && !idle)
            {
                // Running
                moveState = MoveState.Running;
            }
			else if (currentSpeed < minMoveSpeed && currentSpeed > crouchSpeed * 0.85f && crouched && !idle)
            {
                // Crouched
                moveState = MoveState.Crouched;
            }
            else if (currentSpeed < crouchSpeed * 0.5f)
            {
                // Idle
                moveState = MoveState.Idle;
            }
        }
        else
        {
            if (healthController.CheckDistanceBelow() > jumpForce / 10f)
                moveState = MoveState.Flying;
        }
    }

    private void UpdateStaminaAmount ()
    {
        staminaAmount = Mathf.Clamp(staminaAmount, 0, 100);

        if (staminaAmount <= 50)
            audioManager.PlayBreathingSound(breathClip, staminaAmount, 50);

        if (CheckRunning())
        {
            staminaAmount -= drainRate * Time.deltaTime;
        }
        else
        {
            staminaAmount += drainRate * Time.deltaTime;
        }
    }

    private void CheckCrouchedState()
    {
        float crouchingSpeed = 8.0f;

        if (crouched)
        {
            capsuleCol.height = 1.2f;
            capsuleCol.center = new Vector3(0, -0.4f, 0);

            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(0, 0.05f, 0), Time.deltaTime * crouchingSpeed);
        }
        else
        {
            capsuleCol.height = 2;
            capsuleCol.center = Vector3.zero;

            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(0, 0.8f, 0), Time.deltaTime * crouchingSpeed);
        }       
    }

    private void FixedUpdate ()
    {
        if (!isClimbing)
        {
            GroundCheck();
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (airControll || isGrounded))
            {
                // Always move along the camera forward as it is the direction that it being aimed at
                //Vector3 desiredMove = playerCamera.transform.forward * input.y + playerCamera.transform.right * input.x;
                Vector3 desiredMove = gameObject.transform.forward * input.y + gameObject.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, groundContactNormal).normalized;

                desiredMove.x = desiredMove.x * currentSpeed;
                desiredMove.z = desiredMove.z * currentSpeed;
                desiredMove.y = desiredMove.y * currentSpeed;

                if (rigidBody.velocity.sqrMagnitude < (currentSpeed * currentSpeed))
                {
                    rigidBody.AddForce(desiredMove, ForceMode.Impulse);
                }
            }

            if (isGrounded)
            {
                rigidBody.drag = 5f;

                if (jump)
                {
                    rigidBody.drag = 0f;
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
                    rigidBody.AddForce(new Vector3(0f, jumpForce * 10, 0f), ForceMode.Impulse);
                    jumping = true;
                    cameraAnim.JumpShake();
                }

                if (!jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && rigidBody.velocity.magnitude < 1f)
                {
                    rigidBody.Sleep();
                }
            }
            else
            {
                rigidBody.drag = 0f;

                if (rigidBody.velocity.magnitude < fallSpeed)
                {
                    rigidBody.AddForce(Physics.gravity, ForceMode.Impulse);
                }

                if (previouslyGrounded && !jumping)
                {
                    StickToGroundHelper();
                }
            }
            jump = false;
        }
    }

    private float UpdateCurrentSpeed ()
    {
        if (crouched)
        {
            return crouchSpeed;
        }
        else
        {
            if (CheckRunning())
            {
                return RealWalkingSpeed() * (stamina ? RunMultiplierWithStamina() : runMultiplier);
            }
            else
            {
                return aiming ? RealWalkingSpeed() * 0.7f : RealWalkingSpeed();
            }
        }
    }

    public float RunMultiplierWithStamina ()
    {
        return stamina ? 1 + (staminaAmount * (runMultiplier - 1)) / 100 : runMultiplier;
    }

    public float RealWalkingSpeed ()
    {
        switch (weight)
        {
            case 0 : return walkingSpeed;
            case 1 : return walkingSpeed * 0.9f;
            case 2 : return walkingSpeed * 0.85f;
            case 3 : return walkingSpeed * 0.8f;
            case 4 : return walkingSpeed * 0.75f;
            case 5 : return walkingSpeed * 0.7f;
            default: return walkingSpeed;
        }
    }

    public bool CheckRunning ()
    {
        Vector2 input = GetInput();

        if (Input.GetButtonDown("Run"))
        {
            running = true;
        }

        if (Input.GetButton("Run") && running && input.y > 0 && !aiming)
        {
            return true;
        }
        else
        {
            running = false;
            return false;
        }
    } 

    private void StickToGroundHelper ()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, capsuleCol.radius * (1 - shellOffset), Vector3.down, out hitInfo,
                               ((capsuleCol.height / 2f) - capsuleCol.radius) +
                               stickToGroundHelperDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) > slopeLimit)
            {
                rigidBody.velocity = Vector3.ProjectOnPlane(rigidBody.velocity, hitInfo.normal);
            }
        }
    }

    private float DistanceUpwards ()
    {
        RaycastHit hitInfo;

        if (Physics.SphereCast(transform.position + capsuleCol.center - new Vector3(0, capsuleCol.height / 2, 0),
            capsuleCol.radius - 0.1f, transform.up, out hitInfo, 10))
        {
            return hitInfo.distance;
        }
        else
        {
            return 3;
        }
    }

    public Vector2 GetInput ()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };
        
        return input;
    }

    // Sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck ()
    {
        previouslyGrounded = isGrounded;
        RaycastHit hitInfo;
        
        if (Physics.SphereCast(transform.position, capsuleCol.radius * (1 - shellOffset), Vector3.down, out hitInfo,
                               ((capsuleCol.height / 2f) - capsuleCol.radius) + (crouched ? 0.5f : 0.05f), ~0, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            groundContactNormal = hitInfo.normal;
        }
        else
        {
            isGrounded = false;
            groundContactNormal = Vector3.up;
        }
        if (!previouslyGrounded && isGrounded && jumping)
        {
            jumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, 2, 0.5f));
    }
}