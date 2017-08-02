using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for managing the items
/// </summary>
public class Items : MonoBehaviour
{
    [Header("Grenades")]
    public GameObject grenade; // The grenade prefab.

    public Transform throwPos; // Position where grenade will be instantiated.

    public float throwForce = 10; // How far the player can throw the grenade.

    public bool infiniteGrenades;

    public int numberOfGrenades = 3;
    public int maxNumberOfGrenades = 3;

    public float maxForceMultiplier = 3; // The maximum force multiplier.
    private float forceMultiplier = 0; // Throw force multiplier.

    public float delayToThrow = 0.3f; // Delay until instantiate the grenade.

    [Header("Animations")]
    public Animation grenadeAnim;

    [Space()]
    public string pullAnimName = "";
    public string throwAnimName = "";

    public AudioClip pullSound;
    public AudioClip throwSound;

    public float pullVolume = 0.3f;
    public float throwVolume = 0.3f;

    private bool isThrowing; // Is already throwing a grenade?
    private bool canThrow; // Can throw a grenade?

    [Space()]
    public WeaponsManager weaponManager;
    public AudioManager audioManager;
    public PlayerUI ui;

	private void Update ()
    {
        GetUserInput(); // Checks if the user is pressing some action key.
        UpdateUI();  // Update the UI showing the amount of grenades.
    }

    /// <summary>
    /// Updates information about the items in the UI.
    /// </summary>
    private void UpdateUI ()
    {
        ui.SetGranadesAmount(numberOfGrenades); // Updates the amount of grenades.

        if (isThrowing) // Is throwing a grenade?
        {
            ui.ShowCrosshair(true); // Enable the crosshair.
            ui.SetCrosshairType(CrosshairStyle.Point); // Defines the type as a point.
        }
    }

    /// <summary>
    /// Checks whether the user is pressing any action key and invokes the corresponding method.
    /// Action keys: G = (Throw a grenade).
    /// </summary>
    private void GetUserInput ()
    {
        if (numberOfGrenades > 0) // Still have grenades available?
        {
            if (!isThrowing) // Is not already throwing a grenade?
            {
                if (Input.GetKeyDown(KeyCode.G) && weaponManager.canUseItems)
                {
                    HoldToThrowGrenade(); // Pull the grenade pin.
                    canThrow = true; // Can throw the grenade.
                }
            }
            else
            {
                // Hold the Key to throw the grenade with more force.
                if (Input.GetKey(KeyCode.G))
                {
                    if (forceMultiplier <= maxForceMultiplier)
                        forceMultiplier += Time.deltaTime; // Increase the force multiplier with time.
                }

                // Release the Key to throw the grenade with the current force (forceMultiplier).
                if (Input.GetKeyUp(KeyCode.G) && canThrow)
                {
                    canThrow = false;
                    StartCoroutine(ThrowGrenade(forceMultiplier));
                    forceMultiplier = 0;
                }
            }
        }
    }

    /// <summary>
    /// Method that starts the process of throwing a grenade.
    /// </summary>
    private void HoldToThrowGrenade()
    {
        isThrowing = true;
        weaponManager.HideCurrentWeapon(); // Hides the current weapon.

        //PullAnimation(); // Play Pull the Pin Animation.
    }

    /// <summary>
    /// Play throwing grenade animation and invokes the method that instantiates the grenade.
    /// Parameters: How much time the player holds the grenade.
    /// </summary>
    private IEnumerator ThrowGrenade (float holdTime)
    {
        if (GetPullAnimTime() < holdTime) // If has finished the pull the pin animation.
        {
            //ThrowAnimation(); // Play throw animation.

            yield return new WaitForSeconds(delayToThrow);

            InstantiateGrenade(holdTime);

            yield return new WaitForSeconds(GeThrowAnimTime() - delayToThrow);
            weaponManager.SelectCurrentWeapon(); // Activate the current weapon after throwing the grenade.
            isThrowing = false;
        }
        else // Wait until finish the pull animation to play throw animation.
        {
            yield return new WaitForSeconds(GetPullAnimTime() - holdTime);

            //ThrowAnimation(); // Play throw animation.

            yield return new WaitForSeconds(delayToThrow);

            InstantiateGrenade(holdTime);

            yield return new WaitForSeconds(GeThrowAnimTime() - delayToThrow);
            weaponManager.SelectCurrentWeapon(); // Activate the current weapon after throwing the grenade.
            isThrowing = false;
        }
    }

    /// <summary>
    /// Instantiates the grenade.
    /// </summary>
    private void InstantiateGrenade (float holdTime)
    {
        // Create a grenade.
        GameObject grenadeClone = Instantiate(grenade, throwPos.position, throwPos.rotation) as GameObject;

        //grenadeClone.GetComponent<GrenadeScript>().Detonate(holdTime);
        grenadeClone.GetComponent<GrenadeScript>().Detonate(); // Calls the method responsible for blowing up the grenade.

        // Adds force to the grenade to throw it forward.
        grenadeClone.GetComponent<Rigidbody>().velocity = grenadeClone.transform.TransformDirection(Vector3.forward) 
            * throwForce * (holdTime > 1 ? holdTime : 1);

        if (!infiniteGrenades)
            numberOfGrenades--;
    }

    /// <summary>
    /// Method responsible for play the Pull animation.
    /// </summary>
    private void PullAnimation ()
    {
        grenadeAnim.Play(pullAnimName);
        audioManager.PlayGenericSound(pullSound, pullVolume);
    }

    /// <summary>
    /// Method responsible for play the Throw animation.
    /// </summary>
    private void ThrowAnimation()
    {
        grenadeAnim.Play(throwAnimName);
        audioManager.PlayGenericSound(throwSound, throwVolume);
    }

    /// <summary>
    /// Returns the duration of the Throw animation in seconds.
    /// </summary>
    private float GeThrowAnimTime()
    {
        return grenadeAnim != null ? throwAnimName.Length > 0 ? grenadeAnim[throwAnimName].length : 0 : 0;
    }

    /// <summary>
    /// Returns the duration of the Pull animation in seconds.
    /// </summary>
    private float GetPullAnimTime ()
    {
        return grenadeAnim != null ? pullAnimName.Length > 0 ? grenadeAnim[pullAnimName].length : 0 : 0;
    }
}
