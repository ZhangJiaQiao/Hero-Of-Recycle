using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class responsible for controlling the sound of footsteps.
/// </summary>
public class Footsteps : MonoBehaviour
{
	public List<GroundType> surfaces = new List<GroundType>(); // List of surfaces.

    public AudioManager audioManager; // The audio manager.

    private bool canPlay = true; // Can play the sound?
    private MoveController moveController; // The player.

    // Use this for initialization
    private void Start ()
    {
        moveController = GetComponent<MoveController>();
    }
	
	// Update is called once per frame
	private void Update ()
    {
        // Delay between each step (walking) in seconds.
        float audioLengthWalking = 0.4f + ((moveController.walkingSpeed - 
            moveController.RealWalkingSpeed()) / moveController.RealWalkingSpeed());

        // Delay between each step (running) in seconds.
        float audioLengthRunning =  0.35f + ((moveController.walkingSpeed -
            moveController.RealWalkingSpeed() * moveController.RunMultiplierWithStamina()) / 
            (moveController.RealWalkingSpeed() * moveController.RunMultiplierWithStamina() * 5));

        // Is the player grounded?
        if (moveController.Grounded)
        {
            // Walking
            if (moveController.moveState == MoveState.Walking)
            {
                //Plays the footsteps sounds according to the surface.
                StartCoroutine(PlayFootStep(audioLengthWalking, moveController.moveState));
            }
            // Running
            else if (moveController.moveState == MoveState.Running)
            {
                //Plays the footsteps sounds according to the surface.
                StartCoroutine(PlayFootStep(audioLengthRunning, moveController.moveState));
            }
            // Crouched
            else if (moveController.moveState == MoveState.Crouched)
            {
                //Plays the footsteps sounds according to the surface.
                StartCoroutine(PlayFootStep(moveController.crouchSpeed / 3, moveController.moveState));
            }
        }
	}

    /// <summary>
    /// Return a AudioClip based on the surface that the player is currently on.
    /// </summary>
    private AudioClip GetAudio ()
    {
        RaycastHit hitInfo; // Structure used to get information back from a raycast.
        SurfaceType surface = SurfaceType.Default;

        // Check the surface below the player.
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity))
        {
            // If the object that the player is on has the component Surface.
            if (hitInfo.transform.GetComponent<Surface>() != null)
            {
                // Take the surface type of the object.
                surface = hitInfo.collider.GetComponent<Surface>().GetSurface(hitInfo.point, hitInfo.collider.gameObject);
            }
            else
            {
                // If the object does not have the component, assume that it is a generic surface.
                surface = SurfaceType.Default;
            }
        }

        // Check whether the surface is set in the list of surfaces.
        for (int i = 0; i < surfaces.Count; i++)
        {
            // If the surface is found, returns its respective sound.
            if (surface == surfaces[i].surfaceType)
            {
                return surfaces[i].clips[Random.Range(0, surfaces[i].clips.Count)];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the volume of the sound based on the player's current state and the surface he is currently on.
    /// </summary>
    private float GetVolume (MoveState s)
    {
        RaycastHit hitInfo;  // Structure used to get information back from a raycast.
        SurfaceType surface = SurfaceType.Default;

        // Check the surface below the player.
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity))
        {
            // If the object that the player is on has the component Surface.
            if (hitInfo.transform.GetComponent<Surface>() != null)
            {
                // Take the surface type of the object.
                surface = hitInfo.collider.GetComponent<Surface>().GetSurface(hitInfo.point, hitInfo.collider.gameObject);
            }
            else
            {
                // If the object does not have the component, assume that it is a generic surface.
                surface = SurfaceType.Default;
            }
        }

        // Check whether the surface is set in the list of surfaces.
        for (int i = 0; i < surfaces.Count; i++)
        {
            // If the surface is found, returns its respective sound volume based on the player's current state.
            if (surface == surfaces[i].surfaceType)
            {
                if(s == MoveState.Crouched)
                    return surfaces[i].crouchVolume;
                else if (s == MoveState.Walking)
                    return surfaces[i].walkVolume;
                else if (s == MoveState.Running)
                    return surfaces[i].runVolume;
            }
        }
        return 0;
    }

    /// <summary>
    /// Play the sound of footsteps depending on the surface of the player's current state.
    /// </summary>
    private IEnumerator PlayFootStep (float audioLength, MoveState s)
    {
        if (canPlay)
        {
            canPlay = false;
            audioManager.PlayFootstepSound(GetAudio(), GetVolume(s)); // Selects a random clip and volume.
            yield return new WaitForSeconds(audioLength);
            canPlay = true;
        }
    }
}
