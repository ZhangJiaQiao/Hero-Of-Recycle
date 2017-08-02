using UnityEngine;

public class HideCursor : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		hide (true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			hide (false);

		if (Input.GetKeyDown (KeyCode.Mouse0))
			hide (true);
	}

	private void hide (bool hide)
	{
		if (hide) 
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} 
		else 
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
