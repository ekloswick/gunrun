using UnityEngine;
using System.Collections;

public class gunScript : MonoBehaviour {
	
	public int maxAmmo = 16;
	public int currentAmmo = 16;
	public float fireRate = 0.1f;
	public float reloadSpeed = 3.0f;

	public GameObject UICanvas;
	public UIManager CanvasUIManager;

	public bool reloading = false;
	public bool firing = false;

	public AudioClip gunshotSound;
	public AudioClip reloadSound;
	public AudioClip dryfireSound;

	// Use this for initialization
	void Start () {
		UICanvas = GameObject.FindGameObjectWithTag ("UIManager");
		CanvasUIManager = UICanvas.GetComponent<UIManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton (0))
		{
			if(!(firing || reloading))
			{
				fire ();
			}
		} 

		if (Input.GetKey ("r"))
		{
			if(!(firing || reloading))
			{
				reload ();
			}
		}
	}

	void fire() {
		if(currentAmmo > 0)
		{
			firing = true;
			shootBullet();
			AudioSource.PlayClipAtPoint(gunshotSound, transform.position);
			currentAmmo--;
			//CanvasUIManager.GetComponent<UIManager>().updateAmmo(currentAmmo);
			StartCoroutine(fireCooldown());
		}
		else
		{
			firing = true;
			AudioSource.PlayClipAtPoint(dryfireSound, transform.position);
			StartCoroutine(fireCooldown());
		}
	}

	void shootBullet()
	{
		// Do bullet things
	}

	IEnumerator fireCooldown()
	{
		yield return new WaitForSeconds(fireRate);
		firing = false;
	}

	void reload() {
		reloading = true;
		//CanvasUIManager.GetComponent<UIManager>().reloadAnimate();
		AudioSource.PlayClipAtPoint(reloadSound, transform.position);
		StartCoroutine(reloadCooldown());
	}

	IEnumerator reloadCooldown()
	{
		yield return new WaitForSeconds(reloadSpeed);
		currentAmmo = maxAmmo;
		//CanvasUIManager.GetComponent<UIManager>().updateAmmo(currentAmmo);
		reloading = false;
	}

}
