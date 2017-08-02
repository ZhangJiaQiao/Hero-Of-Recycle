using UnityEngine;

/// <summary>
/// Class responsible for controlling all the sounds emitted by the player.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource footstepsSource; // Control the footsteps sounds.
    public AudioSource breathSource; // Control the breathing sounds.
    public AudioSource damageSource; // Control the damage sounds.

    public AudioSource weaponNearSource; // Control the weapon sounds (Reaload, Draw, Vault, Melee).
    public AudioSource weaponFarSource; // Control the weapon sounds (Fire).

    public AudioSource effectsSource; // Control the effects sounds applied to the owner
    public AudioSource heartSource; // Control the heartbeat sound.

    public AudioReverbZone reverbZone; // Control the player reverb zone to apply effects on played audios.

    [HideInInspector]
    public bool isDying; // Hit points is low?

    // Update is called once per frame
    private void Update ()
    {
        if (!isDying)
        {
            //Ray Directions
            bool up = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), 5);

            bool left = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), 5);
            bool right = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), 5);

            bool forward = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), 5);
            bool back = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), 5);

            if (up && (left || right) && (forward || back)) // Is the player inside something (a place tight)?
            {
               ChangeReverbPreset(AudioReverbPreset.Room);
            }
            else
            {
                ChangeReverbPreset(AudioReverbPreset.Off); // Else disable the audio effects
            }
        }
        else
        {
            ChangeReverbPreset(AudioReverbPreset.Arena);
        }
    }

    /// <summary>
    /// Change the ReverbZone preset.
    /// Parameters: The new AudioReverbPreset.
    /// </summary>
    private void ChangeReverbPreset (AudioReverbPreset preset)
    {
        if (reverbZone.reverbPreset != preset) // Current preset is different from the new AudioReverbPreset?
        {
            reverbZone.enabled = false; // Disable the reverb zone.
            reverbZone.reverbPreset = preset; // Change the ReverbZone preset.
            reverbZone.enabled = true; // Enable the reverb zone with the new preset.
        }
    }

    /// <summary>
    /// Play bullet impact sound at a given position in world space.
    /// Parameters: The impact sound, sound volume and impact position.
    /// </summary>
    public void PlayBulletImpact (AudioClip clip, float volume, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }

    /// <summary>
    /// Play shot sound.
    /// Parameters: Shot sound and volume.
    /// </summary>
	public void PlayShot (AudioClip clip, float volume)
	{
		weaponFarSource.PlayOneShot(clip, volume); // Plays shot sound, and scales the AudioSource volume by volume.
    }

    public void StopReload ()
    {
		weaponNearSource.Stop(); // Stops playing the clip.        
    }

    /// <summary>
    /// Play reload sound.
    /// Parameters: Reload sound and volume.
    /// </summary>
	public void PlayReload (AudioClip clip, float volume)
	{
        weaponNearSource.clip = clip; // Set AudioSource.clip as Reload sound.
        weaponNearSource.volume = volume; // Set AudioSource.volume.
        weaponNearSource.Play();
    }

    /// <summary>
    /// Play a generic weapon sound, like Hide or Vault.
    /// Parameters: Generic sound and volume.
    /// </summary>
	public void PlayGenericSound (AudioClip clip, float volume)
	{
        weaponNearSource.clip = clip; // Set AudioSource.clip as Generic sound.
        weaponNearSource.volume = volume; // Set AudioSource.volume.
        weaponNearSource.Play();
    }

    /// <summary>
    /// Plays a noise sound when something happens near the player, like an explosion.
    /// Parameters: Noise sound and volume.
    /// </summary>
    public void PlayNoiseSound(AudioClip clip, float volume)
    {
        effectsSource.clip = clip; // Set AudioSource.clip as Noise sound.
        effectsSource.volume = volume; // Set AudioSource.volume.
        effectsSource.Play();
    }

    /// <summary>
    /// Plays the player breathing sound when he gets tired.
    /// Parameters: Breathing sound, current stamina amount and from which value we can start calculate the volume.
    /// </summary>
    public void PlayBreathingSound (AudioClip clip, float staminaAmount, float startFrom)
    {
        if (CalculateVolumePercentage(startFrom, staminaAmount, 0.6f) > 0) // Volume greater than 0?
        {
            breathSource.volume = CalculateVolumePercentage(startFrom, staminaAmount, 0.6f);

            if (!breathSource.isPlaying) // Is the breathing sound playing right now?
            {
                breathSource.PlayOneShot(clip); // Plays breathing sound, and scales the AudioSource volume by volume.
            }
        }
    }

    /// <summary>
    /// Plays the player heartbeat sound when hit points is low.
    /// Parameters: Heartbeat sound, current hit points amount and from which value we can start calculate the volume.
    /// </summary>
    public void PlayHeartbeatSound (AudioClip clip, float currentHp, float startFrom)
    {
        if (CalculateVolumePercentage(startFrom, currentHp, 0.6f) > 0) // Volume greater than 0?
        {
            heartSource.volume = CalculateVolumePercentage(startFrom, currentHp, 0.6f);

            if (!heartSource.isPlaying) // Is the heartbeat sound playing right now?
            {
                heartSource.PlayOneShot(clip); // Plays heartbeat sound, and scales the AudioSource volume by volume.
            }
        }
    }

    /// <summary>
    /// Calculate the volume based on the percentage of the current amount related to the start amount.
    /// Parameters: Start value, current value and max volume.
    /// </summary>
    private float CalculateVolumePercentage(float start, float amount, float maxVolume)
    {
        float vol = 1 - amount / start; // The volume becomes positive when the amount < or = the start amount.
        return Mathf.Clamp(vol, 0, maxVolume); // Ensures that the return value is not less than 0 or greater than the maximum volume
    }

    /// <summary>
    /// Play footstep sound.
    /// Parameters: Footstep sound and volume.
    /// </summary>
    public void PlayFootstepSound (AudioClip clip, float volume)
    {
        footstepsSource.PlayOneShot(clip, volume); // Plays footstep sound, and scales the AudioSource volume by volume.
    }
}
