using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

/// <summary>
/// Shooting styles.
/// </summary>
public enum FireMode
{
	Auto,
	Semi,
	Burst,
	ShotgunSemi,
	ShotgunAuto
}

/// <summary>
/// Reloading styles.
/// </summary>
public enum ReloadStyle
{
	BulletByBullet,
	Magazines
}

/// <summary>
/// How the damage will be applied? It will be affected by distance or remain constant?
/// </summary>
public enum DamageMode
{
	DecreaseByDistance,
	Constant
}

/// <summary>
/// Base class of all weapons in the game.
/// </summary>
[RequireComponent(typeof(WeaponsAnimations))]
public class Weapon : MonoBehaviour
{
	public string weaponName = ""; // The weapon name.
    public Texture2D weaponIcon; // The weapon icon.
    public int weaponId; // The weapon id must be unique to each weapon.
    public GameObject droppablePrefab; // Droppable Prefab is the prefab that will be instantiated when you switch of weapon.
    public float weight; // The weapon weight.

    public bool melee; // Can melee?

    public CrosshairStyle crosshairStyle = CrosshairStyle.None;
    public FireMode fireMode = FireMode.Auto; // How your weapon will Fire.
    public ReloadStyle reloadStyle = ReloadStyle.Magazines; // // Weapon reload style.
    public DamageMode damageMode = DamageMode.Constant; // The weapon lose power with distance or remains constant?

    public LayerMask cullingMask = 1;  // The layers that will be affected by this weapon.

    public float fireRate = 0.5f; // The interval in seconds between each shot.
    public float shotForce = 50;  // The force applied by each shot.
    public float range = 100; // The maximum distance that your weapon can hit.

    
    public int pelletCount = 6; // How many bullets will be fired at a time (Shotgun only).

    public int burstCount = 3; // How many bullets per burst.

    public bool infiniteAmmo;

    public int magazineSize; // The number of bullets per magazine.
    public int numberOfMags; // The number of magazines.
    public int maxNumberOfMags; // Maximum number of magazines.

    private int magsRemaining; // Number of bullets remaining.
    private int currentAmmo; // Current number of bullets on current magazine.

    public float minDamage = 15; // Minimum damage caused by this weapon.
    public float maxDamage = 35; // Maximum damage caused by this weapon.

     
    public float baseSpread = 4; // Base spread.
    public float maxSpread = 6; // Maximum spread.

    public float baseSpreadCrouch = 3; // Base spread when crouched.

    
    public bool aimDownSights = true; // Can Aim Down Sights?

    public Vector3 aimPosition;
	public Vector3 aimRotation;

	public bool zoom = true; // Increase the camera FOV when aim?
    public int zoomFov = 50; // The FOV value when aim.
    public float aimSpeed = 10; // Camera FOV increase speed.
    public float spreadAIM = 0.4f; // Spread when aiming.

    public bool scope = false; // Weapon has riflescope?
    public RifleScope rifleScope; // RifleScope component.
    public int rifleScopeFOV = 3; // RifleScope field of view.

    public float spreadWhenMove = 2;  // Increase spread amount when moving.
    public float spreadPerShot = 2; // Increase spread amount when firing.

    private float spread; // The current value of spread.

    private float nextFireTime = 0; // Time until the next shooting (in milliseconds).
    private float nextReloadTime = 0; // Time until the next reload (in milliseconds).
    private float nextVaultTime = 0; // Time to be ready for a new jump (in milliseconds).

    private float firingTime = 0; // Time required to stabilize the weapon.

    private bool fighting; // The player is attacking?

    private bool reloading; // The weapon is reloading?

	private bool weaponActive; // The weapon is selected?
	private bool aiming; // The player is aiming?

	public bool recoil = true; // Has recoil?
	public float recoilIntensity = 0.5f; // Vertical Recoil intensity.
    public float recoilAmountAiming = 0.5f; // Vertical Recoil intensity (Aiming).

    public bool limitUpwardsRecoil; // Limit vertical recoil?
    public float maxUpwardsRecoil = 3; // Max Vertical Recoil intensity.

    public float minSidewaysRecoil = -0.5f; // Min Sideways recoil.
    public float maxSidewaysRecoil = 0.5f; // Max Sideways recoil.

    public bool weaponKickBack = false; // Apply the recoil on the weapon?

	public float weaponKick = 0.5f; // Recoil intensity.

    public float recoilTimer = 0.1f; // Recoil speed.

	public CameraAnimations cameraAnimations; // Object responsible for managing all the animations of the camera

    public MoveController controller; // Player.
    public Camera mainCamera;

	public PlayerUI UI; // User Interface.

    public BulletMarkManager bulletMark;

	public WeaponsManager weaponManager; // Weapons manager.

    public bool muzzleFlash = false; // Has Muzzle flash?
    public GameObject muzzle; // Muzzle flash Game Object.

    public bool smoke = false; // Emits smoke?
    public GameObject smokePrefab;
    public Transform smokeTransform; // Where the smoke will be instantiated?

    public bool tracer = false; // Has bullet tracer
    public GameObject tracerPrefab;
	public Transform tracerTransform; // Where the tracer will be instantiated?
    public float tracerSpeed = 200;

    public bool shell = false; // Has bullet shell
    public GameObject shellPrefab;
    public Transform shellTransform;  // Where the shell will be instantiated?
    public float delayToInstantiate = 0; // Delay to instantiate (in seconds).

    public float minEjectForce = 2; // Minimum force applied to the bullet when instantiated.
    public float maxEjectForce = 5; // Maximum force applied to the bullet when instantiated.

    /// <summary>
    /// The weapon is out of ammo?
    /// </summary>
    public bool OutOfAmmo
    {
        get
        {
            return currentAmmo == 0 && magsRemaining == 0;
        }
    }

    /// <summary>
    /// Weapon animation component attached to this gameObject.
    /// </summary>
	private WeaponsAnimations weaponAnim
    {
		get
		{
			return GetComponent<WeaponsAnimations> (); // Get the component attached in the gameObject.
        }
	}

    // Use this for initialization.
    private void Start ()
	{
        if (scope)
        {
            rifleScope.SetRifleScopeFOV(rifleScopeFOV); // Set the rifle scope camera fov.
        }

		currentAmmo = magazineSize; // Starts current ammo with the number of bullets per magazine.
        magsRemaining = magazineSize * numberOfMags; // Number of bullets = amount of bullets per magazine * number of magazines.
    }

    // Update is called once per frame
    private void Update ()
	{
		if (weaponActive) // The weapon is selected?
		{
			if (weaponAnim != null)
			{
                // Sets the player is reloading to disable the running animation.
                weaponAnim.isReloading = reloading || nextReloadTime > Time.time;
			}

            GetUserInput(); // Checks if the user is pressing some action key.

            // Can use items (grenade)?
            weaponManager.canUseItems = nextReloadTime < Time.time && !reloading && !aiming 
                && controller.moveState != MoveState.Running && !fighting && !controller.isClimbing && nextVaultTime < Time.time;

            if (UI != null)
				UpdateUI (); // Update the UI.

            if (aimDownSights) // Can Aim Down Sights?
                AimDownSights();

            controller.canVault = nextReloadTime < Time.time && !reloading && !aiming && !fighting; // Can play the vault animation?

            cameraAnimations.isFiring = firingTime > Time.time; //The player is shooting?
            controller.isAiming = aiming; // Defines the aiming state of the player so that it moves slower and lower the the intensity of the mouse input.
        } 
	}

    // This function is called every fixed framerate frame. It is used to play procedural animations.
    private void FixedUpdate ()
    {
        if (weaponActive) // The weapon is selected?
        {
            if (weaponAnim != null)
            {
                weaponAnim.MoveAnimations(); // Play motion animation, like walk or run.
            }
            
            UpdateSpread(); // Updates the weapon accuracy.
        }
        else if (!weaponManager.canSwitch) // It's changing the weapon?
        {
            weaponAnim.ReturnToOrigin(); // Return to the original position.
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * aimSpeed); // Return camera FOV to the original value.
        }
    }

    /// <summary>
    /// Changes the position of the gameObject for the aiming position.
    /// </summary>
    private void AimDownSights ()
	{
		if (aiming) // Is aiming?
		{
            if (scope && !rifleScope.isAiming) // Activates the rifle scope to see over long distances
                rifleScope.isAiming = true;


            // Set the current position and rotation of gameObject to the aiming position and rotation. 
            transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (aimRotation), Time.deltaTime * aimSpeed);
			transform.localPosition = Vector3.Lerp (transform.localPosition, aimPosition, Time.deltaTime * aimSpeed);

			if (zoom) 
			{
				mainCamera.fieldOfView = Mathf.Lerp (mainCamera.fieldOfView, zoomFov, Time.deltaTime * aimSpeed); // Set camera FOV as zoom FOV.
			}
		} 
		else 
		{
			if (scope && rifleScope.isAiming) // Disable the rifle scope.
				rifleScope.isAiming = false;
			
			if (zoom) 
			{
                mainCamera.fieldOfView = Mathf.Lerp (mainCamera.fieldOfView, 60, Time.deltaTime * aimSpeed); // Return camera FOV to the original value.
            }
		}
	}

    /// <summary>
    /// Sets the spread value according to the player or weapon state (Walking, Running, Firing...).
    /// </summary>
	private void UpdateSpread ()
	{
        spread = Mathf.Clamp (spread, spreadAIM, maxSpread); // Limits the spread value to never be higher than maxSpread and lower than spreadAIM.

        if (controller.moveState != MoveState.Idle || firingTime > Time.time) // The player is not idle or are shooting?
        {
			if (aiming) // Is aiming?
            {
                if (controller.isCrouched) // The player is crouched?
                {
                    if (spread != baseSpreadCrouch) 
                        spread = Mathf.Lerp(spread, baseSpreadCrouch, 
							(firingTime > Time.time ? spreadPerShot : spreadWhenMove) * Time.deltaTime); // Changes the spread value to baseSpreadCrouch.
                } 
				else 
				{
                    if (spread != baseSpread) 
                        spread = Mathf.Lerp(spread, baseSpread, 
							(firingTime > Time.time ? spreadPerShot : spreadWhenMove) * Time.deltaTime); // Changes the spread value to baseSpread.
                }
			}
			else
			{
                if (spread < maxSpread) 
                    spread += (firingTime > Time.time ? spreadPerShot : spreadWhenMove) * Time.deltaTime; // Changes the spread value to maxSpread.
            }
		}
        else
        {
			if (aiming) 
			{
				if (spread != spreadAIM)  
                    spread = Mathf.Lerp(spread, spreadAIM, spreadWhenMove * Time.deltaTime); // Changes the spread value to spreadAIM.
            } 
			else
			{
				if (controller.isCrouched && baseSpreadCrouch != baseSpread) 
				{
					if (spread != baseSpreadCrouch) 
                        spread = Mathf.Lerp(spread, baseSpreadCrouch, spreadWhenMove * Time.deltaTime); // Changes the spread value to baseSpreadCrouch.
                } 
				else 
				{
					if (spread != baseSpread) 
						spread = Mathf.Lerp(spread, baseSpread, spreadWhenMove * Time.deltaTime); // Returns the spread value to the default.
                }
			}
		}
	}

    /// <summary>
    /// Checks whether the user is pressing any action key and invokes the corresponding method.
    /// Action keys: R = (Reload), Mouse1 = (Aim), F = (Melee Attack).
    /// </summary>
	private void GetUserInput ()
	{
        // Restrictions to reload.
        bool canReload = nextReloadTime < Time.time && !reloading && currentAmmo < magazineSize && magsRemaining > 0 
            && !fighting && !controller.isClimbing && nextVaultTime < Time.time;

		if (canReload) // If there is no restriction to reload.
        {
			if (Input.GetKeyDown (KeyCode.R))
				Reload ();
		}

        // Restrictions to shoot.
        bool canFire = nextFireTime < Time.time && nextReloadTime < Time.time && currentAmmo >= 0 && !reloading 
            && controller.moveState != MoveState.Running && !fighting && !controller.isClimbing && nextVaultTime < Time.time;

		if (canFire) // If there is no restriction to shoot.
        {
			if (isFiring()) // Is requesting to shoot?
            {
                if (currentAmmo == 0 && magsRemaining > 0) // If the magazine is empty and have bullets to reload.
                {
				    Reload ();
			    } 
			    else if (currentAmmo > 0) // If the player have bullets available to shoot.
                {
				    Shot ();
			    }
                else if (currentAmmo == 0 && magsRemaining == 0) // Out Of Ammo.
                {
                    weaponManager.OutOfAmmo();
                }
			} 
		}
		else
		{
            if (reloading && reloadStyle == ReloadStyle.BulletByBullet && controller.moveState != MoveState.Running  && currentAmmo > 0)
			{
				if (Input.GetKeyDown (KeyCode.Mouse0)) // Stops Bullet By Bullet reload.
                {
					StopCoroutine(ReloadBulletByBullet ());
					StartCoroutine(StopReloadBulletByBullet());
				}
			}
		}

        // Restrictions to aim.
        bool canAim = !reloading && aimDownSights && !fighting && !controller.isClimbing && nextVaultTime < Time.time;

		if (canAim) // If there is no restriction to aim.
        {
			if (Input.GetKeyDown (KeyCode.Mouse1) && !aiming)
			{
				aiming = true;
			} 
			else if (Input.GetKeyDown (KeyCode.Mouse1) && aiming) 
			{
				aiming = false;
			}
		} 
		else 
		{
			aiming = false;
		}

        // Restrictions to do melee attack.
        bool canMelee = melee && !reloading && !fighting && nextReloadTime < Time.time && controller.moveState != MoveState.Running 
            && !controller.isClimbing && nextVaultTime < Time.time;

        if (canMelee) // If there is no restriction to do melee attack.
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(MeleeAttack());
            }
        }
    }

    /// <summary>
    /// Return true if the user are pressing to shoot.
    /// Action keys:  Mouse0 = (Shot).
    /// </summary>
    private bool isFiring()
    {
        if (fireMode == FireMode.Auto || fireMode == FireMode.ShotgunAuto)
        {
            if (Input.GetKey(KeyCode.Mouse0)) // If the user hold the key to shoot.
            {
                return true;
            }
        }
        else if (fireMode == FireMode.Semi || fireMode == FireMode.ShotgunSemi || fireMode == FireMode.Burst)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) // If the user press the key to shoot.
            {
                return true;
            }
        }

        return false; // If the user does not press anything in the current frame, returns false.
    }

    /// <summary>
    /// Select the type of firing of the weapon based on your fire mode.
    /// </summary>
	private void Shot ()
	{
		if (fireMode == FireMode.Auto || fireMode == FireMode.Semi) // If the shooting style is single or automatic.
        {
			NormalShot ();
		}
		else if (fireMode == FireMode.ShotgunAuto || fireMode == FireMode.ShotgunSemi) // If the shooting style is shotgun.
        {
			ShotgunShot ();
		}
		else if (fireMode == FireMode.Burst) // If the shooting style is bursty
        {
			BurstShot ();
		}
	}

    /// <summary>
    /// Fires a single shot, plays animations and applies the effects of each shot.
    /// </summary>
	private void NormalShot ()
	{
        nextFireTime = fireRate + Time.time; // Calculate the time until the next shooting.
        firingTime = 0.1f + Time.time; // Calculates the time to stabilize the weapon.

        currentAmmo--; // Remove a bullet from current magazine.

		FireOneShot (); // Fires a bullet based on the current position and direction.

        StartCoroutine(FireEffects()); // Apply the shot effects, like muzzle flash, smoke, tracer.
        weaponAnim.PlayShotAnimation(aiming); // Play shot animation.

        PlayRecoil(); // Apply recoil.
    }

    /// <summary>
    /// Fires several shots at once, plays animations and applies the effects of each shot.
    /// </summary>
	private void ShotgunShot ()
	{
        nextFireTime = fireRate + Time.time; // Calculate the time until the next shooting.
        firingTime = 0.1f + Time.time; // Calculates the time to stabilize the weapon.

        currentAmmo--; // Remove a bullet from current magazine.

        for (int i = 0; i < pelletCount; i++) // Fires <pelletCount> shots at once.
        {
            FireOneShot(); // Fires a bullet based on the current position and direction.
        }

        StartCoroutine(FireEffects ()); // Apply the shot effects, like muzzle flash, smoke, tracer.
        weaponAnim.PlayShotAnimation(aiming); // Play shot animation.

        PlayRecoil(); // Apply recoil.
    }

    /// <summary>
    /// Invokes the method that fires a burst of bullets.
    /// </summary>
	private void BurstShot ()
	{
        StartCoroutine(Burst()); // Fires a burst of bullets.
        nextFireTime = fireRate + Time.time; // Calculate the time until the next shooting.
    }

    /// <summary>
    /// Fires a burst of bullets, plays animations and applies the effects of each shot.
    /// </summary>
	private IEnumerator Burst ()
	{
		for (int i = 0; i < burstCount; i++) // Fires a burst of bullets.
        {
            FireOneShot(); // Fires a bullet based on the current position and direction.

            firingTime = 0.1f + Time.time; // Calculates the time to stabilize the weapon.
            currentAmmo--; // Remove a bullet from current magazine.

            StartCoroutine(FireEffects()); // Apply the shot effects, like muzzle flash, smoke, tracer.
            weaponAnim.PlayShotAnimation(aiming); // Play shot animation.

            PlayRecoil(); // Apply recoil.

            yield return new WaitForSeconds(fireRate); // Waits for the interval between shots to continue the sequence of shots.
        }
	}

    /// <summary>
    /// Invokes the camera's method to apply recoil in itself and in the weapon.
    /// </summary>
    private void PlayRecoil ()
    {
        if (recoil) // Can apply recoil?
        {
            // Invokes the camera's method.
            cameraAnimations.PlayRecoil(aiming, recoilIntensity, recoilAmountAiming, minSidewaysRecoil, maxSidewaysRecoil, recoilTimer / 10, weaponKickBack, weaponKick, limitUpwardsRecoil, maxUpwardsRecoil);
        }
    }

    /// <summary>
    /// Active or instantiates each effect defined in the weapon.
    /// </summary>
	private IEnumerator FireEffects ()
	{
		if (muzzleFlash) // Has muzzle flash?
		{
		    muzzle.SetActive(true); // Active the muzzle flash.

            muzzle.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))); // Sets a random rotation.
            yield return new WaitForSeconds(0.03f); // Awaits the 3 millisecond interval.

            muzzle.SetActive(false); // Disable the muzzle flash.
        }

		if (smoke) // Has smoke?
		{
            // Instantiates smoke prefab and destroy after 2 seconds.
            GameObject s = Instantiate (smokePrefab, smokeTransform.position, smokeTransform.rotation) as GameObject;
			Destroy(s, 2);
		}

        if (shell) // Has bullet shell?
        {
            if (shellPrefab != null && shellTransform != null)
            {
                yield return new WaitForSeconds(delayToInstantiate); // Delay to instantiate.

                // Instantiates a bullet shell.
                GameObject shellClone = Instantiate(shellPrefab, shellTransform.position, shellTransform.rotation) as GameObject;

                // Applies a force to launch the bullet shell.
                shellClone.GetComponent<Rigidbody>().velocity = shellClone.transform.forward * Random.Range(minEjectForce, maxEjectForce);

                // Destroy after 1 second.
                Destroy(shellClone, 1);
            }
        }
	}

	/// <summary>
	/// Instantiates a bullet trail according to the direction of each bullet fired.
	/// Parameters: The bullet direction and the tracer life time.
	/// </summary>
	private void Tracer (Vector3 direction, float tracerLifeTime)
	{
		if (tracer) // Has tracer?
		{
            // Instantiates the tracer.
            GameObject t = Instantiate(tracerPrefab, tracerTransform.position, tracerTransform.parent.rotation) as GameObject;
			t.GetComponent<Rigidbody>().velocity = direction * tracerSpeed; // Applies a force to follow the bullet's trajectory.
            Destroy(t, tracerLifeTime); // Destroys after a defined time.
        }
	}

    /// <summary>
    /// A melee attack is an attack that is short-range. It is a secondary attack.
    /// Action key: F = (attack).
    /// </summary>
    private IEnumerator MeleeAttack ()
    {
        fighting = true; // Activates the attacking state.

        weaponAnim.PlayMeleeAnimation(); // Play melee attack animation.

        yield return new WaitForSeconds(0.1f); // Wait 0.1 seconds before applying the damage or force.

        Vector3 direction = mainCamera.transform.TransformDirection(Vector3.forward); // The direction that the player is looking.
        Vector3 origin = mainCamera.transform.position; // Weapon position.

        Ray ray = new Ray(origin, direction); // Creates a ray starting at origin along direction.
        RaycastHit hitInfo; // Structure used to get information back from a raycast.
        
        if (Physics.Raycast(ray, out hitInfo, 2)) // Checks whether the ray intersects something.
        {
            if (hitInfo.collider.GetComponent<Rigidbody>() != null) // If hit a rigidbody applies force to push.
            {
                hitInfo.collider.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(weaponAnim.GetMeleeTime() - 0.1f); // Awaits the animation finish to disable the attack state.
        fighting = false;
    }

    /// <summary>
    /// Select the type of reload of the weapon based on your reload style.
    /// </summary>
    private void Reload ()
	{
		if (reloadStyle == ReloadStyle.Magazines) // If the reload style is by magazines.
        {
			StartCoroutine(ReloadMagazines());
		}
		else if (reloadStyle == ReloadStyle.BulletByBullet) // If the reload style is bullet by bullet.
        {
			StartCoroutine(ReloadBulletByBullet());
		}
	}

    /// <summary>
    /// Reloads the gun by adding a bullet at a time.
    /// </summary>
	private IEnumerator ReloadBulletByBullet ()
	{
		reloading = true; // Activates the reload state.

        weaponAnim.PlayStartReload(); // Play reload animation.
        yield return new WaitForSeconds(weaponAnim.GetStartReloadTime()); // Wait until the start reload animation finish.

        // While the amount of bullets is less than the magazine size and have bullets to reload.
        while ((currentAmmo < magazineSize && magsRemaining > 0) && reloading)
        {
			weaponAnim.PlayInsert(); // Play insert bullet animation.
            yield return new WaitForSeconds(weaponAnim.GetInsertTime() / 2);

			if (reloading) // If is reloading.
            {
				currentAmmo++; // Add a bullet to the weapons.
				if (!infiniteAmmo) magsRemaining--; // Remove a bullet from magazines.
            }

			yield return new WaitForSeconds(weaponAnim.GetInsertTime() / 2);
		}
		
		if (reloading) // If is reloading.
        {
            // Stop reloading and play stop reload animation.
			StartCoroutine(StopReloadBulletByBullet());
			StopCoroutine(ReloadBulletByBullet ());
		}
	}

    /// <summary>
    /// Plays stop reloading animation.
    /// </summary>
	private IEnumerator StopReloadBulletByBullet ()
	{
		nextReloadTime = weaponAnim.GetStopReloadTime() + Time.time; // Calculate the time until the next reload.
        reloading = false; // Disable the reload state.

        weaponAnim.PlayStopReload(); // Play stop reload animation.
        yield return new WaitForSeconds(weaponAnim.GetStopReloadTime());
	}

    /// <summary>
    /// Change the current magazine by a new.
    /// </summary>
	private IEnumerator ReloadMagazines ()
	{
		reloading = true; // Activates the reload state.

        weaponAnim.PlayReloadAnimation (currentAmmo == 0); // Play reload animation based on current amount of bullets.

        yield return new WaitForSeconds(weaponAnim.GetReloadTime(currentAmmo == 0));  // Wait until the reload animation finish.

        if (weaponActive) // The weapon is selected?
        {
			if (!infiniteAmmo) // If do not have infinite bullets.
            {
				if (currentAmmo + magsRemaining > magazineSize) // Have enough bullets for a full magazine?
                {
					magsRemaining -= magazineSize - currentAmmo; // Calculate the bullet number that can be removed from the magazine.
                    currentAmmo = magazineSize;
				} 
				else 
				{
					currentAmmo += magsRemaining; // If do not have enough bullets to fill the magazine, just sum the remaining amount in the current.
                    magsRemaining = 0; // Set as 0 the number of bullets available.
                }
			}
			else
			{
				currentAmmo = magazineSize; // If have infinite bullets, just fill the magazine.
            }
		}
		reloading = false; // Disable the reload state.
    }

    /// <summary>
    /// Updates the weapon information on the UI.
    /// </summary>
	private void UpdateUI ()
	{
        UI.SetWeaponIcon (weaponIcon); // Sets the weapon icon.
        UI.ShowCrosshair (!aiming); // Displays the crosshairs if not aiming.
        UI.SetCrosshairType (crosshairStyle); // Sets the crosshair style.
        UI.SetCrosshairColor (TargetInfo ()); // Sets the crosshair color.
        UI.SetSpread (spread / 10); // Update current spread.

        // If is not reloading, displays the amount of bullets.
        if (!reloading || reloadStyle == ReloadStyle.BulletByBullet)
			UI.SetWeaponProperties (currentAmmo, magazineSize, infiniteAmmo ? 999 : magsRemaining);
	}

    /// <summary>
    /// Calculates where each bullet will hit, instantiates a bullet mark on the surface that the bullet hits.
    /// </summary>
	private void FireOneShot ()
	{
		Vector3 direction = mainCamera.transform.TransformDirection(ShotDirection ()); // Bullet direction.
		Vector3 origin = mainCamera.transform.position; // Bullet origin.

		Ray ray = new Ray (origin, direction); // Creates a ray starting at origin along direction.
        RaycastHit hitInfo; // Structure used to get information back from a raycast.
		
		float tracerLifeTime = 2; // Initial duration of each bullet tracer.

        if (Physics.Raycast(ray, out hitInfo, range, cullingMask)) // Checks whether the ray intersects something.
        {
            // Get the surface type of the object.
            SurfaceType surface = hitInfo.collider.GetComponent<Surface>() != null 
                ? hitInfo.collider.GetComponent<Surface>().GetSurface(hitInfo.point, hitInfo.collider.gameObject) : SurfaceType.Default;

            // Instantiates a bullet mark on the position.
            bulletMark.Instantiate(surface, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal), hitInfo.transform);

            if (hitInfo.collider.GetComponent<Rigidbody>() != null) // If hit a rigidbody applies force to push.
            {
                hitInfo.collider.GetComponent<Rigidbody>().AddForce (direction * shotForce, ForceMode.Impulse);
			}

            // Calculates the tracer lifetime according to the distance traveled by each bullet.
            tracerLifeTime = hitInfo.distance / tracerSpeed;
		}

        // If the tracer lifetime is greater than 0.1 sec
        if (tracerLifeTime > 0.1f)
			Tracer(direction, tracerLifeTime);
	}

    /// <summary>
    /// Returns the damage value of each bullet.
    /// Parameters: The distance traveled by each bullet.
    /// </summary>
    private float GetWeaponDamage (float distance)
    {
        // Calculates damage randomly between the minimum and the maximum value based on the distance.
        return damageMode == DamageMode.Constant ? Random.Range(minDamage, maxDamage) : Random.Range(minDamage, maxDamage) * (1 - (distance / range));
    }

    /// <summary>
    /// Returns the direction of each bullet.
    /// </summary>
	private Vector3 ShotDirection ()
	{
		float x = Random.Range (-0.01f, 0.01f) * spread / 1.5f; // Horizontal variation
        float y = Random.Range (-0.01f, 0.01f) * spread / 1.5f; // Vertical variation
		return new Vector3 (x, y, 1); // Returns a vector with random values based on the current spread.
    }

    /// <summary>
    /// Returns a different color if the weapon is pointed to a object with a specific tag.
    /// Colors: White = Default, Red = Enemy, Black = Explosive.
    /// </summary>
	private Color TargetInfo ()
	{
		Vector3 direction = mainCamera.transform.TransformDirection(Vector3.forward); // 
		Vector3 origin = mainCamera.transform.position; // Weapon postion.

        Ray ray = new Ray (origin, direction); // Creates a ray starting at origin along direction.
        RaycastHit hitInfo; // Structure used to get information back from a raycast.

        if (Physics.Raycast (ray, out hitInfo, range, cullingMask)) // Checks whether the ray intersects something.
        {
			if (hitInfo.collider.tag == "Enemy") // If aim at an enemy.
                return Color.red;
			if (hitInfo.collider.tag == "Explosive") // If aim at an explosive.
                return Color.black;
			else // Default = white.
				return Color.white;
		} 
		else 
		{
			return Color.white; // If not aiming at anything, returns the default color.
        }
	}

    /// <summary>
    /// Returns the time taken to run the Hide animation.
    /// </summary>
	public float GetHideTime ()
	{
		return weaponAnim.GetHideTime ();
	}

    /// <summary>
    /// Invokes the method to select the weapon.
    /// </summary>
	public void Select ()
	{
		StartCoroutine (DrawWeapon ());
	}

    /// <summary>
    /// Select the gun and enables the renders to show the weapon.
    /// </summary>
    private IEnumerator DrawWeapon ()
	{
		DisableRenders (false); // Enables the weapon renders.
        weaponAnim.PlayDrawAnimation (); // Play draw animation.
		yield return new WaitForSeconds(weaponAnim.GetDrawTime()); // Wait for the draw animation finish.
        weaponManager.ready = true; // Ready to change weapon again.
        weaponActive = true; // Enable the gun for use.
    }

    /// <summary>
    /// Disable the weapon instantly.
    /// </summary>
	public void FastDeselect()
	{
        UI.ShowCrosshair(false); // Hide the crosshair.

        // Hold in fire position.
        aiming = false;
        controller.isAiming = false;
        controller.canVault = false;

        // Stop all weapon functions.
        cameraAnimations.isFiring = false;
        reloading = false;
        StopCoroutine(ReloadMagazines());
        weaponActive = false;

        weaponAnim.StopReload();
        weaponManager.ready = false;
        controller.canVault = false;
        DisableRenders(true); // Disable the weapon renders.
    }

    /// <summary>
    /// Deselects the weapon and disables their functions.
    /// </summary>
	public void Deselect ()
	{
		UI.ShowCrosshair (false); // Hide the crosshair.

        // Hold in fire position.
        aiming = false;
		controller.isAiming = false;
        controller.canVault = false;

        // Stop all weapon functions.
        cameraAnimations.isFiring = false;
		reloading = false;
		weaponActive = false;
		StartCoroutine (DeselectWeapon ()); // Invoke the method that will disable the weapon and renders.
    }

    /// <summary>
    /// Disables all functions and plays hide the gun animation.
    /// </summary>
	private IEnumerator DeselectWeapon ()
	{
        // Stop all weapon functions.
        weaponAnim.StopReload();
		StopCoroutine(ReloadMagazines());

		weaponAnim.PlayHideAnimation (); // Play hide animation.
		yield return new WaitForSeconds(weaponAnim.GetHideTime()); // Wait until hide animation finish.
        weaponManager.ready = false; // Weapon Manager is not ready to change weapon.
        DisableRenders (true); // Disable the weapon renders.
    }

    /// <summary>
    /// Enable or Disable all object renders including children.
    /// Parameters: The renderer state (true or false).
    /// </summary>
    public void DisableRenders (bool state)
	{
        // For each object that has a renderer inside the weapon gameObject.
        foreach (Renderer m in GetComponentsInChildren<Renderer>())
		{
			m.enabled = !state; // Enable or disable the renderer.
            m.shadowCastingMode = ShadowCastingMode.Off; // Prevents the weapon produces shadows to avoid ghosts weapons bugs.
        }
	}

    /// <summary>
    /// Returns the time taken to play the Vault animation.
    /// </summary>
    public float GetVaultTime ()
    {
        return weaponAnim.GetVaultTime();
    }

    /// <summary>
    /// Play vault animation.
    /// </summary>
    public void Vault (bool climb)
    {
        nextVaultTime = Time.time + weaponAnim.GetVaultTime(); // Calculate the time until the next jump.
        weaponAnim.PlayVaultAnimation(climb); // Play vault animation.
    }
}