using UnityEngine;

/// <summary>
/// Base class for basic camera animation.
/// </summary>
[System.Serializable]
public class BasicShake
{
    /// <summary>
    /// Sets the end position or rotation randomly between vectors min and max.
    /// </summary>
	[System.Serializable]
	public struct Intensity
	{
		public Vector3 min; // Minimum end position or rotation.  
        public Vector3 max; // Maximum end position or rotation.  
    }

	public Intensity intensity; // Create a structure to manipulate the position or rotation of the target.

    [Space(10)]
	public float speed = 10; // Transition speed between position or rotation.
    public Transform target; // Transform that will be affected by animation.
}
