using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Footsteps))]
public class FootstepsEditor : Editor 
{
	Footsteps m_Target;

	public override void OnInspectorGUI()
	{
		m_Target = (target as Footsteps);

		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		AudioManager audioManager = (AudioManager)EditorGUILayout.ObjectField ("Audio Manager", m_Target.audioManager, typeof(AudioManager), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Audio Manager");
			m_Target.audioManager = audioManager;
		}

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Surfaces", EditorStyles.boldLabel);

		for (int i = 0; i < m_Target.surfaces.Count; i++) 
		{
			DrawSurface (i);
		}

		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button ("Add a new Surface", GUILayout.Height(32)))
		{
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Add a new Surface");
				m_Target.surfaces.Add (new GroundType ());
			}
		}
	}

	private void DrawSurface (int i)
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical ("Window");

		EditorGUILayout.BeginHorizontal();

		EditorGUI.BeginChangeCheck();
		SurfaceType surfaceType = (SurfaceType)EditorGUILayout.EnumPopup ("Surface type", m_Target.surfaces[i].surfaceType);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Surface type");
			m_Target.surfaces [i].surfaceType = surfaceType;
		}

		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button ("Remove"))
		{
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Surface Removed");
				m_Target.surfaces.Remove (m_Target.surfaces [i]);
			}
			return;
		}	
			
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("Audio clips", EditorStyles.boldLabel);
		for (int j = 0; j < m_Target.surfaces[i].clips.Count; j++) 
		{
			DrawAudioClip (i, j);
		}

		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button ("Add a new Audio Clip"))
		{
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Add a new Audio Clip");
				m_Target.surfaces [i].clips.Add (null);
			}
		}

        EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck();
		float walkVolume = EditorGUILayout.Slider("Walking Volume", m_Target.surfaces[i].walkVolume, 0, 1);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Walking Volume");
			m_Target.surfaces [i].walkVolume = walkVolume;
		}

		EditorGUI.BeginChangeCheck();
		float runVolume = EditorGUILayout.Slider("Running Volume", m_Target.surfaces[i].runVolume, 0, 1);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Running Volume");
			m_Target.surfaces [i].runVolume = runVolume;
		}

		EditorGUI.BeginChangeCheck();
		float crouchVolume = EditorGUILayout.Slider("Crouch Volume", m_Target.surfaces[i].crouchVolume, 0, 1);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Crouch Volume");
			m_Target.surfaces [i].crouchVolume = crouchVolume;
		}
        EditorGUILayout.EndVertical ();
	}

	private void DrawAudioClip (int surfaceIndex, int i)
	{
		EditorGUILayout.BeginHorizontal();
	
		EditorGUI.BeginChangeCheck();
		AudioClip clip = (AudioClip)EditorGUILayout.ObjectField (m_Target.surfaces[surfaceIndex].clips[i], typeof(AudioClip), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Audio clip");
			m_Target.surfaces [surfaceIndex].clips [i] = clip;
		}

		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button ("X", GUILayout.Width(30)))
		{
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Audio Clip Removed");
				m_Target.surfaces [surfaceIndex].clips.Remove (m_Target.surfaces [surfaceIndex].clips [i]);
			}
		}
		EditorGUILayout.EndHorizontal();
	}
}
