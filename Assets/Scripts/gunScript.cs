using UnityEngine;
using System.Collections;

public class gunScript : MonoBehaviour {
	
	public int maxAmmo = 16;
	public int currentAmmo = 16;
	public float fireRate = 0.1f;
	public float reloadSpeed = 3.0f;
	// public float bulletSpeed = 30f;
	public float gunRightOffset = 0.5f;

	public GameObject UICanvas;
	public UIManager CanvasUIManager;

	public bool reloading = false;
	public bool firing = false;

	public AudioClip gunshotSound;
	public AudioClip reloadSound;
	public AudioClip dryfireSound;

	public GameObject smokeObjectPrefab;

	void Start ()
	{
		CanvasUIManager = (UIManager) GameObject.FindGameObjectWithTag("UIManager").GetComponent(typeof(UIManager));
	}

	// Update is called once per frame
	void Update ()
	{
		if (!(CanvasUIManager.isInMenu ())) {
			if (Input.GetMouseButton (0))
			{
				if (!(firing || reloading))
				{
					fire ();
				}
			}

			if (Input.GetKey ("r"))
			{
				if (!(firing || reloading))
				{
					reload ();
				}
			}
		}
	}

	void fire()
	{
		if (currentAmmo > 0)
		{
			firing = true;
			shootBullet();
			AudioSource.PlayClipAtPoint(gunshotSound, transform.position);
			currentAmmo--;
			CanvasUIManager.updateAmmo(currentAmmo);
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
		// instantiate bullet prefab
		GameObject bulletSmoke = Network.Instantiate(smokeObjectPrefab, transform.position + transform.right * gunRightOffset, transform.rotation, 0) as GameObject;
		
		// ignore collision with player and give it velocity in the players forward direction
		// Physics.IgnoreCollision(bulletSmoke.collider, transform.collider);
		// bulletSmoke.transform.forward = transform.forward;
		// bulletSmoke.rigidbody.AddForce(bulletSpeed * transform.forward, ForceMode.VelocityChange);
		
		// auto destroy bullet
		Destroy(bulletSmoke, 1);
		
		// raycast for hit collision
		RaycastHit hitInfo;
		if (Physics.Raycast (transform.position, transform.forward, out hitInfo, 100f))
		{
			if (hitInfo.collider.tag == "Player")
			{
				// do player hit logic here
				print("Player hit!");
			}
		}
	}
	
	IEnumerator fireCooldown()
	{
		yield return new WaitForSeconds(fireRate);
		firing = false;
	}

	void reload()
	{
		reloading = true;
		CanvasUIManager.reloadAnimate();
		AudioSource.PlayClipAtPoint(reloadSound, transform.position);
		StartCoroutine(reloadCooldown());
	}

	IEnumerator reloadCooldown()
	{
		yield return new WaitForSeconds(reloadSpeed);
		currentAmmo = maxAmmo;
		CanvasUIManager.updateAmmo(currentAmmo);
		reloading = false;
	}

}
