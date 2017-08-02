using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Surface))]
[CanEditMultipleObjects]
public class SurfaceEditor : Editor
{
    Surface m_Target;

    public override void OnInspectorGUI()
    {
        m_Target = (target as Surface);

        if (m_Target.surface == null)
            m_Target.surface = m_Target.SetSurfaceList();

        EditorGUILayout.Space();
        if (m_Target.surface != null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Texture List", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < m_Target.surface.Length; i++)
            {
               ShowSurfaceInfo(i);
            }

			EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Reset Surface"))
            {
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Reset Surface");
					m_Target.surface = m_Target.SetSurfaceList ();
				}
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No textures detected", MessageType.Warning);
        }
    }

    private void ShowSurfaceInfo (int index)
    {
        EditorGUILayout.BeginHorizontal("Window", GUILayout.Height(64));
        GUILayout.Label(m_Target.surface[index].GetTexture(), GUILayout.Height(64), GUILayout.Width(64));

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        GUILayout.Label("Surface Type");

        EditorGUI.BeginChangeCheck();
        SurfaceType surface = (SurfaceType)EditorGUILayout.EnumPopup(m_Target.surface[index].surface);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Surface Type");
            m_Target.surface[index].surface = surface;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
    }
}
