using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponsAnimations))]
public class WeaponAnimationsEditor : Editor 
{
	WeaponsAnimations m_Target;
	public override void OnInspectorGUI()
	{
		m_Target = (target as WeaponsAnimations);

		EditorGUILayout.Space();
		Base ();
		Move ();
		Draw ();
		Fire ();
		Reload ();
        Melee();
        Vault();
		EditorGUILayout.Space();
    }

    private void Vault()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool vault = EditorGUILayout.Toggle("Vault", m_Target.vault);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Vault");
			m_Target.vault = vault;
		}

        if (m_Target.vault)
        {
			EditorGUI.BeginChangeCheck();
			string vaultAnim =  EditorGUILayout.TextField("Vault Animation", m_Target.vaultAnim);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Vault Animation");
				m_Target.vaultAnim = vaultAnim;
			}

			EditorGUI.BeginChangeCheck();
			AudioClip vaultSound = (AudioClip)EditorGUILayout.ObjectField("Vault Sound", m_Target.vaultSound, typeof(AudioClip), false);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Vault Sound");
				m_Target.vaultSound = vaultSound;
			}

			if (m_Target.vaultSound != null)
			{
				EditorGUI.BeginChangeCheck();
				float vaultVolume = EditorGUILayout.Slider("Volume", m_Target.vaultVolume, 0, 1);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Vault Volume");
					m_Target.vaultVolume = vaultVolume;
				}
			}     
        }
        EditorGUILayout.EndVertical();
    }

    private void Melee ()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool melee = EditorGUILayout.Toggle("Melee", m_Target.melee);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Melee");
			m_Target.melee = melee;
		}

        if (m_Target.melee)
        {
			EditorGUI.BeginChangeCheck();
			string meleeAnim = EditorGUILayout.TextField("Melee Animation", m_Target.meleeAnim);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Melee Animation");
				m_Target.meleeAnim = meleeAnim;
			}

			EditorGUI.BeginChangeCheck();
			AudioClip meleeSound = (AudioClip)EditorGUILayout.ObjectField("Melee Sound", m_Target.meleeSound, typeof(AudioClip), false);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Melee Sound");
				m_Target.meleeSound = meleeSound;
			}

			if (m_Target.meleeSound != null)
			{
				EditorGUI.BeginChangeCheck();
				float meleeVolume = EditorGUILayout.Slider("Volume", m_Target.meleeVolume, 0, 1);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Melee Volume");
					m_Target.meleeVolume = meleeVolume;
				}
			}  
        }
        EditorGUILayout.EndVertical();
    }

	private void Move ()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		GameObject moveAnimations = (GameObject)EditorGUILayout.ObjectField("Move Animations", m_Target.moveAnimations, typeof(GameObject), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Move Animations");
			m_Target.moveAnimations = moveAnimations;
		}

		EditorGUI.BeginChangeCheck();
		string runAnimationName = EditorGUILayout.TextField("Run Animation Name", m_Target.runAnimationName);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Run Animation Name");
			m_Target.runAnimationName = runAnimationName;
		}

		EditorGUILayout.Space ();

		EditorGUILayout.BeginVertical("box");

		EditorGUI.BeginChangeCheck();
		Vector3 runningPos =  EditorGUILayout.Vector3Field("Position", m_Target.runningPos);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Run Position");
			m_Target.runningPos = runningPos;
		}

		EditorGUI.BeginChangeCheck();
		Vector3 runningRot = EditorGUILayout.Vector3Field("Rotation", m_Target.runningRot);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Run Rotation");
			m_Target.runningRot = runningRot;
		}

		if (GUILayout.Button ("Set as Current Position & Rotation"))
		{
			m_Target.runningPos = m_Target.transform.localPosition;
			m_Target.runningRot = m_Target.transform.localRotation.eulerAngles;
		}
		EditorGUILayout.EndVertical();

		EditorGUI.BeginChangeCheck();
		float animSpeed =  EditorGUILayout.FloatField("Move Speed", m_Target.animSpeed);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Move Speed");
			m_Target.animSpeed = animSpeed;
		}
		EditorGUILayout.EndVertical();
	}

	private void Reload ()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool reload = EditorGUILayout.Toggle("Reload", m_Target.reload);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Reload");
			m_Target.reload = reload;
		}

		if (m_Target.reload)
		{
			if (m_Target.weapon.reloadStyle == ReloadStyle.Magazines)
			{
				EditorGUI.BeginChangeCheck();
				string normalReload = EditorGUILayout.TextField("Normal Reload", m_Target.normalReload);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Normal Reload");
					m_Target.normalReload = normalReload;
				}

				EditorGUI.BeginChangeCheck();
				AudioClip normalReloadSound = (AudioClip)EditorGUILayout.ObjectField("Normal Reload Sound", m_Target.normalReloadSound, typeof(AudioClip), false);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Normal Reload Sound");
					m_Target.normalReloadSound = normalReloadSound;
				}

				EditorGUILayout.Space ();

				EditorGUI.BeginChangeCheck();
				string completeReload = EditorGUILayout.TextField("Complete Reload", m_Target.completeReload);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Complete Reload");
					m_Target.completeReload = completeReload;
				}

				EditorGUI.BeginChangeCheck();
				AudioClip completeReloadSound = (AudioClip)EditorGUILayout.ObjectField("Complete Reload Sound", m_Target.completeReloadSound, typeof(AudioClip), false);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Complete Reload Sound");
					m_Target.completeReloadSound = completeReloadSound;
				}
			
				EditorGUILayout.Space ();
				if (m_Target.completeReloadSound != null || m_Target.normalReloadSound != null)
				{
					EditorGUI.BeginChangeCheck();
					float reloadVolume = EditorGUILayout.Slider("Volume", m_Target.reloadVolume, 0, 1);
					if (EditorGUI.EndChangeCheck ())
					{
						Undo.RecordObject (target, "Changed Reload Volume");
						m_Target.reloadVolume = reloadVolume;
					}
				}	
			}
			else if (m_Target.weapon.reloadStyle == ReloadStyle.BulletByBullet)
			{
				EditorGUI.BeginChangeCheck();
				string startReload = EditorGUILayout.TextField("Start Reload", m_Target.startReload);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Start Reload");
					m_Target.startReload = startReload;
				}

				EditorGUI.BeginChangeCheck();
				AudioClip startReloadSound = (AudioClip)EditorGUILayout.ObjectField("Start Reload Sound", m_Target.startReloadSound, typeof(AudioClip), false);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Start Reload Sound");
					m_Target.startReloadSound = startReloadSound;
				}

				EditorGUILayout.Space ();

				EditorGUI.BeginChangeCheck();
				string insert = EditorGUILayout.TextField("Insert", m_Target.insert);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Insert");
					m_Target.insert = insert;
				}

				EditorGUI.BeginChangeCheck();
				AudioClip insertSound = (AudioClip)EditorGUILayout.ObjectField("Insert Reload Sound", m_Target.insertSound, typeof(AudioClip), false);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Insert Reload Sound");
					m_Target.insertSound = insertSound;
				}

				EditorGUILayout.Space ();

				EditorGUI.BeginChangeCheck();
				string stopReload = EditorGUILayout.TextField("Stop Reload", m_Target.stopReload);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Stop Reload");
					m_Target.stopReload = stopReload;
				}

				EditorGUI.BeginChangeCheck();
				AudioClip stopReloadSound = (AudioClip)EditorGUILayout.ObjectField("Stop Reload Sound", m_Target.stopReloadSound, typeof(AudioClip), false);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Stop Reload Sound");
					m_Target.stopReloadSound = stopReloadSound;
				}

				EditorGUILayout.Space ();
				if (m_Target.startReloadSound != null || m_Target.insertSound != null || m_Target.stopReloadSound != null)
				{
					EditorGUI.BeginChangeCheck();
					float reloadVolume = EditorGUILayout.Slider("Volume", m_Target.reloadVolume, 0, 1);
					if (EditorGUI.EndChangeCheck ())
					{
						Undo.RecordObject (target, "Changed Reload Volume");
						m_Target.reloadVolume = reloadVolume;
					}
				}
			}
		}
		EditorGUILayout.EndVertical();
	}
		
	private void Fire ()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool fire = EditorGUILayout.Toggle("Shot", m_Target.fire);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Shot");
			m_Target.fire = fire;
		}

		if (m_Target.fire)
		{
			EditorGUI.BeginChangeCheck();
			string shot = EditorGUILayout.TextField("Shot Animation", m_Target.shot);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Shot Animation");
				m_Target.shot = shot;
			}

			EditorGUI.BeginChangeCheck();
			string aimedShot = EditorGUILayout.TextField("Aimed Shot Animation", m_Target.aimedShot);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Aimed Shot Animation");
				m_Target.aimedShot = aimedShot;
			}

			EditorGUI.BeginChangeCheck();
			AudioClip shotSound = (AudioClip)EditorGUILayout.ObjectField("Shot Sound", m_Target.shotSound, typeof(AudioClip), false);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Shot Sound");
				m_Target.shotSound = shotSound;
			}
			if (m_Target.shotSound != null)
			{
				EditorGUI.BeginChangeCheck();
				float shotVolume = EditorGUILayout.Slider("Volume", m_Target.shotVolume, 0, 1);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Shot Volume");
					m_Target.shotVolume = shotVolume;
				}
			}	
		}
		EditorGUILayout.EndVertical();
	}

	private void Draw ()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool drawHide = EditorGUILayout.Toggle("Draw / Hide", m_Target.drawHide);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Draw / Hide");
			m_Target.drawHide = drawHide;
		}

		if (m_Target.drawHide)
		{
			EditorGUILayout.Space ();

			EditorGUI.BeginChangeCheck();
			string drawAnim = EditorGUILayout.TextField("Draw Animation", m_Target.drawAnim);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Draw Animation");
				m_Target.drawAnim = drawAnim;
			}

			EditorGUI.BeginChangeCheck();
			AudioClip drawSound = (AudioClip)EditorGUILayout.ObjectField("Draw Sound", m_Target.drawSound, typeof(AudioClip), false);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Draw Sound");
				m_Target.drawSound = drawSound;
			}

			if (m_Target.drawSound != null)
			{
				EditorGUI.BeginChangeCheck();
				float drawVolume =  EditorGUILayout.Slider("Volume", m_Target.drawVolume, 0, 1);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Draw Volume");
					m_Target.drawVolume = drawVolume;
				}
			}
				
			EditorGUILayout.Space ();

			EditorGUI.BeginChangeCheck();
			string hideAnim = EditorGUILayout.TextField("Hide Animation", m_Target.hideAnim);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Hide Animation");
				m_Target.hideAnim = hideAnim;
			}

			EditorGUI.BeginChangeCheck();
			AudioClip hideSound = (AudioClip)EditorGUILayout.ObjectField("Hide Sound", m_Target.hideSound, typeof(AudioClip), false);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Hide Sound");
				m_Target.hideSound = hideSound;
			}

			if (m_Target.hideSound != null)
			{
				EditorGUI.BeginChangeCheck();
				float hideVolume = EditorGUILayout.Slider("Volume", m_Target.hideVolume, 0, 1);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Hide Volume");
					m_Target.hideVolume = hideVolume;
				}
			}
		}
		EditorGUILayout.EndVertical();
	}

	private void Base ()
	{
		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		AudioManager audioManager = (AudioManager)EditorGUILayout.ObjectField("Audio Manager", m_Target.audioManager, typeof(AudioManager), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Audio Manager");
			m_Target.audioManager = audioManager;
		}

		EditorGUI.BeginChangeCheck();
		Animation weaponAnimation = (Animation)EditorGUILayout.ObjectField("Animations", m_Target.weaponAnimation, typeof(Animation), true); 
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Animations");
			m_Target.weaponAnimation = weaponAnimation;
		}
	}
}
