using UnityEngine;

/// <summary>
/// Class responsible for defining the parameters of the object that the player will vault.
/// </summary>
public class ParkourTrigger : MonoBehaviour
{
    public Vector3 endPosition; // The final position of the player after jumping the object.
    public float speed = 2; // Vault speed.
    public bool climb; // The player will climb on top of the object or just go over?
}
