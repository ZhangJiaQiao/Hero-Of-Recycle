using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for managing all camera animations.
/// </summary>
public class CameraAnimations : MonoBehaviour 
{
	public MoveController controller; // Player.

	public GameObject walkingAnimations; // Walking animation gameObject.

    [Header("Hit Animations")]
    public BasicShake weaponsHitAnimation = new BasicShake(); // Weapon animation when hit.
    public BasicShake cameraHitAnimation = new BasicShake(); // Camera animation when hit.

    [Header("Fall Animations")]
	public BasicShake weaponsFallAnimation = new BasicShake(); // Weapon animation when fall.
    public BasicShake cameraFallAnimation = new BasicShake(); // Camera animation when fall.

    [Header("Jump Animations")]
	public BasicShake weaponsJumpAnimation = new BasicShake(); // Weapon animation when jump.
    public BasicShake cameraJumpAnimation = new BasicShake(); // Camera animation when jump.

    [Header("Parkour Animations")]
    public BasicShake cameraParkourAnimation = new BasicShake(); // Camera animation when vault.

    [Header("Weapon Recoil")]
	public Transform playerRoot; // Object that will be applied recoil animation.
    public Transform weaponRecoil; // Object that will be applied kick back animation.

    public Camera mainCamera; // Main camera.

    [Header("Breath Effect")]
	public bool breathEffect = true; // Breath animation?

    [Range(0, 2)]
	public float breathSpeed = 1; // Animation Speed.

    public Transform cameraBreathEffect; // Camera breathing animation transform.
    public Transform weaponBreathEffect; // Weapon breathing animation transform.
    private float breathAngle = 0; // Angle used to generate the breathing animation.

    [Header("Explosion Effect")]
    private float shakeAmount; // The amount to shake this frame.
    private float shakeDuration; // The duration this frame.

    private float shakeSpeed; // The shake speed.

    private float shakePercentage; // A percentage (0-1) representing the amount of shake to be applied when setting rotation.
    private float startAmount; // The initial shake amount (to determine percentage), set when ShakeCamera is called.
    private float startDuration; // The initial shake duration, set when ShakeCamera is called.

    private bool isRunning = false; // Is the coroutine running right now?

    public bool smooth; // Smooth rotation.

    public float smoothAmount = 5f; // Amount to smooth.

    public Transform cameraExplosionShake; // Explosion Shake Transform.

    [HideInInspector]
	public bool isFiring; // The weapon is stabilizing?

    private MoveState lastMoveState = MoveState.Idle; // Player state in the previous frame for a more precise verification of the change between states.

    private float animScale = 2f; // Animation scale.
    private float yVelocity = 0.0f; // Upwards speed.
    private float angle = 0.0f; // Current radians angle used to calculate animation position.

    private float currentRecoilAmount = 0; // Current amount of recoil applied.

    private bool isClimbing; // Player is vaulting or climbing on something?

    // Update is called once per frame
    private void Update () 
	{
        // If the player fell, return to original rotation.
        ReturnFromFall();
		ReturnFromJump ();

        returnFromHit(); // If the player is hit, returns to the original rotation.

        lastMoveState = controller.moveState; // Set the last move state as current move state.

        if (!isFiring) // The current weapon is stabilized?
        {
            // Return Weapon Recoil Transform to original rotation.
            weaponRecoil.localPosition = Vector3.Lerp (weaponRecoil.localPosition, Vector3.zero, 10 * Time.deltaTime);

            // Return Camera Recoil Transform to original rotation.
			playerRoot.localRotation = Quaternion.Lerp (playerRoot.localRotation, Quaternion.identity, 10 * Time.deltaTime);

			if (currentRecoilAmount > 0) // If the recoil is applied.
            {
				currentRecoilAmount -= Time.deltaTime * 10; // Returns the value of the recoil to 0.
            }
		}

        if (!isClimbing) // If the player are not climbing on something.
        {
            // Return Camera Parkour Transform to original rotation.
            cameraParkourAnimation.target.localRotation = Quaternion.Lerp(cameraParkourAnimation.target.localRotation, Quaternion.identity, cameraParkourAnimation.speed * 2 * Time.deltaTime);
        }
	}

    // This function is called every fixed framerate frame. It is used to play procedural animations.
    private void FixedUpdate ()
	{
		MoveAnimations (); // Play motion animation, like walk or run.
        BreathEffect (breathSpeed); // Play breathing animation.
    }

    /// <summary>
    /// Method responsible for simulate the animation of walking or running procedurally generated.
    /// </summary>
	private void MoveAnimations ()
	{
		if (controller.moveState == MoveState.Idle) // The player is stopped?
        {
			ResetMoving (); // Returns the animation to the starting position.
        }
		else if (controller.moveState == MoveState.Walking || controller.moveState == MoveState.Crouched) // The player is walking?
		{
            // Generates a motion animation with a slow transition speed.
            IsMoving(controller.RealWalkingSpeed() * (controller.isAiming || controller.moveState == MoveState.Crouched ?
				1.15f : 1.5f), controller.moveState);
		}
		else if (controller.moveState == MoveState.Running) // The player is running?
		{
            // Generates a motion animation with a fast transition speed.
            IsMoving(controller.RealWalkingSpeed() * (controller.stamina ? 
				controller.RunMultiplierWithStamina() : controller.runMultiplier) * 1.5f, controller.moveState);
		}
		else if (controller.moveState == MoveState.Flying && lastMoveState != controller.moveState && controller.isJumping) // The player is not touching the ground?
        {
			ResetMoving (); // Returns the animation to the starting position.
            StartCoroutine (CameraJumpAnimation ()); // Generates the jump animation on camera.
            StartCoroutine (WeaponJumpAnimation ()); // Generates the jump animation on weapon gameObject.
        }
	}

    /// <summary>
    /// Reset the weapon to the initial state of animation.
    /// </summary>
	private void ResetMoving ()
	{
		// Reset to a random initial state
		if(angle != 0 || angle != Mathf.PI)
			angle = Random.Range(0, 100) % 2 == 0 ? 0 : Mathf.PI;

        // Return to original position
        walkingAnimations.transform.localRotation = Quaternion.Lerp(walkingAnimations.transform.localRotation, 
			Quaternion.identity, Time.deltaTime * 10);
	}

    /// <summary>
    /// Generates a procedurally motion animation.
    /// Parameters: Animation speed and the current player move state.
    /// </summary>
	private void IsMoving (float speed, MoveState m)
	{
		angle += Time.deltaTime * speed; // Increments the angle over time with the given speed.
        if (angle > (2 * Mathf.PI)) // If the angle is greater than 2π radians, reset to 0.
            angle = 0;

        // Change the transform rotation with circular mathematical functions
        if (m == MoveState.Walking || m == MoveState.Crouched) 
		{
            // Calculates the rotation that the camera will be rotated in the next frame by multiplying the rotation by the sine and cosine values of the angle.
            float newPosition = Mathf.SmoothDamp (walkingAnimations.transform.localRotation.y, (Mathf.Sin (angle) * Mathf.Cos (angle) * -1)
			                    * animScale, ref yVelocity, 0.125f);

            // Rotate the camera to the next rotation previously calculated.
            walkingAnimations.transform.localRotation = Quaternion.Lerp (walkingAnimations.transform.localRotation, 
				Quaternion.Euler (new Vector3 (newPosition * animScale * 1.75f, Mathf.Sin (angle) * animScale / 2.5f)), 
				Time.deltaTime * speed);
		} 
		else if (m == MoveState.Running) 
		{
            // Calculates the rotation that the camera will be rotated in the next frame by multiplying the rotation by the sine and cosine values of the angle.
            float newPosition = Mathf.SmoothDamp (walkingAnimations.transform.localRotation.y, (Mathf.Sin (angle) * Mathf.Cos (angle) * -1)
			                    * animScale, ref yVelocity, 0.12f);

            // Rotate the camera to the next rotation previously calculated.
            walkingAnimations.transform.localRotation = Quaternion.Lerp (walkingAnimations.transform.localRotation, 
				Quaternion.Euler (new Vector3 (newPosition * Mathf.Pow (animScale, 2), Mathf.Sin (angle) * animScale / 2)), 
				Time.deltaTime * speed);
		}
	}

    
    /// <summary>
    /// Generates a procedurally breathing animation.
    /// Parameters: The animation speed.
    /// </summary>
    private void BreathEffect (float speed)
	{
		breathAngle += Time.deltaTime * speed; // Increments the angle with the given speed.

        if (breathAngle > (2 * Mathf.PI)) // If the angle is greater than 2π radians, reset to 0.
            breathAngle = 0;
		
		if (breathEffect) // Can generate the animation?
        {
            // Simulates the breath animation using mathematical functions to calculate transform rotation.
            weaponBreathEffect.localRotation = Quaternion.Lerp(weaponBreathEffect.localRotation, Quaternion.Euler(
				new Vector3((Mathf.Sin(breathAngle) * Mathf.Cos(breathAngle)), Mathf.Sin(breathAngle) / 2, 0)), Time.deltaTime * 10 * speed);
			
			if(controller.isAiming) // If are aiming, apply the effect on camera.
            {
				cameraBreathEffect.localRotation = Quaternion.Lerp(weaponBreathEffect.localRotation, Quaternion.Euler(
				new Vector3(0,(Mathf.Sin(breathAngle) * Mathf.Cos(breathAngle)), 0)), Time.deltaTime * 10 * speed);
			}
			else
			{
                // Reset the camera to the initial state of animation.
                cameraBreathEffect.localRotation = Quaternion.Lerp(cameraBreathEffect.localRotation, Quaternion.identity, Time.deltaTime * 10);
			}
		}
		else
		{
            // Reset the weapon and the camera to the initial state of animation.
            cameraBreathEffect.localRotation = Quaternion.Lerp(cameraBreathEffect.localRotation, Quaternion.identity, Time.deltaTime * 10);
			weaponBreathEffect.localRotation = Quaternion.Lerp(weaponBreathEffect.localRotation, Quaternion.identity, Time.deltaTime * 10);
		}
	}

    /// <summary>
    /// Returns from fall.
    /// </summary>
    public void ReturnFromFall ()
	{
		if (controller.moveState != MoveState.Flying) // The player is grounded?
        {
            // Reset the fall camera object to the original rotation.
            cameraFallAnimation.target.transform.localRotation = Quaternion.Lerp(cameraFallAnimation.target.transform.localRotation, 
				Quaternion.identity, Time.deltaTime * cameraFallAnimation.speed);

            // Reset the fall weapon object to the original rotation.
            weaponsFallAnimation.target.transform.localRotation = Quaternion.Lerp(weaponsFallAnimation.target.transform.localRotation, 
				Quaternion.identity, Time.deltaTime * weaponsFallAnimation.speed);
		}
	}

   
	/// <summary>
	/// Returns from jump.
	/// </summary>
    public void ReturnFromJump ()
	{
		if (controller.moveState == MoveState.Flying) // The player is Flying?
        {
			// Reset the jump camera object to the original rotation.
			cameraJumpAnimation.target.transform.localRotation = Quaternion.Lerp(cameraJumpAnimation.target.transform.localRotation, 
				Quaternion.identity, Time.deltaTime * cameraJumpAnimation.speed);

			// Reset the jump weapon object to the original rotation.
			weaponsJumpAnimation.target.transform.localRotation = Quaternion.Lerp(weaponsJumpAnimation.target.transform.localRotation, 
				Quaternion.identity, Time.deltaTime * weaponsJumpAnimation.speed);
		}
	}

	/// <summary>
	/// Returns from hit.
	/// </summary>
    public void returnFromHit()
    {
		// Reset the hit camera object to the original rotation.
        cameraHitAnimation.target.transform.localRotation = Quaternion.Lerp(cameraHitAnimation.target.transform.localRotation,
            Quaternion.identity, Time.deltaTime * cameraHitAnimation.speed);

		// Reset the hit weapon object to the original rotation.
        weaponsHitAnimation.target.transform.localRotation = Quaternion.Lerp(weaponsHitAnimation.target.transform.localRotation,
            Quaternion.identity, Time.deltaTime * weaponsHitAnimation.speed);
    }
		
	/// <summary>
	/// Play fall animation.
	/// </summary>
    public void FallShake ()
	{
		StartCoroutine (CameraFallShake ()); // Run the animation in the camera.
		StartCoroutine (WeaponFallShake ()); // Run the animation in the weapon.
	}

    /// <summary>
	/// Play jump animation.
	/// </summary>
    public void JumpShake()
    {
        StartCoroutine(CameraJumpAnimation()); // Run the animation in the camera.
        StartCoroutine(WeaponJumpAnimation()); // Run the animation in the weapon.
    }


    /// <summary>
    /// Play the parkour animation.
    /// </summary>
    public void PlayParkourAnimation ()
    {
		StartCoroutine(CameraParkourAnimation()); // Run the animation in the camera.
    }
		
	/// <summary>
	/// Applies a generic movement animation to simulate parkour animation on camera.
	/// </summary>
    private IEnumerator CameraParkourAnimation ()
    {
		// Set the initial rotation of the object.
        Quaternion startRotation = cameraParkourAnimation.target.localRotation;

		// Sets the end rotation of the animation before returning to origin.
        Quaternion endRotation = cameraParkourAnimation.target.localRotation * Quaternion.Euler(
            new Vector3(Random.Range(cameraParkourAnimation.intensity.min.y, cameraParkourAnimation.intensity.max.y) * -1,
                Random.Range(cameraParkourAnimation.intensity.min.x, cameraParkourAnimation.intensity.max.x) * -1));

		float t = 0.0f; // Animation progress.

		isClimbing = true; // Activates the climbing state.
        while (t < 1.0)
        {
			t += Time.deltaTime * cameraParkourAnimation.speed; // Increase animation progress.
			cameraParkourAnimation.target.localRotation = Quaternion.Lerp(startRotation, endRotation, t); // Move the object to its target rotation.
            yield return null;
        }
		isClimbing = false; // Disable the climbing state.
    }

	/// <summary>
	/// Applies a generic movement animation to simulate fall animation on weapon.
	/// </summary>
    private IEnumerator WeaponFallShake ()
	{
		// Set the initial rotation of the object.
		Quaternion startRotation = weaponsFallAnimation.target.localRotation;

		// Sets the end rotation of the animation before returning to origin.
		Quaternion endRotation = weaponsFallAnimation.target.localRotation * Quaternion.Euler(
			new Vector3(Random.Range(weaponsFallAnimation.intensity.min.y, weaponsFallAnimation.intensity.max.y) * -1,
				Random.Range(weaponsFallAnimation.intensity.min.x, weaponsFallAnimation.intensity.max.x) * -1));

		float t = 0.0f; // Animation progress.

		while (t < 1.0)
		{
			t += Time.deltaTime * weaponsFallAnimation.speed; // Increase animation progress.
			weaponsFallAnimation.target.localRotation = Quaternion.Lerp(startRotation, endRotation, t); // Move the object to its target rotation.
			yield return null;
		}
	}

	/// <summary>
	/// Applies a generic movement animation to simulate fall animation on camera.
	/// </summary>
	private IEnumerator CameraFallShake ()
	{
		// Set the initial rotation of the object.
		Quaternion startRotation = cameraFallAnimation.target.localRotation;

		// Sets the end rotation of the animation before returning to origin.
		Quaternion endRotation = cameraFallAnimation.target.localRotation * Quaternion.Euler(
			new Vector3(Random.Range(cameraFallAnimation.intensity.min.y, cameraFallAnimation.intensity.max.y) * -1,
				Random.Range(cameraFallAnimation.intensity.min.x, cameraFallAnimation.intensity.max.x) * -1));

		float t = 0.0f; // Animation progress.

		while (t < 1.0)
		{
			t += Time.deltaTime * cameraFallAnimation.speed; // Increase animation progress.
			cameraFallAnimation.target.localRotation = Quaternion.Lerp(startRotation, endRotation, t); // Move the object to its target rotation.
			yield return null;
		}
	}

	/// <summary>
	/// Applies a generic movement animation to simulate jump animation on weapon.
	/// </summary>
	private IEnumerator WeaponJumpAnimation ()
	{
		// Set the initial rotation of the object.
		Quaternion startRotation = weaponsJumpAnimation.target.localRotation;

		// Sets the end rotation of the animation before returning to origin.
		Quaternion endRotation = weaponsJumpAnimation.target.localRotation * Quaternion.Euler(
			new Vector3(Random.Range(weaponsJumpAnimation.intensity.min.y, weaponsJumpAnimation.intensity.max.y) * -1,
				Random.Range(weaponsJumpAnimation.intensity.min.x, weaponsJumpAnimation.intensity.max.x) * -1));

		float t = 0.0f; // Animation progress.

		while (t < 1.0)
		{
			t += Time.deltaTime * weaponsJumpAnimation.speed; // Increase animation progress.
			weaponsJumpAnimation.target.localRotation = Quaternion.Lerp(startRotation, endRotation, t); // Move the object to its target rotation.
			yield return null;
		}
	}

	/// <summary>
	/// Applies a generic movement animation to simulate jump animation on camera.
	/// </summary>
	private IEnumerator CameraJumpAnimation ()
	{
		// Set the initial rotation of the object.
		Quaternion startRotation = cameraJumpAnimation.target.localRotation;

		// Sets the end rotation of the animation before returning to origin.
		Quaternion endRotation = cameraJumpAnimation.target.localRotation * Quaternion.Euler(
			new Vector3(Random.Range(cameraJumpAnimation.intensity.min.y, cameraJumpAnimation.intensity.max.y) * -1,
				Random.Range(cameraJumpAnimation.intensity.min.x, cameraJumpAnimation.intensity.max.x) * -1));

		float t = 0.0f; // Animation progress.

		while (t < 1.0)
		{
			t += Time.deltaTime * cameraJumpAnimation.speed; // Increase animation progress.
			cameraJumpAnimation.target.localRotation = Quaternion.Lerp(startRotation, endRotation, t); // Move the object to its target rotation.
			yield return null;
		}
	}

	/// <summary>
	/// Play hit animation.
	/// </summary>
    public void HitShake()
    {
		StartCoroutine(CameraHitAnimation()); // Run the animation in the camera.
		StartCoroutine(WeaponHitAnimation()); // Run the animation in the weapon.
    }

	/// <summary>
	/// Applies a generic movement animation to simulate hit animation on camera.
	/// </summary>
    private IEnumerator CameraHitAnimation()
    {
		// Set the initial rotation of the object.
        Quaternion startRotation = cameraHitAnimation.target.localRotation;

		// Sets the end rotation of the animation before returning to origin.
        Quaternion endRotation = cameraHitAnimation.target.localRotation * Quaternion.Euler(
            new Vector3(Random.Range(cameraHitAnimation.intensity.min.y, cameraHitAnimation.intensity.max.y) * -1,
                Random.Range(cameraHitAnimation.intensity.min.x, cameraHitAnimation.intensity.max.x) * -1));

		float t = 0.0f; // Animation progress.

        while (t < 1.0)
        {
			t += Time.deltaTime * cameraHitAnimation.speed; // Increase animation progress.
			cameraHitAnimation.target.localRotation = Quaternion.Lerp(startRotation, endRotation, t); // Move the object to its target rotation.
            yield return null;
        }
    }

	/// <summary>
	/// Applies a generic movement animation to simulate hit animation on weapon.
	/// </summary>
    private IEnumerator WeaponHitAnimation()
    {
		// Set the initial rotation of the object.
        Quaternion startRotation = weaponsHitAnimation.target.localRotation;

		// Sets the end rotation of the animation before returning to origin.
        Quaternion endRotation = weaponsHitAnimation.target.localRotation * Quaternion.Euler(
            new Vector3(Random.Range(weaponsHitAnimation.intensity.min.y, weaponsHitAnimation.intensity.max.y) * -1,
                Random.Range(weaponsHitAnimation.intensity.min.x, weaponsHitAnimation.intensity.max.x) * -1));

		float t = 0.0f; // Animation progress.

        while (t < 1.0)
        {
			t += Time.deltaTime * weaponsHitAnimation.speed; // Increase animation progress.
			weaponsHitAnimation.target.localRotation = Quaternion.Lerp(startRotation, endRotation, t); // Move the object to its target rotation.
            yield return null;
        }
    }
		
	/// <summary>
	/// Invokes the method that applies the recoil.
	/// </summary>
    public void PlayRecoil(bool aiming, float recoilAmount, float recoilAmountAiming, float minSidewaysRecoil, float maxSidewaysRecoil, float recoilTimer, bool kickBack, float weaponKick, bool limitUpwardsRecoil, float maxUpwardsRecoil)
	{
		// Recoil restrictions.
        bool canRecoil = (playerRoot.localRotation.x * Mathf.Rad2Deg * 2 + mainCamera.transform.localRotation.x * Mathf.Rad2Deg * 2)
		                 > mainCamera.GetComponent<MouseController> ().minimumY && (playerRoot.localRotation.x * Mathf.Rad2Deg * 2
						+ mainCamera.transform.localRotation.x * Mathf.Rad2Deg * 2) < mainCamera.GetComponent<MouseController> ().maximumY; 

		if (canRecoil)
			StartCoroutine (Recoil (aiming, recoilAmount, recoilAmountAiming, minSidewaysRecoil, maxSidewaysRecoil, recoilTimer, kickBack, weaponKick, limitUpwardsRecoil, maxUpwardsRecoil));
	}

	/// <summary>
	/// Simulates the recoil effect, based on the parameters.
	/// </summary>
	private IEnumerator Recoil (bool aiming, float recoilAmount, float recoilAmountAiming, float minSidewaysRecoil, float maxSidewaysRecoil, float recoilTimer, bool kickBack, float weaponKick, bool limitUpwardsRecoil, float maxUpwardsRecoil)
	{
		// Sets the intensity of recoil based on the aiming state.
		float recoilIntensity = (aiming ? -recoilAmountAiming : -recoilAmount);

		// Sets the intensity of kick back based on the aiming state.
		float kick = (aiming ? -weaponKick * 0.6f : -weaponKick);

		Vector3 endPosition = Vector3.zero; // The end position of the animation before returning to origin.
		Quaternion endRotation = Quaternion.identity; // The end rotation of the animation before returning to origin.

		if (limitUpwardsRecoil) // Limit vertical recoil?
		{
			if (currentRecoilAmount < maxUpwardsRecoil) // If the amount of recoil applied is less than the maximum.
			{
				// Sets the end rotation of the animation before returning to origin.
				endRotation = playerRoot.localRotation * Quaternion.Euler (new Vector3 (recoilIntensity, Random.Range (minSidewaysRecoil, maxSidewaysRecoil), 0));
				currentRecoilAmount += Time.deltaTime * 10; // Increase the amount of recoil applied.
			} 
			else
			{
				// Prevents the gun exceeds the vertical limit.
				endRotation = playerRoot.localRotation * Quaternion.Euler(new Vector3(0, Random.Range(minSidewaysRecoil, maxSidewaysRecoil), 0));
			}
				
		}
		else
		{
			// If have no limit, apply the vertical recoil.
			endRotation = playerRoot.localRotation * Quaternion.Euler(new Vector3(recoilIntensity, Random.Range(minSidewaysRecoil, maxSidewaysRecoil), 0));
		}

		if (kickBack) // Apply kickback?
		{
			// Sets the end position of the animation before returning to origin.
			endPosition = weaponRecoil.localPosition + new Vector3 (0, 0, Random.Range (kick / 200, kick / 100));
		}
			
		float t = 0.0f; // Animation progress.

		while (t < 1.0)
		{
			t += Time.deltaTime / recoilTimer; // Increase animation progress.

			if (kickBack) // Apply kickback?
				weaponRecoil.localPosition = Vector3.Lerp (weaponRecoil.localPosition, endPosition, t); // Move the weapon to kickback position.
			
			playerRoot.localRotation = Quaternion.Lerp(playerRoot.localRotation, endRotation, t); // Move the object to its recoil rotation.
			yield return null;
		}
	}

	/// <summary>
	/// Play camera explosion animation.
	/// </summary>
    public void ExplosionShakeCamera(float amount, float duration, float speed)
    {
		shakeAmount += amount; // Add to the current amount.
		startAmount = shakeAmount; // Reset the start amount, to determine percentage.
		shakeDuration += duration; // Add to the current time.
		startDuration = shakeDuration; // Reset the start time.
        shakeSpeed = speed;

        //Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
		if (!isRunning)
		{
			StartCoroutine(ExplosionShake());
		}  
    }

	/// <summary>
	/// Simulates the shaking of an explosion.
	/// </summary>
    private IEnumerator ExplosionShake()
    {
        isRunning = true;

        while (shakeDuration > 0.01f)
        {
			Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount; // A Vector3 to add to the Local Rotation.
			rotationAmount.z = 0; // Don't change the Z.

			shakePercentage = shakeDuration / startDuration; // Used to set the amount of shake (% * startAmount).

			shakeAmount = startAmount * shakePercentage; // Set the amount of shake (% * startAmount).

			shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime * shakeSpeed); // Lerp the time, so it is less and tapers off towards the end.

            if (smooth)
            {
                cameraExplosionShake.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * smoothAmount);
            }
            else
            {
                // Set the local rotation the be the rotation amount.
                cameraExplosionShake.localRotation = Quaternion.Euler(rotationAmount);
            }
            yield return null;
        }

        // Set the local rotation to 0 when done, just to get rid of any fudging stuff.
        cameraExplosionShake.localRotation = Quaternion.identity;
        isRunning = false;
    }
}
