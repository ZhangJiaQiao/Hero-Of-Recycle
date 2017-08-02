using UnityEngine;
using System.Collections.Generic;

// THIS SCRIPT IS FOR DEMONSTRATION ONLY.
// EACH GAME MUST HAVE YOUR OWN INTERFACE.
// USE THIS INSPIRATION SCRIPT TO CREATE YOUR PROPERTY INTERFACE :)

public enum CrosshairStyle
{
    None,
    Cross,
    Circle,
    Point
}

public class PlayerUI : MonoBehaviour
{
    public GUIStyle label1, label2, label3, labelWeapons;

    [Header("Weapons")]
    public Texture2D weaponAmmoBar;
    public Texture2D weaponBG;
    public Texture2D grenadeIcon;
    public Texture2D lifeIcon;
    public Texture2D lifeBar;
    public Texture2D lifeBarBG;

	[Header("Crosshair")]
    public Texture2D pointTex;
    public Texture2D circleTex;
    public Texture2D defaultTex;

	private CrosshairStyle crosshairStyle = CrosshairStyle.None;
    private float hitmarkAlpha = 0;

    [Header("Damage")]
    public Texture2D hitScreen;
    public Texture2D bloodScreenLow;
    public Texture2D bloodScreenMedium;
    public Texture2D bloodScreenHigh;

    [Header("Damage Indicator")]
    public bool damageIndicator = true;
    [Range(1, 10)]
    public int maxIndicators = 5;
    public Texture2D hitIndicator;
    public Texture2D circularIndicator;
    private float circularIndicatorAlpha;

    private List<DamageIndicator> indicatorList = new List<DamageIndicator>();

    private float hitAlpha;

    [Space()]
    public HealthController damController;
    public Camera playerCamera;
    [Range(0.5f, 1.5f)]
    public float uiScale = 1.0f;

	private int currentAmmo, magazineSize, magsRemaining, granades;
	private float spread;

	private bool showCrosshair;

	private Texture2D weaponIcon;

	// The color of life icon
    private Color lifeIconColor = Color.green;
	private Color crosshairColor = Color.white;

	private float ammoAlpha = 0;
	private string ammoInfo;
	private string weaponName;
	private string currentWeaponName;

	private bool showWeaponPickUpGUI = false;
	private bool showWeaponSwitchGUI = false;

	private bool showWeaponsGUI = true;
	
	// Update is called once per frame
	private void Update ()
    {
        if (damController.CurrentHP >= damController.maxHP * 0.75f)
        {
            lifeIconColor =  Color.Lerp(lifeIconColor, Color.green, 10 * Time.deltaTime);
        }
        else if (damController.CurrentHP < damController.maxHP * 0.75f && damController.CurrentHP >= damController.maxHP * 0.35f)
        {
            lifeIconColor =  Color.Lerp(lifeIconColor, Color.yellow, 10 * Time.deltaTime);
        }
        else
        {
            lifeIconColor =  Color.Lerp(lifeIconColor, Color.red, 10 * Time.deltaTime);
        }

        for (int i = 0; i < indicatorList.Count; i++)
        {
            if (indicatorList[i].indicatorAlpha > 0)
            {
                CalculateDamageIndicatorAngle(indicatorList[i]);
                indicatorList[i].indicatorAlpha -= Time.deltaTime;
            }
            else
            {
                indicatorList.RemoveAt(i);
            }
        }

        if (hitmarkAlpha > 0)
            hitmarkAlpha -= Time.deltaTime;

        if (ammoAlpha > 0)
			ammoAlpha -= Time.deltaTime;

        if (hitAlpha > 0)
            hitAlpha -= Time.deltaTime;

        if (circularIndicatorAlpha > 0)
            circularIndicatorAlpha -= Time.deltaTime;
    }

    private void OnGUI ()
    {
        label1.fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height / 22.5f / (2 - uiScale)), 16, 48);
        label2.fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height / 45 / (2 - uiScale)), 8, 24);
        label3.fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height / 45 / (2 - uiScale)), 8, 24);
		labelWeapons.fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height / 45 / (2 - uiScale)), 6, 18);

        // Generates a value to match the size of the screen to automatically adjust the UI
        float offset = Mathf.Clamp((Screen.height / 7.2f) / (2 - uiScale), 50, 150);

        ShowDamageScreen(offset);

        WeaponsGUI(offset);
        CrosshairGUI(offset);
		ShowAmmoGUI (offset);

        ShowWeaponPickUpGUI(offset, weaponName);
		ShowWeaponSwitchGUI(offset, currentWeaponName, weaponName);
    }

    private void ShowDamageScreen (float offset)
    {
        //Hit
        if (hitAlpha > 0)
        {
            GUI.color = new Color(1, 1, 1, hitAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), hitScreen);
        }

        //BloodScreen
        if (damController.CurrentHP < damController.maxHP)
        {
            GUI.color = new Color(1, 1, 1, BloodScreenAlpha(damController.maxHP));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bloodScreenLow);

            GUI.color = new Color(1, 1, 1, BloodScreenAlpha(damController.maxHP * 0.7f));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bloodScreenMedium);

            GUI.color = new Color(1, 1, 1, BloodScreenAlpha(damController.maxHP * 0.35f));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bloodScreenHigh);
        }

        float radius = 1.5f;
        float size = offset * 1.28f;

        float size2 = offset * 1.60f;

        GUI.color = new Color(1, 0, 0, circularIndicatorAlpha);
        GUI.DrawTexture(new Rect(Screen.width / 2 - size2 / 2 * radius, Screen.height / 2 - size2 / 2 * radius, size2 * radius, size2 * radius), circularIndicator);

        //Damage Indicator
        for (int i = 0; i < indicatorList.Count; i++)
        {
            GUI.color = new Color(1, 0, 0, indicatorList[i].indicatorAlpha);
            GUIUtility.RotateAroundPivot(indicatorList[i].indicatorAngle, new Vector2((Screen.width / 2), (Screen.height / 2)));
            GUI.DrawTexture(new Rect(Screen.width / 2 - size / 2, Screen.height / 2 - size * radius, size, size), hitIndicator);
            GUIUtility.RotateAroundPivot(0 - indicatorList[i].indicatorAngle, new Vector2((Screen.width / 2), (Screen.height / 2)));
        }

        GUI.color = new Color(1, 1, 1, 1);
    }

    private void CalculateDamageIndicatorAngle(DamageIndicator d)
    {
        Vector3 lhs = playerCamera.transform.forward;

        Vector3 rhs = d.damageDir - transform.position;
        rhs.y = 0;
        rhs.Normalize();

        d.indicatorAngle = Vector3.Cross(lhs, rhs).y > 0 ? (1 - Vector3.Dot(lhs, rhs)) * 90 : (1 - Vector3.Dot(lhs, rhs)) * -90;
    }

    private float BloodScreenAlpha(float start)
    {
        float a = 1 - (damController.maxHP * damController.CurrentHP) / (start * damController.maxHP);
        return (Mathf.Clamp(a, 0, 1));
    }

    private void ShowWeaponPickUpGUI (float offset, string weaponName)
	{
		if (showWeaponPickUpGUI)
		{
			GUI.color = new Color(1, 1, 1, 1);
			GUI.Label(new Rect(Screen.width/2 - offset * 0.145f, Screen.height - offset * 1.15f, offset * 0.32f, offset * 0.32f), 
			"PRESS <color=#ffff00ff>E</color> TO PICK UP \n <color=#ffff00ff>" + weaponName + "</color>", labelWeapons);
		}
	}

	public void ShowWeaponSwitchGUI (float resFactor, string currentWeaponName, string weaponName)
	{
		if (showWeaponSwitchGUI)
		{
			GUI.color = new Color(1, 1, 1, 1);
			GUI.Label(new Rect(Screen.width/2 - resFactor * 0.145f, Screen.height - resFactor * 1.15f, resFactor * 0.32f, resFactor * 0.32f),
			 "PRESS <color=#ffff00ff>E</color> TO SWAP \n <color=#ffff00ff>" + currentWeaponName 
			 + "</color> FOR THE <color=#ffff00ff>" + weaponName +"</color>", labelWeapons);
		}
	}

	private void ShowAmmoGUI (float resFactor)
	{
		GUI.color = new Color(1, 1, 0, ammoAlpha);
		GUI.Label(new Rect(Screen.width/2 - resFactor * 0.145f, Screen.height - resFactor * 0.8f, resFactor * 0.32f, resFactor * 0.32f), ammoInfo, labelWeapons);
	}

    private void CrosshairGUI (float resFactor)
    {
        float crossW = Mathf.Clamp(resFactor / 1000, 0.01f, 0.02f);
        float crossH = resFactor / 833.3f;

        if (showCrosshair) 
		{
			GUI.color = crosshairColor;
			if (crosshairStyle == CrosshairStyle.Circle)
			{
				float circleSize = 1.28f;
				GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor *  circleSize/2 * spread, Screen.height / 2 - resFactor * circleSize/2 * spread, resFactor * circleSize * spread, resFactor * circleSize * spread), circleTex);
			}
			else if (crosshairStyle == CrosshairStyle.Cross)
			{
				//Horizontal
				GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH/2) - (resFactor * spread), Screen.height / 2 - resFactor * crossW/2, resFactor * crossH, resFactor * crossW), defaultTex);
				GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH/2) + (resFactor * spread), Screen.height / 2 - resFactor * crossW/2, resFactor * crossH, resFactor * crossW), defaultTex);

				//Vertical
				GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW/2, (Screen.height / 2 - resFactor * crossH/2) - (resFactor * spread), resFactor * crossW, resFactor * crossH), defaultTex);
				GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW/2, (Screen.height / 2 - resFactor * crossH/2) + (resFactor * spread), resFactor * crossW, resFactor * crossH), defaultTex);
			}
			else if (crosshairStyle == CrosshairStyle.Point)
			{
				float pointSize = 0.075f;
				GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * pointSize, Screen.height / 2 - resFactor * pointSize, resFactor * pointSize, resFactor * pointSize), pointTex);
			}
        }

        // Hit mark
        if (hitmarkAlpha > 0)
        {
            GUI.color = new Color(1, 1, 1, hitmarkAlpha);
            GUIUtility.RotateAroundPivot(45, new Vector2((Screen.width / 2), (Screen.height / 2)));

            //Horizontal
            GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH / 2) - (resFactor * 0.2f), Screen.height / 2 - resFactor * crossW / 2, resFactor * crossH, resFactor * crossW), defaultTex);
            GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH / 2) + (resFactor * 0.2f), Screen.height / 2 - resFactor * crossW / 2, resFactor * crossH, resFactor * crossW), defaultTex);

            //Vertical
            GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW / 2, (Screen.height / 2 - resFactor * crossH / 2) - (resFactor * 0.2f), resFactor * crossW, resFactor * crossH), defaultTex);
            GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW / 2, (Screen.height / 2 - resFactor * crossH / 2) + (resFactor * 0.2f), resFactor * crossW, resFactor * crossH), defaultTex);
            GUIUtility.RotateAroundPivot(-45, new Vector2((Screen.width / 2), (Screen.height / 2)));
            GUI.color = new Color(1, 1, 1, 1);
        }
    }

    private void WeaponsGUI(float resFactor)
    {
		if (showWeaponsGUI)
		{
			// Weapons Background
			GUI.DrawTexture(new Rect(Screen.width - resFactor * 3.2f, Screen.height - resFactor * 1.28f, resFactor * 2.56f, resFactor * 0.64f), weaponBG);

			// Weapons Ammo Bar
			GUI.BeginGroup(new Rect(Screen.width - resFactor * 3.2f, Screen.height - resFactor * 1.28f, currentAmmo * (resFactor / magazineSize) * 2.56f, resFactor * 0.64f));
			GUI.DrawTexture(new Rect(0, 0, resFactor * 2.56f, resFactor * 0.64f), weaponAmmoBar);
			GUI.EndGroup();

			// Current Ammo - Clips Remaining
			GUI.Label(new Rect(Screen.width - resFactor * 1.6f, Screen.height - resFactor * 1.05f, resFactor * 0.32f, resFactor * 0.32f), currentAmmo.ToString(), label1);
			GUI.Label(new Rect(Screen.width - resFactor * 1.1f, Screen.height - resFactor * 1f, resFactor * 0.32f, resFactor * 0.32f), magsRemaining.ToString(), label2);

			// Weapon Icon
			if (weaponIcon != null)
				GUI.DrawTexture(new Rect(Screen.width - resFactor * 2.9f, Screen.height - resFactor * 1.18f, resFactor * 0.96f, resFactor * 0.48f), weaponIcon);

			// Grenade Icon
			GUI.DrawTexture(new Rect(Screen.width - resFactor * 1.6f, Screen.height - resFactor * 1.25f, resFactor * 0.18f, resFactor * 0.18f), grenadeIcon);

			// Current granades
			GUI.Label(new Rect(Screen.width - resFactor * 1.48f, Screen.height - resFactor * 1.32f, resFactor * 0.32f, resFactor * 0.32f), granades.ToString(), label3);
		
			// Life bar Background
			GUI.DrawTexture(new Rect(Screen.width - resFactor * 2.65f, Screen.height - resFactor * 0.6f, resFactor * 2.01f, resFactor * 0.16f), lifeBarBG);

			// Life bar
			GUI.BeginGroup(new Rect(Screen.width - resFactor * 2.65f, Screen.height - resFactor * 0.6f, damController.CurrentHP * (resFactor / damController.maxHP) * 2.01f, resFactor * 0.16f));
			GUI.DrawTexture(new Rect(0, 0, resFactor * 2.01f, resFactor * 0.16f), lifeBar);
			GUI.EndGroup();

			// Life Icon
			GUI.color = lifeIconColor;
			GUI.DrawTexture(new Rect(Screen.width - resFactor * 2.9f, Screen.height - resFactor * 0.6f, resFactor * 0.48f, resFactor * 0.16f), lifeIcon);
			GUI.color = new Color(1, 1, 1, 1);
		}
    }

    public void ShowHitScreen ()
    {
        hitAlpha = 0.75f;
    }

    public void ShowHitMark()
    {
        hitmarkAlpha = 0.8f;
    }

    public void ShowAmmoInfo(string info)
    {
        ammoAlpha = 2;
        ammoInfo = info;
    }

    public void ShowDamageIndicator (Vector3 damageDirection)
    {
        if (damageIndicator)
        {
            if (indicatorList.Count > maxIndicators)
            {
                indicatorList.RemoveAt(0);
            }

            DamageIndicator d = new DamageIndicator(damageDirection, 3);
            indicatorList.Add(d);
        }
    }

    public void ShowCircularIndicator ()
    {
        circularIndicatorAlpha = 2;
    }

    public void ShowWeaponSwitch(bool show, string currentWeaponName, string weaponName)
	{
        showWeaponSwitchGUI = show;
		this.currentWeaponName = currentWeaponName;
		this.weaponName = weaponName;
	}

	public void ShowWeaponPickUp  (bool show, string weaponName)
	{
        showWeaponPickUpGUI = show;
		this.weaponName = weaponName;
	}

	public void ShowWeaponsGUI (bool show)
	{
		showWeaponsGUI = show;
	}

	public void SetCrosshairColor (Color c)
	{
		if (crosshairColor != c)
			crosshairColor = c;
	}

	public void ShowCrosshair (bool show)
	{
		if (showCrosshair != show)
		{
			showCrosshair = show;
		}
	}

	public void SetCrosshairType (CrosshairStyle c)
	{
		if (crosshairStyle != c)
			crosshairStyle = c;
	}
		
	public void SetSpread (float spread)
	{
		if (this.spread != spread)
			this.spread = Mathf.Clamp(spread * 1.5f, 0.15f, 1.5f);
	}

	public void SetWeaponProperties (int currentAmmo, int clipsSize, int clipsRemaining)
	{
		if (this.currentAmmo != currentAmmo || magsRemaining != clipsRemaining || magazineSize != clipsSize) 
		{
			this.currentAmmo = currentAmmo;
			magazineSize = clipsSize;
			magsRemaining = clipsRemaining;
		}
	}

	public void SetGranadesAmount (int amount)
	{
		if (granades != amount)
			granades = amount;
	}

	public void SetWeaponIcon (Texture2D tex)
	{
		if (weaponIcon != tex && tex != null)
			weaponIcon = tex;
	}
}