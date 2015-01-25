using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject AmmoDisplay;
	public GameObject HitpointDisplay;

	private Text ammoText;
	private Image bulletDisplay;

	// Use this for initialization
	void Start () {
		ammoText = AmmoDisplay.GetComponent<Text> ();
		bulletDisplay = AmmoDisplay.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateAmmo(int ammoCount)
	{
		ammoText.text = ammoCount.ToString();
		bulletDisplay.rectTransform.sizeDelta = new Vector2(ammoCount * 16, 64);
	}

	public void reloadAnimate()
	{
		// Nothing as of now
	}
}
