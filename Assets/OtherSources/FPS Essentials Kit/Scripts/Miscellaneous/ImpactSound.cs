using UnityEngine;

/// <summary>
/// Class responsible for checking and playing a sound when two objects collide.
/// </summary>
[RequireComponent(typeof (AudioSource))]
public class ImpactSound : MonoBehaviour
{
    public float minImpactForce = 2; // Minimum linear velocity to detect a collision between two objects.
    public AudioClip collisionSound; // Sound played when two objects collide.
    public float volume = 0.3f; // Sound volume.

    /// <summary>
    /// Method responsible for checking the collision of this object with any other.
    /// Parameters: The information about the collision.
    /// </summary>
    private void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > minImpactForce) // If the impact velocity is greater than the minimum speed.
        {
            GetComponent<AudioSource>().clip = collisionSound; // Set AudioSource.clip as collision sound.
            GetComponent<AudioSource>().volume = volume; // Set AudioSource.volume.
            GetComponent<AudioSource>().Play();
        }
    }
}
