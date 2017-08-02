using UnityEngine;
using System.Collections;

/// <summary>
/// Base class of all grenades in the game.
/// </summary>
public class GrenadeScript : MonoBehaviour
{
    public float explosionRadius = 5;
    public float explosionForce = 10;
    public float damage = 100;

    public float timeToExplode = 5; // Time until grenade explosion.
    public GameObject particle; // The explosion particle.

    /// <summary>
    /// Invoke the method that starts the countdown until the grenade explode.
    /// Parameters: How much time the player holds the grenade.
    /// </summary>
    public void Detonate (float holdTime)
    {
        StartCoroutine(WaitToExplode(holdTime));
    }

    /// <summary>
    /// Instantly invoke the method that starts the countdown until the grenade explode.
    /// </summary>
    public void Detonate()
    {
        StartCoroutine(WaitToExplode());
    }

    /// <summary>
    /// Instantly invokes the method responsible for instantiating the explosion particle and calculate the damage made by grenade.
    /// </summary>
    private IEnumerator WaitToExplode()
    {
        yield return new WaitForSeconds(timeToExplode);
        Explode();
    }

    /// <summary>
    /// Invokes the method responsible for instantiating the explosion particle and calculate the damage made by grenade.
    /// Parameters: How much time the player holds the grenade.
    /// </summary>
    private IEnumerator WaitToExplode(float holdTime)
    {
        yield return new WaitForSeconds(timeToExplode - holdTime);
        Explode();
    }

    /// <summary>
    /// Instantiate the explosion particle and calculates the damage caused by grenade.
    /// </summary>
    public void Explode()
    {
        GameObject explosion = Instantiate(particle, transform.position, Quaternion.identity) as GameObject; // Instantiate the explosion particle.

        // Calculates damage dealt.
        Explosion.NewExplosion(explosionRadius, explosionForce, damage, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        Destroy(explosion, 3); // Destroys the particle after 3 seconds.
        Destroy(gameObject); // Destroy the grenade.
    }

    // Draw a sphere to show grenade explosion radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
