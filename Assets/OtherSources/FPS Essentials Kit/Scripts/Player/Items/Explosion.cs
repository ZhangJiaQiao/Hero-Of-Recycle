using UnityEngine;

/// <summary>
/// Class responsible for simulating the physics of an explosion.
/// </summary>
public class Explosion : MonoBehaviour
{
    /// <summary>
    /// Create a new explosion and applies force and damage to all objects within the radius.
    /// Parameters: The explosion radius, force, damage and explosion position in world space.
    /// </summary>
    public static void NewExplosion (float radius, float force, float damage, Vector3 pos)
    {
        // List of colliders near of the player.
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);

        // For each collider near the player.
        foreach (Collider c in hitColliders)
        {
            Vector3 dir = c.transform.position - pos;
            Ray ray = new Ray(pos, dir);

            RaycastHit hitInfo;
            // Create a ray to check if has anything intersecting the explosion and the collider.
            if (Physics.Raycast(ray, out hitInfo, radius))
            {
                if (hitInfo.collider.GetComponent<Rigidbody>() != null && hitInfo.collider.tag != "Player")
                {
                    // Apply a force to all rigidbody hit by explosion (except the player).
                    hitInfo.collider.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
                }

                // Apply damage based on distance from explosion center.
                if (hitInfo.collider.GetComponent<HealthController>() != null)
                {
                    float intensity = (radius - hitInfo.distance) / radius;
                    hitInfo.collider.GetComponent<HealthController>().ExplosionDamage(intensity * damage, intensity, pos);
                }
            }
        }
    }
}
