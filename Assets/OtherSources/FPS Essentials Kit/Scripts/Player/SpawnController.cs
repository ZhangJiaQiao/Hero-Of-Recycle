using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for managing where the player will be instantiated.
/// </summary>
public class SpawnController : MonoBehaviour 
{
	public GameObject player; // Player that will be instantiated.
    
    public List<Weapon> weaponEquipped = new List<Weapon>(); // List of weapons equipped.
    public WeaponsManager weaponsManager;  // Weapons manager that is attached to the player.

    public Color WireCubeColor = Color.yellow;

	// Use this for initialization
	private void Start ()
	{
		if (!Inside ()) // If the player is not inside any object.
        {
			weaponsManager.weaponEquipped = weaponEquipped; // Defines the weapons that the player will start.
            Instantiate (player, transform.position, transform.rotation); // Installs the player at the current spawn position.
        }
	}

	public bool SetWeaponsManager ()
	{
		if (player != null)
			weaponsManager = player.GetComponentInChildren<WeaponsManager> ();

		if (weaponsManager != null)
		{
			return true;
		} 
		else
		{
			return false;
		}
	}

    /// <summary>
    /// Returns true if the spawn is inside some object.
    /// </summary>
    public bool Inside ()
	{
		return Physics.CheckCapsule(transform.position - new Vector3(0, 0.65f), transform.position + new Vector3(0, 0.65f), 0.4f);
	}

    /// <summary>
    /// Displays a cube to be aware of the size of the player in relation to the scenario.
    /// </summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = WireCubeColor;
		Gizmos.DrawCube(transform.position, new Vector3(0.5f, 2, 0.5f));
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(0.75f, 2.25f, 0.75f));
	}
}
