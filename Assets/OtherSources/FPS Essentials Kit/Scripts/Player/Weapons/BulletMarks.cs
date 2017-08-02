using UnityEngine;

/// <summary>
/// Class that represents the properties of a bullet mark.
/// </summary>
[System.Serializable]
public class BulletMarks 
{
	public SurfaceType surface = SurfaceType.Default; // Related Surface.
    public GameObject[] particles; // List of particles.
    public Texture2D[] textures; // List of textures.
	public AudioClip[] sounds; // List of impact sounds.
}
