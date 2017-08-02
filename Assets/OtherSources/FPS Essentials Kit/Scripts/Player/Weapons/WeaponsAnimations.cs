using UnityEngine;

/// <summary>
/// Class responsible for managing all weapon animations.
/// </summary>
[RequireComponent(typeof(Weapon))]
public class WeaponsAnimations : MonoBehaviour 
{
    /// <summary>
    /// Weapon component attached to this gameObject.
    /// </summary>
	public Weapon weapon
	{
		get
		{
			return GetComponent<Weapon> (); // Get the component attached in the gameObject.
        }
	}
	
	public AudioManager audioManager; // Audio Manager.
    public Animation weaponAnimation; // Object that will play the animations.
	
	public bool fire = true; // Does this weapon have fire animations?

    public string shot = "Fire"; // Hip fire animation name.
    public string aimedShot = "Fire"; // Aim fire animation name.

	public AudioClip shotSound; // Fire sound.
	public float shotVolume = 0.5f; // Fire sound volume.

	public bool drawHide = true; // Does this weapon have draw or hide animations?

    public string drawAnim = "Draw"; // Draw animation name.
	public string hideAnim = "Hide"; // Hide animation name.

	public AudioClip drawSound; // Draw sound.
	public AudioClip hideSound; // Hide sound.
    public float drawVolume = 0.3f; // Draw sound volume.
    public float hideVolume = 0.3f; // Hide sound volume.

    public bool melee; // Does this weapon have melee animations?

    public string meleeAnim = "Melee"; // Melee animation name.
    public AudioClip meleeSound; // Melee sound.
    public float meleeVolume = 0.3f; // Melee sound volume.

    public bool vault; // Does this weapon have vault animations?
    public string vaultAnim = "Vault"; // Vault animation name.
    public AudioClip vaultSound; // Vault sound.
    public float vaultVolume = 0.3f; // Vault sound volume.

    public bool reload = true; // Does this weapon have reload animations?

    //////////RELOAD MAGS//////////

    public string normalReload = "Reload"; // Normal reload animation name.
    public string completeReload = "CompleteReload"; // Full reload animation name.

    public AudioClip normalReloadSound; // Normal reload sound.
    public AudioClip completeReloadSound; // Complete reload sound.
    public float reloadVolume = 0.5f; // Reload sound volume.

    //////////RELOAD BULLET BY BULLET//////////

    public string startReload = "Start"; // Start reload animation name.
	public string insert = "Insert"; // Insert bullet animation name.
	public string stopReload = "Stop"; // Stop animation name.

	public AudioClip startReloadSound; // Start reload sound.
	public AudioClip insertSound; // Insert bullet sound.
	public AudioClip stopReloadSound; // Stop reload sound.

    //////////MOVE ANIMATIONS//////////

    public GameObject moveAnimations; // Object responsible for playing the walking animations.

    public string runAnimationName = "SwayAnimation"; // Running animation name.
    public float animSpeed = 10; // Transition speed between running and walking animations.

    public Vector3 runningPos; // Weapon position when running.
    public Vector3 runningRot; // Weapon rotation when running.

    private float animScale = 10f; // Animation scale.
    private float yVelocity = 0.0f; // Upwards speed.
    private float angle = 0.0f; // Current radians angle used to calculate animation position.

    public bool isReloading; // Are the player reloading this gun?

    /// <summary>
    /// Returns the time taken to play the vault animation.
    /// </summary>
    public float GetVaultTime ()
    {
        return vault ? weaponAnimation[vaultAnim].length : 0;
    }

    /// <summary>
    /// Returns the time taken to play the melee animation.
    /// </summary>
    public float GetMeleeTime ()
    {
        return melee ? weaponAnimation[meleeAnim].length : 0;
    }

    /// <summary>
    /// Returns the time taken to play the reload animation.
    /// </summary>
    public float GetReloadTime (bool isEmpty)
	{
		if (isEmpty) 
		{
			return reload ? weaponAnimation[completeReload].length : 0;
		}
		else
		{
			return reload ? weaponAnimation[normalReload].length : 0;	
		}
	}

    /// <summary>
    /// Returns the time taken to play the start reload animation.
    /// </summary>
    public float GetStartReloadTime ()
	{
		return reload ? weaponAnimation [startReload].length : 0;
	}

    /// <summary>
    /// Returns the time taken to play the insert bullet animation.
    /// </summary>
    public float GetInsertTime ()
	{
		return reload ? weaponAnimation [insert].length : 0;
	}

    /// <summary>
    /// Returns the time taken to play the stop reload animation.
    /// </summary>
    public float GetStopReloadTime ()
	{
		return reload ? weaponAnimation [stopReload].length : 0;
	}

    /// <summary>
    /// Returns the time taken to play the draw animation.
    /// </summary>
    public float GetDrawTime ()
	{
		return drawHide ? weaponAnimation [drawAnim].length : 0;
	}

    /// <summary>
    /// Returns the time taken to play the hide animation.
    /// </summary>
    public float GetHideTime()
    {
        return drawHide ? weaponAnimation[hideAnim].length : 0;
    }

    /// <summary>
    /// Plays the vault animation.
    /// </summary>
    public void PlayVaultAnimation(bool climb)
    {
        weaponAnimation.Stop(); // Stops all playing animations.

        if (vault)
        {
            weaponAnimation.Play(vaultAnim);
            audioManager.PlayGenericSound(vaultSound, vaultVolume);
        }
    }

    /// <summary>
    /// Plays the melee animation.
    /// </summary>
    public void PlayMeleeAnimation ()
    {
        weaponAnimation.Stop(); // Stops all playing animations.

        if (melee)
        {
            weaponAnimation.Play(meleeAnim);
            audioManager.PlayGenericSound(meleeSound, meleeVolume);
        }
    }

    /// <summary>
    /// Plays the Draw animation.
    /// </summary>
	public void PlayDrawAnimation ()
	{
        weaponAnimation.Stop(); // Stops all playing animations.

        if (drawHide) 
		{
			weaponAnimation.Play (drawAnim);
			audioManager.PlayGenericSound (drawSound, drawVolume);
		}
	}

    /// <summary>
    /// Plays the Hide animation.
    /// </summary>
	public void PlayHideAnimation ()
	{
        weaponAnimation.Stop(); // Stops all playing animations.

        if (drawHide) 
		{
			weaponAnimation.Play (hideAnim);
			audioManager.PlayGenericSound (hideSound, hideVolume);
		}
	}

    /// <summary>
    /// Plays the Reload animation.
    /// </summary>
	public void PlayReloadAnimation (bool isEmpty)
	{
        weaponAnimation.Stop(); // Stops all playing animations.

        if (isEmpty) 
		{
			if (reload) 
			{
				weaponAnimation.Play (completeReload);
				audioManager.PlayReload (completeReloadSound, reloadVolume);
			}
		} 
		else 
		{
			if (reload) 
			{
				weaponAnimation.Play (normalReload);
				audioManager.PlayReload (normalReloadSound, reloadVolume);
			}
		}
	}

    /// <summary>
    /// Plays the start reload animation.
    /// </summary>
	public void PlayStartReload ()
	{
        weaponAnimation.Stop(); // Stops all playing animations.

        if (reload)
		{
			weaponAnimation.Play (startReload);
			audioManager.PlayReload (startReloadSound, reloadVolume);
		}
	}

    /// <summary>
    /// Plays the insert bullet animation.
    /// </summary>
	public void PlayInsert ()
	{
        weaponAnimation.Stop(); // Stops all playing animations.

        if (reload)
		{
			weaponAnimation.Play (insert);
			audioManager.PlayReload (insertSound, reloadVolume);
		}
	}

    /// <summary>
    /// Plays the stop reload animation.
    /// </summary>
	public void PlayStopReload ()
	{
        weaponAnimation.Stop(); // Stops all playing animations.

        if (reload)
		{
			weaponAnimation.Play (stopReload);
			audioManager.PlayReload (stopReloadSound, reloadVolume);
		}
	}

    /// <summary>
    /// Stops all playing animations and stops all sounds from Audio Manager.
    /// </summary>
	public void StopReload ()
	{
        weaponAnimation.Stop(); // Stops all playing animations.
        audioManager.StopReload();
	}

    /// <summary>
    /// Plays the fire animation.
    /// </summary>
	public void PlayShotAnimation (bool aiming)
	{
        weaponAnimation.Stop(); // Stops all playing animations.

        if (aiming) 
		{
			if (fire) 
			{
				weaponAnimation.Play (aimedShot);
				audioManager.PlayShot (shotSound, shotVolume);
			}
		} 
		else 
		{
			if (fire) 
			{
				weaponAnimation.Play (shot);
				audioManager.PlayShot (shotSound, shotVolume);
			}
		}
	}

    /// <summary>
    /// Method responsible for simulate the animation of walking or running procedurally generated.
    /// </summary>
    public void MoveAnimations ()
	{
		if (weapon.controller.moveState == MoveState.Idle) // The player is stopped?
        {
            isIdle(); // Returns the weapon to the starting position and reset the animation to the initial state.
            isRunning(weapon.controller.moveState); // Moves the weapon to the running position/rotation.
        }
		else if (weapon.controller.moveState == MoveState.Walking || weapon.controller.moveState == MoveState.Crouched)
		{
            // Plays the procedurally calculated walking animation.
            isWalking(weapon.controller.moveState == MoveState.Crouched ? weapon.controller.crouchSpeed * 2.0f : weapon.controller.RealWalkingSpeed() * 1.5f);
			isRunning(weapon.controller.moveState); // Moves the weapon to the running position/rotation.
        }
		else if (weapon.controller.moveState == MoveState.Running)
		{
			if (!moveAnimations.GetComponent<Animation> ().enabled)
				moveAnimations.GetComponent<Animation> ().enabled = true;

			if (runAnimationName != null) 
			{
                // Plays the running animation.
                moveAnimations.GetComponent<Animation>()[runAnimationName].speed = Mathf.Clamp(weapon.controller.GetComponent<Rigidbody>().velocity.magnitude /
					(weapon.controller.RealWalkingSpeed() * weapon.controller.runMultiplier) * 1.2f, 0.8f, 1.2f);

				moveAnimations.GetComponent<Animation>().CrossFade(runAnimationName);
			}

			isRunning(weapon.controller.moveState); // Moves the weapon to the running position/rotation.
        }
		else if (weapon.controller.moveState == MoveState.Flying)
		{
			isIdle (); // Returns the weapon to the starting position and reset the animation to the initial state.
            isRunning(weapon.controller.moveState); // Moves the weapon to the running position/rotation.
        }
	}

    /// <summary>
    /// Moves the weapon to the running position/rotation depending on the player's current move state.
    /// Parameters: The current move state.
    /// </summary>
	private void isRunning (MoveState state)
	{
        // Moves the weapon to the running position/rotation.
        if (state == MoveState.Running && !isReloading && !weapon.controller.isClimbing)
		{
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(runningRot), animSpeed * Time.deltaTime);
			transform.localPosition = Vector3.Lerp(transform.localPosition, runningPos, animSpeed * Time.deltaTime);
		}
		else
		{
            // Returns the weapon to the starting position.
            StopRunning();
		}
	}

    /// <summary>
    /// Generates a procedurally motion animation.
    /// Parameters: Animation speed.
    /// </summary>
	private void isWalking (float speed)
	{
        // Disables animations of the object to apply the procedurally calculated animation.
        moveAnimations.GetComponent<Animation>().Stop();
		moveAnimations.GetComponent<Animation> ().enabled = false;

        angle += Time.deltaTime * speed; // Increments the angle over time with the given speed.
        if (angle > (2 * Mathf.PI)) // If the angle is greater than 2π radians, reset to 0.
            angle = 0;

        // Calculates the position that the weapon will be moved in the next frame by multiplying the position by the sine and cosine values of the angle.
        float newPosition = Mathf.SmoothDamp(moveAnimations.transform.localPosition.y, (Mathf.Sin(angle) * Mathf.Cos(angle) * -1) 
			/ (animScale * 5), ref yVelocity, 0.125f);

        // Moves the weapon to the next position previously calculated.
        moveAnimations.transform.localPosition = new Vector3(Mathf.Sin(angle) / (animScale * 10), newPosition, 
			moveAnimations.transform.localPosition.z);
	}

    /// <summary>
    /// Returns the weapon to the starting position and reset the animation to the initial state.
    /// </summary>
	private void StopRunning ()
	{
		moveAnimations.GetComponent<Animation>().Stop();
		moveAnimations.transform.localRotation = Quaternion.Lerp(moveAnimations.transform.localRotation, Quaternion.identity, animSpeed * Time.deltaTime);
		moveAnimations.transform.localPosition = Vector3.Lerp(moveAnimations.transform.localPosition, Vector3.zero, animSpeed * Time.deltaTime);

		if (!weapon.controller.isAiming) // If not aiming.
        {
            ReturnToOrigin(); // Reset the animation to the origin position/rotation.
        }
	}

    /// <summary>
    /// Reset the animation to the origin position/rotation.
    /// </summary>
	public void ReturnToOrigin ()
	{
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, animSpeed * Time.deltaTime);
		transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, animSpeed * Time.deltaTime);
	}

    /// <summary>
    /// Returns the weapon to the starting position and reset the animation to the initial state.
    /// </summary>
	private void isIdle ()
	{
        // Reset the animation to the initial state.
        moveAnimations.transform.localRotation = Quaternion.Lerp(moveAnimations.transform.localRotation, Quaternion.identity, animSpeed * Time.deltaTime);
		moveAnimations.transform.localPosition = Vector3.Lerp(moveAnimations.transform.localPosition, Vector3.zero, animSpeed * Time.deltaTime);

		if (!weapon.controller.isAiming) // If not aiming.
        {
            // Returns the weapon to the starting position
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, animSpeed * Time.deltaTime);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, animSpeed * Time.deltaTime);
		}

		// Reset to a random initial state.
		if(angle != 0 || angle != Mathf.PI)
			angle = Random.Range(0, 100) % 2 == 0 ? 0 : Mathf.PI;
	}
}
