using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for controlling the player's ability to jump objects.
/// </summary>
public class ParkourController : MonoBehaviour
{
    public float interactionRange = 1; // Minimum distance for the player to interact with a object.

    public WeaponsManager weaponManager; // The weapons manager.
    public CameraAnimations cameraAnimations; // The camera animations.

    /// <summary>
    /// The player.
    /// </summary>
    private MoveController controller
    {
        get
        {
            return GetComponent<MoveController>();
        }
    }
	
	// Update is called once per frame
	private void Update ()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward); // Looking direction.
        Vector3 origin = transform.position; // Position of the player's body.

        Ray ray = new Ray(origin, direction); // Creates a ray starting at origin along direction.
        RaycastHit hitInfo; // Structure used to get information back from a raycast.

        // If the player is facing some object.
        if (Physics.Raycast(ray, out hitInfo, interactionRange))
        {
            // If the player is not crouched or flying.
            if (!controller.isCrouched && controller.Grounded)
            {
                // If the player is not vaulting something.
                if (!controller.isClimbing && controller.canVault)
                {
                    // If you press the jump button or the player are running.
                    if ((Input.GetKeyDown(KeyCode.Space) && controller.GetInput().y > 0) || controller.moveState == MoveState.Running)
                    {
                        // Get the information from the object forward.
                        ParkourTrigger p = hitInfo.collider.GetComponent<ParkourTrigger>();
                        if (p != null)
                        {
                            // If the object contains the ParkourTrigger component, takes the information for the vault.
                            StartCoroutine(Vault(p.speed, p.climb, p.endPosition));
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Moves the player from the current position to the end position.
    /// Parameters: Vault speed, will the player climb on top of the object? and the end position.
    /// </summary>
    private IEnumerator Vault (float speed, bool climb, Vector3 endPosition)
    {
        // Locks the player so it can not move.
        controller.isClimbing = true;

        float t = 0.0f; // Movement progress.

        Vector3 start = transform.position;
        Vector3 end = transform.position + (transform.up * endPosition.y) + (transform.forward * endPosition.z);

        cameraAnimations.PlayParkourAnimation(); // Play vault animation on camera.
        weaponManager.Vault(climb); // Play vault animation on the current weapon.

        while (t < 1.0f)
        {
            t += Time.deltaTime * speed; // Increase movement progress.
            transform.position = Vector3.Lerp(start, end, t); // Moves the player from the current position to the end position.
            yield return null;
        }
        controller.isClimbing = false;
    }
}
