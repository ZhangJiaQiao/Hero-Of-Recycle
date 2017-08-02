using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// Class responsible for simulating the camera effects.
/// </summary>
public class CameraEffects : MonoBehaviour
{
	public AudioManager audioManager; // The audio manager.
    public MoveController player; // Player.

	public MotionBlur motionBlur; // The motion blur effect.
    public VignetteAndChromaticAberration vignetteAndChromaticAberration; // The camera Vignette and Chromatic Aberration.
    public CameraMotionBlur cameraMotionBlur; // The camera motion blur effect.

    public AudioClip noiseSound; // This audio is played when something explodes near to the player.

	private bool disoriented; // The player are disoriented?

	private float disorientIntensity; // The disorient intensity.

	/// <summary>
	/// Invoke the method to simulate the explosion effect.
	/// </summary>
    public void ExplosionEffect (float intensity)
    {
		disorientIntensity = intensity; // Intensity (Max = 1, Min = 0).

        // If the intensity is greater than 0.4, we can say that the explosion was very strong.
        if (disorientIntensity > 0.4f)
        {
			StartCoroutine(Disoriented()); // Apply disoriented effect.
        }
    }

	/// <summary>
	/// Simulates the effect of disoriented by a explosion.
	/// </summary>
    private IEnumerator Disoriented ()
    {
		if (!disoriented) // The coroutine is not running?
        {
            disoriented = true;

            // Play the disoriented sound.
            audioManager.PlayNoiseSound(noiseSound, disorientIntensity);
            yield return new WaitForSeconds(noiseSound.length);

            disoriented = false;
        }
    }

    public void Update ()
    {
        // If the player are disoriented, blurs the screen to obstruct the view.
        if (disoriented)
        {
            motionBlur.blurAmount = Mathf.Lerp(motionBlur.blurAmount, Mathf.Clamp(disorientIntensity * 1.5f, 0, 0.9f), Time.deltaTime * 10);
        }
        else
        {
            // If the effect has passed, clears the screen gradually over time.
            if (motionBlur.blurAmount > 0.1f)
                motionBlur.blurAmount = Mathf.Lerp(motionBlur.blurAmount, 0, Time.deltaTime / 2);
            else
                motionBlur.blurAmount = 0;
        }

        // If the camera is moving, enable motion blur.
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            cameraMotionBlur.enabled = true;
        }
        else
        {
            cameraMotionBlur.enabled = false;
        }

        // If the player is aiming, create an area of blur around the weapon.
        if (player.isAiming)
        {
            vignetteAndChromaticAberration.enabled = true;
            vignetteAndChromaticAberration.intensity = Mathf.Lerp(vignetteAndChromaticAberration.intensity, 0.2f, Time.deltaTime * 10);
            vignetteAndChromaticAberration.blur = Mathf.Lerp(vignetteAndChromaticAberration.blur, 0.8f, Time.deltaTime * 10);
        }
        else
        {
            vignetteAndChromaticAberration.intensity = Mathf.Lerp(vignetteAndChromaticAberration.intensity, 0, Time.deltaTime * 10);
            vignetteAndChromaticAberration.blur = Mathf.Lerp(vignetteAndChromaticAberration.blur, 0, Time.deltaTime * 10);

            if (vignetteAndChromaticAberration.blur < 0.03f)
                vignetteAndChromaticAberration.enabled = false;
        }
    }
}
