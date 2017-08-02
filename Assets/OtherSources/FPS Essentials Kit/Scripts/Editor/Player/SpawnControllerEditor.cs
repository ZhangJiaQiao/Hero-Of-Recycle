using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnController))]
public class SpawnControllerEditor : Editor
{
	SpawnController m_Target;

	[MenuItem("Tools/Spawn Controller")]
	private static void NewSpawnController ()
	{
		GameObject spawnController = new GameObject ("Spawn Controller");
		spawnController.AddComponent<SpawnController>();
	}

	private void OnSceneGUI ()
	{
		if (m_Target == null)
			m_Target = (target as SpawnController);
		
		Handles.color = Color.yellow;
		Handles.ArrowCap(0, m_Target.transform.position + Vector3.up / 2, m_Target.transform.rotation, 1.0f);
	}

	public override void OnInspectorGUI()
	{
		m_Target = (target as SpawnController);

		if (m_Target.gameObject.name != "Spawn Controller")
			m_Target.gameObject.name = "Spawn Controller";

		EditorGUILayout.Space ();

		if (m_Target.Inside ())
		{
			EditorGUILayout.HelpBox("Unable to instantiate the player", MessageType.Error);
			EditorGUILayout.Space ();

			m_Target.WireCubeColor = Color.red;
		} 
		else
		{
			m_Target.WireCubeColor = Color.green;
		}

		EditorGUI.BeginChangeCheck();
		GameObject player = (GameObject)EditorGUILayout.ObjectField ("Player", m_Target.player, typeof(GameObject), false);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Player");
			m_Target.player = player;
		}

		if (m_Target.player == null)
		{
			EditorGUILayout.HelpBox("You must assign a player to instantiate", MessageType.Warning);
		}

		if (m_Target.SetWeaponsManager ())
		{
			if (m_Target.weaponEquipped.Count > m_Target.weaponsManager.maxWeapons)
			{
				for (int i = m_Target.weaponEquipped.Count; i > m_Target.weaponsManager.maxWeapons; i--)
				{
					m_Target.weaponEquipped.Remove(m_Target.weaponEquipped[i - 1]);
				}
			}

			DrawWeaponEquipped ();

			EditorGUI.BeginChangeCheck();
			int maxWeapons = EditorGUILayout.IntSlider("Max Weapons Selected", m_Target.weaponsManager.maxWeapons, 0, 10);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Max Weapons Selected");
				m_Target.weaponsManager.maxWeapons = maxWeapons;
			}

			DrawWeaponList ();
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

		for (int i = 0; i < m_Target.weaponsManager.weaponList.Count; i++) 
		{
			EditorGUILayout.BeginHorizontal("box");
			EditorGUILayout.LabelField(m_Target.weaponsManager.weaponList[i].weaponName + "\t(ID = " + m_Target.weaponsManager.weaponList[i].weaponId + ")");

			if (m_Target.weaponsManager.weaponList[i] != null && m_Target.weaponEquipped.Count < m_Target.weaponsManager.maxWeapons)
			{
                if (!isEquipped(m_Target.weaponsManager.weaponList[i].weaponId))
                {
                    EditorGUI.BeginChangeCheck();
                    if (GUILayout.Button("Equip"))
                    {
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(target, "Changed Weapon Equipped List");
                            m_Target.weaponEquipped.Add(m_Target.weaponsManager.weaponList[i]);
                        }
                    }
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
