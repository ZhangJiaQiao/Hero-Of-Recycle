using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponsManager))]
public class WeaponManagerEditor : Editor 
{
	WeaponsManager m_Target;

	public override void OnInspectorGUI()
	{
		m_Target = (target as WeaponsManager);

        EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck();
		MoveController controller = (MoveController)EditorGUILayout.ObjectField("Player", m_Target.controller, typeof(MoveController), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Player");
			m_Target.controller = controller;
		}

		EditorGUI.BeginChangeCheck();
		Camera mainCamera = (Camera)EditorGUILayout.ObjectField("Main Camera", m_Target.mainCamera, typeof(Camera), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Main Camera");
			m_Target.mainCamera = mainCamera;
		}

		EditorGUI.BeginChangeCheck();
		PlayerUI ui = (PlayerUI)EditorGUILayout.ObjectField("UI", m_Target.UI, typeof(PlayerUI), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Main Camera");
			m_Target.UI = ui;
		}

		EditorGUI.BeginChangeCheck();
		float interactionRange = EditorGUILayout.FloatField("Interaction Range", m_Target.interactionRange);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Interaction Range");
			m_Target.interactionRange = interactionRange;
		}
			
        if (m_Target.weaponEquipped.Count > m_Target.maxWeapons)
        {
            for (int i = m_Target.weaponEquipped.Count; i > m_Target.maxWeapons; i--)
            {
                m_Target.weaponEquipped.Remove(m_Target.weaponEquipped[i - 1]);
            }
        }

        DrawWeaponEquipped ();

		EditorGUI.BeginChangeCheck();
		int maxWeapons = EditorGUILayout.IntSlider("Max Weapons Selected", m_Target.maxWeapons, 0, 10);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Max Weapons Selected");
			m_Target.maxWeapons = maxWeapons;
		}

		DrawWeaponList ();

		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button ("Add")) 
		{
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Weapon List");
				m_Target.weaponList.Add (null);
			}
		}
    }

	private void DrawWeaponEquipped ()
	{
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("Weapons Equipped", EditorStyles.boldLabel);

        if (m_Target.weaponEquipped.Count == 0)
        {
            EditorGUILayout.HelpBox("No weapon selected", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < m_Target.weaponEquipped.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField(m_Target.weaponEquipped[i].weaponName + "\t(ID = " + m_Target.weaponEquipped[i].weaponId + ")");

				EditorGUI.BeginChangeCheck();
                if (GUILayout.Button("Unequip"))
                {
					if (EditorGUI.EndChangeCheck ())
					{
						Undo.RecordObject (target, "Changed Weapon Equipped List");
						m_Target.weaponEquipped.Remove (m_Target.weaponEquipped [i]);
					}
                }
                EditorGUILayout.EndHorizontal();
            }
        }
	}

	private void DrawWeaponList ()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Weapon List", EditorStyles.boldLabel);

		for (int i = 0; i < m_Target.weaponList.Count; i++) 
		{
			EditorGUILayout.BeginHorizontal("box");
			m_Target.weaponList[i] = (Weapon)EditorGUILayout.ObjectField (m_Target.weaponList[i], typeof(Weapon), true);

            if (m_Target.weaponList[i] != null && m_Target.weaponEquipped.Count < m_Target.maxWeapons)
            {
                if (!isEquipped(m_Target.weaponList[i].weaponId))
                {
                    EditorGUI.BeginChangeCheck();
                    if (GUILayout.Button("Equip"))
                    {
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(target, "Changed Weapon Equipped List");
                            m_Target.weaponEquipped.Add(m_Target.weaponList[i]);
                        }
                    }
                }
            }
            
			EditorGUI.BeginChangeCheck();
            if (GUILayout.Button ("Remove"))
			{
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Weapon Removed");
					m_Target.weaponEquipped.Remove (m_Target.weaponList [i]);
					m_Target.weaponList.Remove (m_Target.weaponList [i]);
				}
			}
            EditorGUILayout.EndHorizontal ();
		}
	}

    private bool isEquipped(int id)
    {
        for (int i = 0; i < m_Target.weaponEquipped.Count; i++)
        {
            if (m_Target.weaponEquipped[i].weaponId == id)
            {
                return true;
            }
        }
        return false;
    }
}
