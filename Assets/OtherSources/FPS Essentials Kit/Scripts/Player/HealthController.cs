using UnityEngine;

/// <summary>
/// Class responsible for controlling player's life.
/// </summary>
public class HealthController : MonoBehaviour
{
    [HideInInspector]
    public float CurrentHP; // Current amount of Hp.
    public float maxHP = 100; // Max amount of Hp.

    public bool regenerate; // Will Hp regenerate over time?
    public float regenerationSpeed = 5; // Regeneration rate.

    public float delayToRegenerate = 5; // Delay to start regenerating.
    private float nextRegenerationTime = 0;

    [Space()]
    //public AudioClip[] hitSounds;
    public AudioClip heartBeat; // When life gets too low, it plays the sound of heartbeats.

    [Header("Fall Damage")]
    public float heightThreshold = 5.0f;  // The max height that player can fall without hurting yourself.
    public float damageMultiplier = 4.0f;  // The damage multiplier increases the damage in the fall, to make it more realistic.

    public AudioManager audioManager; // The audio manager.
    public PlayerUI ui; // The player UI.
    public CameraAnimations cameraAnim;
    public CameraEffects cameraEffects;

    private float higherHeight = 0.0f;  // Distance to the ground of the last fall.

    private MoveController mController; // The player.
    private CapsuleCollider capsuleCol; // The player collider.

    // Use this for initialization
    private void Start ()
    {
        mController = GetComponent<MoveController>();
        capsuleCol = GetComponent<CapsuleCollider>();

        // Player starts with max hit points.
        CurrentHP = maxHP;
    }
	
	// Update is called once per frame
	private void Update ()
    {
        CheckFalling(); // Checks if player is falling.

        CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHP); // Prevents the amount of hp from being less than 0 or greater than the maximum of HP.

        audioManager.isDying = CurrentHP < maxHP * 0.35f; // If the current life is less than 30% of the total, enables the low life effect on the audio manager.

        // If the current life is less than 30% of the total, it starts to play the sound of heartbeat.
        if (CurrentHP < maxHP * 0.3f)
            audioManager.PlayHeartbeatSound(heartBeat, CurrentHP, maxHP * 0.3f);

        // Regenerates Hp
        if (regenerate && CurrentHP < maxHP && CurrentHP > 0 && nextRegenerationTime < Time.time)
            CurrentHP += Time.deltaTime * regenerationSpeed;
    }

    /// <summary>
    /// Applies the damage of an explosion to the player.
    /// Parameters: The explosion damage, the explosion intensity and direction.
    /// </summary>
    public void ExplosionDamage (float damage, float intensity, Vector3 dir)
    {
        ApplyDamage(damage);
        ui.ShowDamageIndicator(dir);
        cameraEffects.ExplosionEffect(intensity);
        cameraAnim.ExplosionShakeCamera(100 * intensity, intensity, 3);
    }

    /// <summary>
    /// Applies the damage of a shot on the player.
    /// Parameters: Shot damage and direction.
    /// </summary>
    public void BulletDamage (float damage, Vector3 dir)
    {
        ApplyDamage(damage);
        ui.ShowDamageIndicator(dir);
        cameraAnim.HitShake();
    }

    /// <summary>
    /// Applies damage from melee attack.
    /// Parameters: Melee attack damage and direction.
    /// </summary>
    public void MeleeDamage (float damage, Vector3 dir)
    {
        ApplyDamage(damage);
        ui.ShowDamageIndicator(dir);
        cameraAnim.HitShake();
    }

    /// <summary>
    /// Apply fall damage.
    /// Parameters: The fall damage.
    /// </summary>
    public void FallDamage (float damage)
    {
        ApplyDamage(damage);
        ui.ShowCircularIndicator();
    }

    /// <summary>
    /// Applies damage to the player.
    /// Parameters: The amount of Hp the player will lose.
    /// </summary>
    private void ApplyDamage (float damage)
    {
        if (CurrentHP > 0 && damage > 0)
        {
            CurrentHP -= damage;
            ui.ShowHitScreen();
        }
        nextRegenerationTime = Time.time + delayToRegenerate;
    }

    /// <summary>
    /// Checks if player is falling.
    /// </summary>
    private void CheckFalling ()
    {
        // If the player is touching the ground.
        if (mController.Grounded && higherHeight > 0.0f)
        {
            // If the height of the drop is greater than the limit, Applies damage to the player.
            if (higherHeight > heightThreshold)
            {
                FallDamage(Mathf.Round(damageMultiplier * -Physics.gravity.y * (higherHeight - heightThreshold)));
				cameraAnim.FallShake ();
            }
            else if (higherHeight >= mController.jumpForce / 6.2f)
            {
                // Shakes the camera when hit the ground.
				if (cameraAnim != null)
					cameraAnim.FallShake ();
            }

            // Reset the value to be able to calculate a new fall.
            higherHeight = 0.0f;
        }
        else if (!mController.Grounded)
        {
            // Calculates the distance to the ground and takes the longest distance (the highest point that you have fallen).
            if (CheckDistanceBelow() > higherHeight)
                higherHeight = CheckDistanceBelow();
        }
    }

    /// <summary>
    /// Calculates the distance from the current position to a surface near the bottom of the player.
    /// </summary>
    public float CheckDistanceBelow()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.SphereCast(transform.position, capsuleCol.radius, Vector3.down, out hit, heightThreshold * 10))
        {
            return hit.distance;
        }
        else
        {
            // If the distance is too big, returns a big value that when the player hit the ground will die instantly.
            return heightThreshold * 10;
        }
    }
}
