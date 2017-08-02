using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor 
{
	Weapon m_Target;

	private bool hide;

	public override void OnInspectorGUI()
	{
		m_Target = (target as Weapon);

		DrawWeaponBase ();
		DrawWeaponProperties ();
		DrawWeaponAccuracy ();
		DrawWeaponRecoil();
		DrawWeaponEffects ();
	}

    private void DrawWeaponEffects ()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

        EditorGUI.BeginChangeCheck();
        bool muzzleFlash = EditorGUILayout.Toggle("Muzzle Flash", m_Target.muzzleFlash);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Disabe/Enable Muzzle Flash");
            m_Target.muzzleFlash = muzzleFlash;
        }

		if (m_Target.muzzleFlash)
        {
            EditorGUI.BeginChangeCheck();
            GameObject muzzle = (GameObject)EditorGUILayout.ObjectField("Muzzle Flash GO", m_Target.muzzle, typeof(GameObject), true);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Muzzle Flash");
                m_Target.muzzle = muzzle;
            }
        }
			
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

        EditorGUI.BeginChangeCheck();
        bool smoke = EditorGUILayout.Toggle("Smoke", m_Target.smoke);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Disabe/Enable Smoke");
            m_Target.smoke = smoke;
        }

        if (m_Target.smoke)
		{
            EditorGUI.BeginChangeCheck();
            GameObject smokePrefab = (GameObject)EditorGUILayout.ObjectField("Smoke Particle", m_Target.smokePrefab, typeof(GameObject), false);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Smoke Particle");
                m_Target.smokePrefab = smokePrefab;
            }

            EditorGUI.BeginChangeCheck();
            Transform smokeTransform = (Transform)EditorGUILayout.ObjectField("Smoke Transform", m_Target.smokeTransform, typeof(Transform), true);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Smoke Transform");
                m_Target.smokeTransform = smokeTransform;
            }    
		}
	
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

        EditorGUI.BeginChangeCheck();
        bool tracer = EditorGUILayout.Toggle("Tracer", m_Target.tracer);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Disabe/Enable Tracer");
            m_Target.tracer = tracer;
        }   

		if (m_Target.tracer)
		{
			EditorGUI.BeginChangeCheck();
			GameObject tracerPrefab = (GameObject)EditorGUILayout.ObjectField("Tracer Prefab", m_Target.tracerPrefab, typeof(GameObject), false);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Tracer Prefab");
				m_Target.tracerPrefab = tracerPrefab;
			}

			EditorGUI.BeginChangeCheck();
			Transform tracerTransform = (Transform)EditorGUILayout.ObjectField("Tracer Transform", m_Target.tracerTransform, typeof(Transform), true);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Tracer Transform");
				m_Target.tracerTransform = tracerTransform;
			}

			EditorGUI.BeginChangeCheck();
			float tracerSpeed = EditorGUILayout.FloatField("Tracer Speed", m_Target.tracerSpeed);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Tracer Speed");
				m_Target.tracerSpeed = tracerSpeed;
			}
		}
	
		EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool shell = EditorGUILayout.Toggle("Shell", m_Target.shell);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Disabe/Enable Shell");
			m_Target.shell = shell;
		}   

        if (m_Target.shell)
        {
			EditorGUI.BeginChangeCheck();
			GameObject shellPrefab = (GameObject)EditorGUILayout.ObjectField("Shell Prefab", m_Target.shellPrefab, typeof(GameObject), false);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Shell Prefab");
				m_Target.shellPrefab = shellPrefab;
			}
				
			EditorGUI.BeginChangeCheck();
			Transform shellTransform = (Transform)EditorGUILayout.ObjectField("Shell Transform", m_Target.shellTransform, typeof(Transform), true);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Transform");
				m_Target.shellTransform = shellTransform;
			}
            
			EditorGUI.BeginChangeCheck();
			float delayToInstantiate = EditorGUILayout.FloatField("Delay To Instantiate", m_Target.delayToInstantiate);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Delay To Instantiate");
				m_Target.delayToInstantiate = delayToInstantiate;
			}

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Eject Force");

            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.BeginHorizontal("TextArea");
            GUILayout.Label("Min: " + m_Target.minEjectForce.ToString("F1"));
            EditorGUILayout.EndHorizontal();

			EditorGUI.BeginChangeCheck();
			float minEjectForce = m_Target.minEjectForce;
			float maxEjectForce = m_Target.maxEjectForce;
			EditorGUILayout.MinMaxSlider(ref minEjectForce, ref maxEjectForce, 0, 10);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Min/Max Eject Force");
				m_Target.minEjectForce = minEjectForce;
				m_Target.maxEjectForce = maxEjectForce;
			}

            EditorGUILayout.BeginHorizontal("TextArea");
            GUILayout.Label("Max: " + m_Target.maxEjectForce.ToString("F1"));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
		EditorGUILayout.Space ();
    }

	private void DrawWeaponRecoil()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool recoil = EditorGUILayout.Toggle("Recoil", m_Target.recoil);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject(target, "Disabe/Enable Recoil");
			m_Target.recoil = recoil;
		}

		if (m_Target.recoil)
		{
			EditorGUI.BeginChangeCheck();
			float recoilAmount = EditorGUILayout.FloatField("Recoil Intensity", m_Target.recoilIntensity);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Recoil Intensity");
				m_Target.recoilIntensity = recoilAmount;
			}

			EditorGUI.BeginChangeCheck();
			float recoilAmountAiming = EditorGUILayout.FloatField("Recoil Intensity (Aim)", m_Target.recoilAmountAiming);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Recoil Intensity (Aim)");
				m_Target.recoilAmountAiming = recoilAmountAiming;
			}
            
			EditorGUI.BeginChangeCheck();
			float recoilTimer = EditorGUILayout.Slider("Recoil Smooth", m_Target.recoilTimer, 0.1f, 1f);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Recoil Timer");
				m_Target.recoilTimer = recoilTimer;
			}

			EditorGUI.BeginChangeCheck();
			bool limitUpwardsRecoil = EditorGUILayout.Toggle("Limit Upwards Recoil", m_Target.limitUpwardsRecoil);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Disabe/Enable Limit Upwards Recoil");
				m_Target.limitUpwardsRecoil = limitUpwardsRecoil;
			}
			
			if (m_Target.limitUpwardsRecoil)
			{
				EditorGUI.BeginChangeCheck();
				float maxUpwardsRecoil = EditorGUILayout.FloatField("Max Upwards Recoil", m_Target.maxUpwardsRecoil);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Recoil Timer");
					m_Target.maxUpwardsRecoil = maxUpwardsRecoil;
				}
			}

            EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Sideways Recoil");

			EditorGUILayout.BeginHorizontal("box");

			EditorGUILayout.BeginHorizontal("TextArea");
			GUILayout.Label("Min: " + m_Target.minSidewaysRecoil.ToString("F1"));
			EditorGUILayout.EndHorizontal();

			EditorGUI.BeginChangeCheck();
			float minSidewaysRecoil = m_Target.minSidewaysRecoil;
			float maxSidewaysRecoil = m_Target.maxSidewaysRecoil;
			EditorGUILayout.MinMaxSlider(ref minSidewaysRecoil, ref maxSidewaysRecoil, -2, 2);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Changed Min/Max Sideways Recoil");
				m_Target.minSidewaysRecoil = minSidewaysRecoil;
				m_Target.maxSidewaysRecoil = maxSidewaysRecoil;
			}

			EditorGUILayout.BeginHorizontal("TextArea");
			GUILayout.Label("Max: " + m_Target.maxSidewaysRecoil.ToString("F1"));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space ();

			EditorGUI.BeginChangeCheck();
			bool weaponKickBack = EditorGUILayout.Toggle("Weapon Kick", m_Target.weaponKickBack);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject(target, "Disabe/Enable Weapon Kick");
				m_Target.weaponKickBack = weaponKickBack;
			}

			if (m_Target.weaponKickBack)
			{
				EditorGUI.BeginChangeCheck();
				float weaponKick = EditorGUILayout.Slider("Weapon Kick", m_Target.weaponKick, 0.0f, 5.0f);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject(target, "Changed Weapon Kick");
					m_Target.weaponKick = weaponKick;
				}
			}
        }
		EditorGUILayout.EndVertical();
	}

	private void DrawWeaponAccuracy()
	{
		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		float baseSpread =  EditorGUILayout.Slider("Base Spread", m_Target.baseSpread, 0.1f, 10.0f);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject(target, "Changed Base Spread");
			m_Target.baseSpread = baseSpread;
		}

		EditorGUI.BeginChangeCheck();
		float maxSpread =  EditorGUILayout.Slider("Max Spread", m_Target.maxSpread, m_Target.baseSpread, 10.0f);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Max Spread");
			m_Target.maxSpread = maxSpread;
		}

		EditorGUI.BeginChangeCheck();
		float baseSpreadCrouch = EditorGUILayout.Slider("Base Spread Crouched", m_Target.baseSpreadCrouch, 0.1f, m_Target.baseSpread);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Base Spread Crouched");
			m_Target.baseSpreadCrouch = baseSpreadCrouch;
		}
		
		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		float spreadWhenMove = EditorGUILayout.Slider("Spread by Step", m_Target.spreadWhenMove, 0.1f, 10.0f);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Spread by Step");
			m_Target.spreadWhenMove = spreadWhenMove;
		}

		EditorGUI.BeginChangeCheck();
		float spreadPerShot = EditorGUILayout.Slider("Spread per Shot", m_Target.spreadPerShot, 0.1f, 10.0f);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Spread per Shot");
			m_Target.spreadPerShot = spreadPerShot;
		}

		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		bool aimDownSights = EditorGUILayout.Toggle("Aim Down Sights", m_Target.aimDownSights);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Aim Down Sights");
			m_Target.aimDownSights = aimDownSights;
		}

		if (m_Target.aimDownSights)
		{
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical("box");

			EditorGUI.BeginChangeCheck();
			Vector3 aimPosition = EditorGUILayout.Vector3Field("Position", m_Target.aimPosition);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Aim Position");
				m_Target.aimPosition = aimPosition;
			}

			EditorGUI.BeginChangeCheck();
			Vector3 aimRotation = EditorGUILayout.Vector3Field("Rotation", m_Target.aimRotation);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Aim Rotation");
				m_Target.aimRotation = aimRotation;
			}

			if (GUILayout.Button ("Set as Current Position & Rotation"))
			{
				m_Target.aimPosition = m_Target.transform.localPosition;
				m_Target.aimRotation = m_Target.transform.localRotation.eulerAngles;
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.Space ();

			EditorGUI.BeginChangeCheck();
			float aimSpeed = EditorGUILayout.Slider("Aim Speed", m_Target.aimSpeed, 0.1f, 20.0f);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Aim Speed");
				m_Target.aimSpeed = aimSpeed;
			}

			EditorGUILayout.Space ();

			EditorGUI.BeginChangeCheck();
			float spreadAIM = EditorGUILayout.Slider("Spread When Aiming", m_Target.spreadAIM, 0.1f, m_Target.baseSpread);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Spread When Aiming");
				m_Target.spreadAIM = spreadAIM;
			}

			EditorGUILayout.Space ();

			EditorGUILayout.Space ();

			EditorGUI.BeginChangeCheck();
			bool zoom = EditorGUILayout.Toggle("Zoom", m_Target.zoom);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Disabe/Enable Zoom");
				m_Target.zoom = zoom;
			}

			if (m_Target.zoom)
			{
				EditorGUI.BeginChangeCheck();
				int zoomFov = EditorGUILayout.IntSlider("Zoom FOV", m_Target.zoomFov, 1, 179);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Zoom FOV");
					m_Target.zoomFov = zoomFov;
				}
			}

			EditorGUILayout.Space ();

			EditorGUI.BeginChangeCheck();
			bool scope = EditorGUILayout.Toggle("Rifle Scope", m_Target.scope);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Disabe/Enable Rifle Scope");
				m_Target.scope = scope;
			}

			if (m_Target.scope)
			{
				EditorGUI.BeginChangeCheck();
				RifleScope sniperScope = (RifleScope)EditorGUILayout.ObjectField("Rifle Scope GO", m_Target.rifleScope, typeof(RifleScope), true);
				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (target, "Changed Rifle Scope GO");
					m_Target.rifleScope = sniperScope;
				}

                EditorGUI.BeginChangeCheck();
                int zoomFov = EditorGUILayout.IntSlider("Rifle Scope FOV", m_Target.rifleScopeFOV, 1, 10);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Rifle Scope FOV");
                    m_Target.rifleScopeFOV = zoomFov;
                }
            }
		}
		EditorGUILayout.EndVertical();
	}

	private void DrawWeaponProperties()
	{
		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		FireMode fireMode = (FireMode)EditorGUILayout.EnumPopup ("Fire Mode", m_Target.fireMode);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Fire Mode");
			m_Target.fireMode = fireMode;
		}

		EditorGUI.BeginChangeCheck();
		ReloadStyle reloadStyle = (ReloadStyle)EditorGUILayout.EnumPopup("Reload Style", m_Target.reloadStyle);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Reload Style");
			m_Target.reloadStyle = reloadStyle;
		}

		EditorGUI.BeginChangeCheck();
		LayerMask cullingMask = EditorTools.LayerMaskField("Culling Mask", m_Target.cullingMask);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Culling Mask");
			m_Target.cullingMask = cullingMask;
		}

		EditorGUI.BeginChangeCheck();
		float fireRate = EditorGUILayout.FloatField("Fire Rate", m_Target.fireRate);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Fire Rate");
			m_Target.fireRate = fireRate;
		}

		EditorGUI.BeginChangeCheck();
		float shotForce = EditorGUILayout.FloatField("Force", m_Target.shotForce);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Fire Force");
			m_Target.shotForce = shotForce;
		}

		EditorGUI.BeginChangeCheck();
		float range = EditorGUILayout.FloatField("Range", m_Target.range);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Fire Range");
			m_Target.range = range;
		}

        EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck();
		DamageMode damageMode = (DamageMode)EditorGUILayout.EnumPopup("Damage Mode", m_Target.damageMode);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Damage Mode");
			m_Target.damageMode = damageMode;
		}

        EditorGUILayout.LabelField("Weapon Damage");

        EditorGUILayout.BeginHorizontal("box");

        EditorGUILayout.BeginHorizontal("TextArea");
        GUILayout.Label("Min: " + m_Target.minDamage.ToString("F0"));
        EditorGUILayout.EndHorizontal();

		EditorGUI.BeginChangeCheck();
		float minDamage = m_Target.minDamage;
		float maxDamage = m_Target.maxDamage;
		EditorGUILayout.MinMaxSlider(ref minDamage, ref maxDamage, 0, 100);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Min/Max Weapon Damage");
			m_Target.minDamage = minDamage;
			m_Target.maxDamage = maxDamage;
		}

        EditorGUILayout.BeginHorizontal("TextArea");
        GUILayout.Label("Max: " + m_Target.maxDamage.ToString("F0"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();


		EditorGUI.BeginChangeCheck();
		bool infiniteAmmo = EditorGUILayout.Toggle("Infine Ammo", m_Target.infiniteAmmo);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Infine Ammo");
			m_Target.infiniteAmmo = infiniteAmmo;
		}

		if (m_Target.fireMode == FireMode.ShotgunAuto || m_Target.fireMode == FireMode.ShotgunSemi)
		{
			EditorGUI.BeginChangeCheck();
			int pelletCount = EditorGUILayout.IntField("Bullets per Shot", m_Target.pelletCount);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Bullets per Shot");
				m_Target.pelletCount = pelletCount;
			}
		}
		else if (m_Target.fireMode == FireMode.Burst)
		{
			EditorGUI.BeginChangeCheck();
			int burstCount = EditorGUILayout.IntField("Burst Shot", m_Target.burstCount);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (target, "Changed Burst Shot");
				m_Target.burstCount = burstCount;
			}
		}

		EditorGUI.BeginChangeCheck();
		int magazineSize = EditorGUILayout.IntField("Bullets per Mag", m_Target.magazineSize);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Bullets per Mag");
			m_Target.magazineSize = magazineSize;
		}

		EditorGUI.BeginChangeCheck();
		int numberOfMags = EditorGUILayout.IntField("Number of Mags", m_Target.numberOfMags);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Number of Mags");
			m_Target.numberOfMags = numberOfMags;
		}

		EditorGUI.BeginChangeCheck();
		int maxNumberOfMags = EditorGUILayout.IntField("Max Number of Mags", m_Target.maxNumberOfMags);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Max Number of Mags");
			m_Target.maxNumberOfMags = maxNumberOfMags;
		}
    }

	private void DrawWeaponBase()
	{
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical("Window");

		EditorGUI.BeginChangeCheck();
		int weaponId = EditorGUILayout.IntField("Weapon ID", m_Target.weaponId);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Weapon ID");
			m_Target.weaponId = weaponId;
		}

		EditorGUI.BeginChangeCheck();
		string weaponName = EditorGUILayout.TextField("Weapon Name", m_Target.weaponName);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Weapon Name");
			m_Target.weaponName = weaponName;
		}

		if (m_Target.weaponName.Length > 0)
			m_Target.gameObject.name = m_Target.weaponName;
		else
			m_Target.gameObject.name = "Weapon Name";

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField ("Weapon Icon");

		EditorGUI.BeginChangeCheck();
		Texture2D weaponIcon = (Texture2D)EditorGUILayout.ObjectField(m_Target.weaponIcon, typeof(Texture2D), false);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Weapon Icon");
			m_Target.weaponIcon = weaponIcon;
		}

		EditorGUILayout.EndHorizontal();

		EditorGUI.BeginChangeCheck();
		GameObject droppablePrefab = (GameObject)EditorGUILayout.ObjectField("Droppable Prefab", m_Target.droppablePrefab, typeof(GameObject), false);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Droppable Prefab");
			m_Target.droppablePrefab = droppablePrefab;
		}

        EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		float weight = EditorGUILayout.Slider("Weapon Weight", m_Target.weight, 0, 2.5f);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Weapon Weight");
			m_Target.weight = weight;
		}

		EditorGUI.BeginChangeCheck();
		CrosshairStyle crosshairStyle = (CrosshairStyle)EditorGUILayout.EnumPopup("Crosshair Style", m_Target.crosshairStyle);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Crosshair Style");
			m_Target.crosshairStyle = crosshairStyle;
		}

		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck();
		bool melee = EditorGUILayout.Toggle("Melee Attack", m_Target.melee);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Disabe/Enable Melee Attack");
			m_Target.melee = melee;
		}

        EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck();
		Camera mainCamera = (Camera)EditorGUILayout.ObjectField("Main Camera", m_Target.mainCamera, typeof(Camera), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Main Camera");
			m_Target.mainCamera = mainCamera;
		}

		EditorGUI.BeginChangeCheck();
		MoveController controller = (MoveController)EditorGUILayout.ObjectField("Player", m_Target.controller, typeof(MoveController), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Player");
			m_Target.controller = controller;
		}

		EditorGUI.BeginChangeCheck();
		CameraAnimations cameraAnimations = (CameraAnimations)EditorGUILayout.ObjectField("Camera Animations", m_Target.cameraAnimations, typeof(CameraAnimations), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed Camera Animations");
			m_Target.cameraAnimations = cameraAnimations;
		}

		EditorGUI.BeginChangeCheck();
		PlayerUI ui = (PlayerUI)EditorGUILayout.ObjectField("UI", m_Target.UI, typeof(PlayerUI), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Changed UI");
			m_Target.UI = ui;
		}

		EditorGUI.BeginChangeCheck();
		BulletMarkManager bulletMark = (BulletMarkManager)EditorGUILayout.ObjectField("Bullet Marks Manager", m_Target.bulletMark, typeof(BulletMarkManager), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Bullet Marks Manager");
			m_Target.bulletMark = bulletMark;
		}

		EditorGUI.BeginChangeCheck();
		WeaponsManager weaponManager = (WeaponsManager)EditorGUILayout.ObjectField("Weapons Manager", m_Target.weaponManager, typeof(WeaponsManager), true);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Bullet Weapons Manager");
			m_Target.weaponManager = weaponManager;
		}

		EditorGUILayout.Space ();
		if (GUILayout.Button("Enable / Disabe"))
		{
			if (hide)
				hide = false;
			else
				hide = true;
			m_Target.DisableRenders(hide);
		}

		EditorGUILayout.EndVertical();
	}
}