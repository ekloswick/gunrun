using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
	
	public GameObject canvasBrowser;
	public GameObject canvasGame;
	public GameObject canvasServer;

	// 0 = Browser/Main
	// 1 = Player
	// 2 = Server
	//-1 = No UI
	public int UIMode = 1;

	// UI-specific vars
	public GameObject ammoCount;
	public GameObject ammoDisplay;
	public GameObject throwableCount;
	public GameObject throwableDisplay;
	public GameObject hitpointCount;
	public GameObject hitpointDisplay;

	private Text ammoText;
	private Image ammoImage;
	private Text throwableText;
	private Image throwableImage;
	private Text hitpointText;
	private Image hitpointImage;

	// Use this for initialization
	void Start ()
	{
		this.setUIMode(0);
		ammoText = (Text) ammoCount.GetComponent(typeof(Text));
		ammoImage = (Image) ammoDisplay.GetComponent(typeof(Image));
		throwableText = (Text) throwableCount.GetComponent(typeof(Text));
		throwableImage = (Image) throwableDisplay.GetComponent(typeof(Image));
		hitpointText = (Text) hitpointCount.GetComponent(typeof(Text));
		hitpointImage = (Image) hitpointDisplay.GetComponent(typeof(Image));
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch(UIMode)
		{
			// If in browser view...
			case 0:
			{
				HandleBrowserInput();
			}
			break;

			// If in player view...
			case 1: 
			{
				HandlePlayerInput();
			}
			break;

			// If in server view...
			case 2: 
			{
				HandleServerInput();
			}
			break;
		
			// All other cases...
			default:
			{

			}
			break;
		}
	}

	// UIMode 0 - Server Browser
	void HandleBrowserInput()
	{
		// hides server browser
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			setUIMode(1);
		}
	}
	
	// UIMode 1 - Player View
	void HandlePlayerInput()
	{
		// shows server browser
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			setUIMode(0);
		}
	}
	
	// UIMode 2 - Server View
	void HandleServerInput()
	{
	
	}

	public void setUIMode(int mode)
	{
		UIMode = mode;
		switch(mode) {
			case 0:
			{
				canvasBrowser.SetActive(true);
				canvasGame.SetActive (false);
				canvasServer.SetActive(false);
			} break;
			case 1:
			{
				canvasBrowser.SetActive(false);
				canvasGame.SetActive (true);
				canvasServer.SetActive(false);
			} break;
			case 2:
			{
				canvasBrowser.SetActive(false);
				canvasGame.SetActive (false);
				canvasServer.SetActive(true);
			} break;
			default:
			{
			
			} break;
		}
	}
	
	public int getUIMode()
	{
		return UIMode;
	}


	// UI-specific methods

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

	public void updateHitpoints(int hitpointCount)
	{
		hitpointText.text = hitpointCount.ToString();
		hitpointImage.rectTransform.sizeDelta = new Vector2(hitpointCount * 16, 64);
	}

	public void reloadAnimate()
	{
		// Nothing as of now
	}
}
