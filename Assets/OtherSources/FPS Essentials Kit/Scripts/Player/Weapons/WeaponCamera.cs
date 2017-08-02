using UnityEngine;

public class WeaponCamera : MonoBehaviour
{
    public Transform mainCamera;

	// Update is called once per frame
	private void Update ()
    {
        transform.localPosition = mainCamera.localPosition;
        transform.localRotation = mainCamera.localRotation;
    }
}
