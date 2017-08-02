using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for managing bullet marks.
/// </summary>
public class BulletMarkManager : MonoBehaviour 
{
    public int maxMarks = 100; // The max bullet marks.
    public BulletMarks[] marks;  // List of bullet marks types.

    public AudioManager audioManager; // Audio Manager.

    private List<GameObject> marksList = new List<GameObject>(); // List of existing bullet marks.

    /// <summary>
    /// Create a bullet mark based on surface.
    /// Parameters: The surface type, position, rotation and is it attached to any object (parent)?
    /// </summary>
    public void Instantiate (SurfaceType surface, Vector3 pos, Quaternion rot, Transform parent)
	{
		GameObject bulletMark = null; // Creates the object that will be our bullet mark.

        for (int i = 0; i < marks.Length; i++) // Search in the list of surfaces if the given surface has been set.
        {
			if (surface == marks[i].surface)
			{
                // Instances the particle corresponding to the surface type. 
                bulletMark = Instantiate (GetParticle(marks[i].particles), pos, rot) as GameObject;

                // If there is any impact sound for the given surface.
                if (marks[i].sounds.Length > 0)
                {
                    // Play the surface hit sound at position.
                    audioManager.PlayBulletImpact(marks[i].sounds[Random.Range(0, marks[i].sounds.Length)], 0.8f, pos);
                }
				
				if (marks[i].textures.Length > 0 && bulletMark.GetComponentInChildren<MeshRenderer>() != null)
                {
                    // Adds a random texture to the bullet mark.
                    bulletMark.GetComponentInChildren<MeshRenderer>().material.mainTexture = marks[i].textures[Random.Range(0, marks[i].textures.Length)];
                }	
			}
		}

        // Sets a random size for the bullet mark.
        float size = Random.Range(0.5f, 1.2f);
        bulletMark.transform.localScale = new Vector3(size, size, size);
        bulletMark.transform.Rotate(new Vector3(0, Random.Range(-180.0f, 180.0f), 0)); // Sets a random rotation.

        bulletMark.GetComponent<ParticleSystem>().Play(); // Play the particles of the bullet mark

        // Attaches the bullet to the object so that they are connected.
        bulletMark.transform.parent = parent;

        // Add the current bullet mark to bullet mark list.
        AddToList(bulletMark);

        // Destroy after 90 secs
		Destroy(bulletMark, 90);
	}

    /// <summary>
    /// Adds a bullet mark to the list so that the number of instantiated tags can be managed.
    /// Parameters: The bullet mark.
    /// </summary>
	private void AddToList (GameObject go)
	{
		if (marksList.Count == maxMarks) 
		{
			GameObject auxGO = marksList [0] as GameObject;
			Destroy (auxGO);
			marksList.RemoveAt (0);
		}

		if (go != null)
			marksList.Add (go);
	}

    /// <summary>
    /// Selects a random particle to be instantiated.
    /// </summary>
	private GameObject GetParticle (GameObject[] particleList)
	{
		return particleList[Random.Range(0, particleList.Length)];
	}
}
