using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
	
	public GameObject ammoDisplay;
	public GameObject bulletDisplay;
	public GameObject HitpointDisplay;
	public GameObject canvasNetwork;
	public GameObject canvasGame;

	private Text ammoboxText;
	private Image ammoboxBullets;
	private bool menu = true;

	// Use this for initialization
	void Start ()
	{
		canvasGame.SetActive (false);
		ammoboxText = (Text) ammoDisplay.GetComponent(typeof(Text));
		ammoboxBullets = (Image) bulletDisplay.GetComponent(typeof(Image));
	}
	
	// Update is called once per frame
	void Update ()
	{
		// shows/hides server browser
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (menu == false)
			{
				canvasNetwork.SetActive(true);
				canvasGame.SetActive (false);
				menu = true;
			}
			else
			{
				canvasNetwork.SetActive(false);
				canvasGame.SetActive (true);
				menu = false;
			}
		}
	}

	public void updateAmmo(int ammoCount)
	{
		ammoboxText.text = ammoCount.ToString();
		ammoboxBullets.rectTransform.sizeDelta = new Vector2(ammoCount * 16, 64);
	}

	public void reloadAnimate()
	{
		// Nothing as of now
	}

	public bool isInMenu()
	{
		return menu;
	}
}
