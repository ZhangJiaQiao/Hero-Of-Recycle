using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for controlling the rotation according to the mouse input.
/// </summary>
public class MouseController : MonoBehaviour
{
    [Header("X Axis")]
	public float sensitivityX = 4f; // Mouse look sensitivity.

    [Header("Y Axis")]
    public float sensitivityY = 4f; // Mouse look sensitivity.

    [Range(-360, 360)]
    public float maximumY = 60f; // Maximum angle you can look up.

    [Range(-360, 360)]
    public float minimumY = -60f; // Minimum angle you can look down.

    [Space()]
    public int frameCounter = 10; // Number of frames to be averaged, used for smoothing mouselook.
    public MoveController controller; // Player.
 
    //Mouse rotation input
    private float rotationX = 0f;
    private float rotationY = 0f;
 
    //Used to calculate the rotation of this object
    private Quaternion xQuaternion;
    private Quaternion yQuaternion;
    private Quaternion originalRotation;
 
    //Array of rotations to be averaged
    private List<float> rotArrayX = new List<float> ();
    private List<float> rotArrayY = new List<float> ();
 
    private void Start ()
    {
  		if (GetComponent<Rigidbody>())
  			GetComponent<Rigidbody>().freezeRotation = true;

       	originalRotation = transform.localRotation;
    }
 
    private void Update ()
	{
        float rotAverageX = 0f; // Average rotationX for smooth mouselook.

        // Collect the mouse input value and multiplies by the intensity, the intensity value is divided by 2 if the player are aiming.
        rotationX += Input.GetAxis("Mouse X") * (controller.isAiming ? sensitivityX * 0.5f : sensitivityX);

        rotArrayX.Add(rotationX); // Add the current rotation to the array, at the last position.

        // Reached max number of steps? Remove the oldest rotation from the array.
        if (rotArrayX.Count >= frameCounter)
        {
            rotArrayX.RemoveAt(0);
        }

        // Add all of these rotations together.
        for (int i_counterX = 0; i_counterX < rotArrayX.Count; i_counterX++)
        {
            // Loop through the array.
            rotAverageX += rotArrayX[i_counterX];
        }

        // Now divide by the number of rotations by the number of elements to get the average.
        rotAverageX /= rotArrayX.Count;

        // Average rotationY, same process as above.
        float rotAverageY = 0;

		rotationY += Input.GetAxis("Mouse Y") * (controller.isAiming ? sensitivityY * 0.5f : sensitivityY);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        rotArrayY.Add(rotationY);

        if (rotArrayY.Count >= frameCounter)
        {
            rotArrayY.RemoveAt(0);
        }

        for (int i_counterY = 0; i_counterY < rotArrayY.Count; i_counterY++)
        {
            rotAverageY += rotArrayY[i_counterY];
        }

        rotAverageY /= rotArrayY.Count;

        // Apply and rotate this object.
        xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
        yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
	}

    /// <summary>
    /// Clamps an angle between mininum and maximum values.
    /// Parameters: The angle, minimum and maximum values.
    /// </summary>
    private float ClampAngle (float angle, float min, float max)
    {
 		if (angle < -360f)
         	angle += 360f;
 
       	if (angle > 360f)
         	angle -= 360f;

       	return Mathf.Clamp (angle, min, max);
    }
}
