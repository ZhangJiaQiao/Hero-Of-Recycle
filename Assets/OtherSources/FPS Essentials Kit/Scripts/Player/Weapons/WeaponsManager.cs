using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class responsible for managing the weapons which weapons the player is using or may use.
/// </summary>
public class WeaponsManager : MonoBehaviour 
{
    public List<Weapon> weaponList = new List<Weapon>(); // List of available weapons.
    public List<Weapon> weaponEquipped = new List<Weapon>(); // List of weapons equipped.

    public int maxWeapons; // Maximum number of weapons the player can carry.

    public MoveController controller; // The player.
    public Camera mainCamera; // Player camera.
    public PlayerUI UI; // UI.
    public float interactionRange = 2; // Minimum distance for the player to interact with a weapon.

    [HideInInspector]
	public bool ready; // Is the player ready to change weapons?

    private bool readyToUseItems; // Is the player ready to use items?

    /// <summary>
    /// Returns true or false depending on whether or not the player is using items.
    /// </summary>
    public bool canUseItems
    {
        get
        {
            return ready && readyToUseItems && !isSwitching; // Are the player ready to hide weapon and use items?
        }

        set
        {
            readyToUseItems = value;
        }
    }

    private int currentWeapon = 0; // What is the currently selected weapon index?

    [HideInInspector]
    public bool canSwitch; // Can the player switch of weapon?

    [HideInInspector]
	public bool isSwitching; // Is the player switching weapons?

    private void Start ()
	{
        // If the player has any weapons in the list of equipped weapons, select the first one.
        if (weaponEquipped.Count > 0 && weaponEquipped[currentWeapon] != null)
        {
            Select(weaponEquipped[currentWeapon]);
        }
        else
        {
            ready = true;
            controller.canVault = true;
        }

        // Calculates initial weight.
        CalculateWeight();
	}

    /// <summary>
    /// Returns a key according to the given index.
    /// Parameters: The selected weapon index.
    /// </summary>
    private KeyCode GetWeaponKeyCode(int index)
    {
        switch (index)
        {
            case 0: return KeyCode.Alpha1;
            case 1: return KeyCode.Alpha2;
            case 2: return KeyCode.Alpha3;
            case 3: return KeyCode.Alpha4;
            case 4: return KeyCode.Alpha5;
            case 5: return KeyCode.Alpha6;
            case 6: return KeyCode.Alpha7;
            case 7: return KeyCode.Alpha8;
            case 8: return KeyCode.Alpha9;
            case 9: return KeyCode.Alpha0;
        }
        return KeyCode.None;
    }

    // Update is called once per frame
    private void Update() 
	{
        // Restrictions to switch.
        canSwitch = ready && controller.moveState != MoveState.Running && !isSwitching && !controller.isClimbing;
        
        for (int i = 0; i < weaponEquipped.Count; i++)
		{
            // If you press a key corresponding to the index of some weapon in the list of equipped weapons.
            if (Input.GetKeyDown(GetWeaponKeyCode(i)) && currentWeapon != i && canSwitch)
			{
				StartCoroutine(Switch(weaponEquipped[currentWeapon], weaponEquipped[i]));
				currentWeapon = i;
			}
		}

        // Only displays the weapons UI if have any weapons selected.
        UI.ShowWeaponsGUI (weaponEquipped.Count > 0);

        // Check if there is any weapon near the player.
        SearchForWeapons();
    }

    private void SearchForWeapons()
    {
        Vector3 direction = mainCamera.transform.TransformDirection(Vector3.forward); // Looking direction.
        Vector3 origin = mainCamera.transform.position; // Position of the player's head.

        Ray ray = new Ray(origin, direction); // Creates a ray starting at origin along direction.
        RaycastHit hitInfo; // Structure used to get information back from a raycast.

        // If the player looks at any nearby weapon (within the range of interaction).
        if (Physics.Raycast(ray, out hitInfo, interactionRange))
        {
            // If the ray hit a gun.
            if (hitInfo.collider.GetComponent<WeaponInfo>() != null)
            {
                // Grab the gun information in the weapons list looking for its id.
                Weapon w = GetWeapon(hitInfo.collider.GetComponent<WeaponInfo>().weaponId);

                // If the weapon exists and the player is not running.
                if (w != null && controller.moveState != MoveState.Running)
                {
                    // If the weapon is not already equipped.
                    if (!isEquipped(w.weaponId))
                    {
                        // If have slot to pick up the weapon.
                        if (weaponEquipped.Count < maxWeapons)
                        {
                            // Displays a message for the player to pick up the weapon.
                            UI.ShowWeaponPickUp(!isEquipped(w.weaponId), w.weaponName);

                            // If press the pick up button.
                            if (Input.GetKeyDown(KeyCode.E) && !isSwitching && ready)
                            {
                                // Adds the weapon to the list of weapons equipped.
                                weaponEquipped.Add(w);

                                // Play the pick up animation.
                                StartCoroutine(Switch(weaponEquipped[currentWeapon], w));

                                // Select the weapon you just picked up.
                                currentWeapon = GetWeaponIndex(w);

                                // If the weapon you picked up must be destroyed by picking up.
                                if (hitInfo.collider.GetComponent<WeaponInfo>().destroyWhenPickup)
                                {
                                    // Destroy the prefab.
                                    Destroy(hitInfo.collider.gameObject);
                                }

                                // Recalculates the weight of the weapons.
                                CalculateWeight();
                            }
                        }
                        else // If you do not have space to get new weapons, then we should change the current weapon.
                        {
                            // Displays a message to the player to change his weapon.
                            UI.ShowWeaponSwitch(true, weaponEquipped[currentWeapon].weaponName, w.weaponName);

                            // If press the pick up button.
                            if (Input.GetKeyDown(KeyCode.E) && !isSwitching && ready)
                            {
                                // Saves the selected weapon, to be instantiated your copy after.
                                Weapon lastWeapon = weaponEquipped[currentWeapon];

                                // Adds the weapon to the list of weapons equipped.
                                weaponEquipped.Add(w);

                                // Call the method to play switch weapon animation.
                                StartCoroutine(Switch(weaponEquipped[currentWeapon], w));

                                // Removes the current weapon from the list of weapons equipped.
                                weaponEquipped.Remove(weaponEquipped[currentWeapon]);

                                // Select the weapon you just picked up.
                                currentWeapon = GetWeaponIndex(w);

                                // Instantiate a prefab clone of the weapon you were equipped with.
                                PickAndDrop(hitInfo.collider.gameObject, lastWeapon);

                                // Recalculates the weight of the weapons.
                                CalculateWeight();
                            }
                        }
                    }
                    else
                    {
                        // Removes pick up or change messages and displays a message saying that the weapon is already selected.
                        UI.ShowWeaponPickUp(false, null);
                        UI.ShowWeaponSwitch(false, null, null);
                        UI.ShowAmmoInfo(w.weaponName + " ALREADY EQUIPPED");
                    }    
                }
            }
            else
            {
                // Hide messages.
                UI.ShowWeaponPickUp(false, null);
                UI.ShowWeaponSwitch(false, null, null);
            }
        }
        else
        {
            // Hide messages.
            UI.ShowWeaponPickUp(false, null);
            UI.ShowWeaponSwitch(false, null, null);
        }
    }

    /// <summary>
    /// Instantiate a prefab of the weapon you just dropped.
    /// Parameters: The gameObject of the weapon you just picked up and the weapon that you will instantiate a clone.
    /// </summary>
    private void PickAndDrop (GameObject w, Weapon lastWeapon)
    {
        //yield return new WaitForSeconds(weaponEquipped[currentWeapon].GetHideTime());

        // Instantiate a clone of the current weapon.
        Instantiate(lastWeapon.droppablePrefab, w.transform.position, w.transform.rotation);
        Destroy(w); // Destroy the prefab of the weapon you just picked up.
    }

    /// <summary>
    /// Returns a weapon from the list of weapons that has the ID equal to the ID passed by parameter.
    /// Parameters: The weapon ID.
    /// </summary>
    private Weapon GetWeapon (int id)
    {
        foreach (Weapon w in weaponList)
        {
            if (w.weaponId == id)
                return w;
        }
        return null;
    }

    /// <summary>
    /// Returns the ID of the weapon.
    /// Parameters: The weapon what do you want to know the ID.
    /// </summary>
    private int GetWeaponIndex (Weapon w)
    {
        for (int i = 0; i < weaponEquipped.Count; i++)
        {
            if(w.weaponId == weaponEquipped[i].weaponId)
            {
                return i;
            }
        }
        return 0;
    }

    /// <summary>
    /// Returns true if weapon is already equipped.
    /// Parameters: The weapon ID.
    /// </summary>
    private bool isEquipped(int id)
    {
        for (int i = 0; i < weaponEquipped.Count; i++)
        {
            if (weaponEquipped[i].weaponId == id)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Calculate the weight the player is carrying.
    /// </summary>
    private void CalculateWeight ()
	{
		float weight = 0;

        // For each weapon the player is carrying.
        for (int i = 0; i < weaponEquipped.Count; i++)
		{
			weight += weaponEquipped[i].weight; // Sum weight of all weapons.
        }
		controller.weight = Mathf.RoundToInt(weight); // Convert to nearest integer and sets the weight the player are carrying.
    }

    /// <summary>
    /// Selects the weapon given.
    /// Parameters: The weapon that will be selected.
    /// </summary>
	private void Select(Weapon w)
	{
		w.Select ();
	}

    /// <summary>
    /// Swap your current weapon with a new one.
    /// Parameters: Your current weapon and the new weapon.
    /// </summary>
    private IEnumerator Switch (Weapon oldWeapon, Weapon newWeapon)
	{
        isSwitching = true;
        oldWeapon.Deselect ();
        yield return new WaitForSeconds (oldWeapon.GetHideTime ());
		newWeapon.Select ();
		isSwitching = false;
    }

    /// <summary>
    /// When the player have no more ammunition, swap for any weapon that still has ammo available.
    /// </summary>
    public void OutOfAmmo ()
    {
        // If you have more than one equipped weapon.
        if (weaponEquipped.Count > 1)
        {
            // Look for a weapon in the weapons equipped list.
            for (int i = 0; i < weaponEquipped.Count; i++)
            {
                // If the weapon has ammunition.
                if (!weaponEquipped[i].OutOfAmmo)
                {
                    // Swap the current weapon for the weapon that still has ammo.
                    StartCoroutine(Switch(weaponEquipped[currentWeapon], weaponEquipped[i]));
                    currentWeapon = i;
                }
            }
        }
    }

    /// <summary>
    /// Hides the current weapon without play hide animation.
    /// </summary>
    public void HideCurrentWeapon ()
    {
        if (ready)
            weaponEquipped[currentWeapon].FastDeselect(); 
    }

    /// <summary>
    /// Select the current weapon.
    /// </summary>
    public void SelectCurrentWeapon ()
    {
        weaponEquipped[currentWeapon].Select();
    }

    /// <summary>
    /// Hides the current weapon with a delay without play hide animation.
    /// </summary>
    public void HideCurrentWeapon (float secs)
    {
        if (ready)
            StartCoroutine(HideAndSelect(secs));     
    }

    /// <summary>
    /// Hide the weapon, wait a few seconds, and select again.
    /// Parameters: How long will have to wait.
    /// </summary>
    private IEnumerator HideAndSelect (float secs)
    {
        weaponEquipped[currentWeapon].FastDeselect();
        yield return new WaitForSeconds(secs);
        weaponEquipped[currentWeapon].Select();
    }

    /// <summary>
    /// Returns the time required to play vault animation.
    /// </summary>
    public float GetVaultTime()
    {
        return weaponEquipped[currentWeapon] != null ? weaponEquipped[currentWeapon].GetVaultTime() : 0; 
    }

    /// <summary>
    /// Play vault animation on selected weapon.
    /// Parameters: The player will climb on the object?
    /// </summary>
    public void Vault (bool climb)
    {
        // If have any weapon selected.
        if (weaponEquipped.Count > 0)
        {
            if (weaponEquipped[currentWeapon] != null && weaponEquipped.Count > 0)
            {
                //  Play vault animation on selected weapon.
                weaponEquipped[currentWeapon].Vault(climb);
            }
        } 
    }
}
