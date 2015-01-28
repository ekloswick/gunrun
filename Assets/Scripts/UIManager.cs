using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
	
	public GameObject ammoCount;
	public GameObject ammoDisplay;
	public GameObject throwableCount;
	public GameObject throwableDisplay;

	public GameObject HitpointDisplay;
	public GameObject canvasNetwork;
	public GameObject canvasGame;

	private Text ammoText;
	private Image ammoImage;
	private Text throwableText;
	private Image throwableImage;
	private bool menu = true;

	// Use this for initialization
	void Start ()
	{
		canvasGame.SetActive (false);
		ammoText = (Text) ammoCount.GetComponent(typeof(Text));
		ammoImage = (Image) ammoDisplay.GetComponent(typeof(Image));
		throwableText = (Text) throwableCount.GetComponent(typeof(Text));
		throwableImage = (Image) throwableDisplay.GetComponent(typeof(Image));
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
		ammoText.text = ammoCount.ToString();
		ammoImage.rectTransform.sizeDelta = new Vector2(ammoCount * 16, 64);
	}

	public void updateThrowables(int throwableCount)
	{
		throwableText.text = throwableCount.ToString();
		throwableImage.rectTransform.sizeDelta = new Vector2(throwableCount * 32, 64);
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
