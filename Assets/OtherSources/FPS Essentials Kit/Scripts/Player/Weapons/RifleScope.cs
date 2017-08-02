using UnityEngine;

/// <summary>
/// Class responsible for controlling the rifle scope.
/// </summary>
public class RifleScope : MonoBehaviour 
{
	public Camera scopeCamera; // Camera responsible for creating enlarged images.
    public GameObject scopeLens; // Object responsible for rendering the camera image.

    public GameObject crosshair; // Crosshair

	public Material scopeMaterial; // Lens material when not aiming.
    public Material aimScopeMaterial; // Lens material when aiming.

    [HideInInspector]
	public bool isAiming = false;

	public void Update ()
	{
		if (isAiming) // The player is aiming?
        {
			if (!scopeCamera.enabled)
				scopeCamera.enabled = true; // Enables zoomed view.

            if (scopeLens.GetComponent<Renderer> ().material != aimScopeMaterial)
				scopeLens.GetComponent<Renderer> ().material = aimScopeMaterial; // Switches the lens material to the camera view. 

            crosshair.SetActive (true); // Enables the crosshair.
        } 
		else
		{
			if (scopeCamera.enabled)
				scopeCamera.enabled = false; // Disables zoomed view.

            if (scopeLens.GetComponent<Renderer> ().material != scopeMaterial)
				scopeLens.GetComponent<Renderer> ().material = scopeMaterial; // Switches the lens material to the default material. 

            crosshair.SetActive (false); // Disables the crosshair.
        }
	}

    /// <summary>
    /// Sets the camera's FOV to the given value.
    /// Parameters: The new FOV value.
    /// </summary>
    public void SetRifleScopeFOV (float fov)
    {
        scopeCamera.fieldOfView = fov;
    }
}
