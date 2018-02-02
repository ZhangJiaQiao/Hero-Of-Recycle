using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGrenade : MonoBehaviour {
    public ParticleSystem smoke;
    private float explosionRadius;
    private float exposionForce;
    private float damageValue;
    // Use this for initialization
    void Start()
    {
        explosionRadius = 4;
        exposionForce = 10000;
        damageValue = 50;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            Collider[] objects = Physics.OverlapSphere(transform.position, explosionRadius);
            Rigidbody rigidbodyOfCollider;
            foreach (Collider colliderObject in objects)
            {
                if (rigidbodyOfCollider = colliderObject.GetComponent<Rigidbody>())
                {
                    if (colliderObject.gameObject.GetComponent<characterProperty>())
                    {
                        rigidbodyOfCollider.AddExplosionForce(exposionForce, transform.position, explosionRadius);
                        colliderObject.gameObject.GetComponent<characterProperty>().life -= damageValue;
                    }
                }
            }
            ParticleSystem explosionParticle = Instantiate<ParticleSystem>(smoke);
            explosionParticle.transform.position = this.transform.position;
            this.gameObject.SetActive(false);
        }
    }
}
