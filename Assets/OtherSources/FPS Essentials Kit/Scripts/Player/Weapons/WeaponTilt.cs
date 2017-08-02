using UnityEngine;

/// <summary>
/// Class used to simulate the tilting effects of a weapon, for a less robotic tone.
/// </summary>
public class WeaponTilt : MonoBehaviour 
{
    public float smooth = 4.0f; // Speed to stabilize the movement.
    public float angle = 5.0f; // Average angle that the gun can tilt.
    public float maxTiltAngle = 15; // Maximum angle that the gun can tilt.

    public MoveController controller; // The player.

    private void Update () 
    {
        // If the player is not stopped or you are moving the mouse.
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || controller.GetInput() != Vector2.zero)
        {
            // Tilt in Y.
            float TiltY = Mathf.Clamp(Input.GetAxis("Mouse X") * -angle, -maxTiltAngle, maxTiltAngle);

            // Tilt in X.
            float TiltX = Mathf.Clamp(Input.GetAxis("Mouse Y") * angle, -maxTiltAngle, maxTiltAngle);

            // Tilt in Z.
            float TiltZ = controller.GetComponent<Rigidbody>().velocity.magnitude >= controller.crouchSpeed * 0.8f ?
                Mathf.Clamp(controller.GetInput().x * -angle, -maxTiltAngle, maxTiltAngle) : 0;

            // Defines the end position according to the tilt on each axis.
            Quaternion newRotation = Quaternion.Euler(TiltX, TiltY, TiltZ);

            // Moves the weapon from the current rotation to the end rotation.
            transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, Time.deltaTime * smooth);
        }
        else
        {
            // If the player is not moving and the mouse input is zero (Vector2.zero), reset it to its original position.
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * smooth);
        }
	}
}
