using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class used to represent a surface that will be used by the footsteps script.
/// </summary>
[System.Serializable]
public class GroundType
{
	public SurfaceType surfaceType = SurfaceType.Default; // The surface type.
	public List<AudioClip> clips = new List<AudioClip>(); // List of audio clips.
    public float crouchVolume = 0.1f; // Volume when crouched.
    public float walkVolume = 0.2f; // Volume when walking.
    public float runVolume = 0.4f; // Volume when running.
}