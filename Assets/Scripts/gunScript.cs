using UnityEngine;
using System.Collections;

public class gunScript : MonoBehaviour {
	
	public int maxAmmo = 16;
	public int maxThrowables = 3;
	public int currentAmmo = 16;
	public int currentThrowables = 3;
	public float fireRate = 0.1f;
	public float reloadSpeed = 2.6f;
	public float throwSpeed = 8.0f;
	public float throwRate = 1.5f;
	public float gunRightOffset = 0.5f;

	public UIManager CanvasUIManager;
	
	public bool reloading = false;
	public bool firing = false;
	public bool throwing = false;
	
	public AudioClip gunshotSound;
	public AudioClip reloadSound;
	public AudioClip dryfireSound;
	public AudioClip throwSound;
	
	public GameObject bulletTracerPrefab;
	public GameObject throwablePrefab;
	
	void Start ()
	{
		CanvasUIManager = (UIManager) GameObject.FindGameObjectWithTag("UIManager").GetComponent(typeof(UIManager));
		CanvasUIManager.updateThrowables(currentThrowables);
		CanvasUIManager.updateAmmo(currentAmmo);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (CanvasUIManager.getUIMode() == 1) {
			if (Input.GetMouseButton (0))
			{
				if (!(firing || reloading))
				{
					fireProjectile ();
				}
			}
			
			if (Input.GetKey ("r"))
			{
				if (!(firing || reloading))
				{
					reload ();
				}
			}

			if (Input.GetKey ("f"))
			{
				if (!(throwing))
				{
					throwProjectile ();
				}
			}
		}
	}
	
	void fireProjectile()
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
		GameObject tracerBullet = Network.Instantiate(bulletTracerPrefab, transform.position + transform.right * gunRightOffset, transform.rotation, 0) as GameObject;
		
		// auto destroy bullet
		Destroy(tracerBullet, 1);
		
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

	void throwProjectile()
	{
		if (currentThrowables > 0)
		{
			throwing = true;
			shootThrowable();
			AudioSource.PlayClipAtPoint(throwSound, transform.position);
			currentThrowables--;
			CanvasUIManager.updateThrowables(currentThrowables);
			StartCoroutine(throwCooldown());
		}
	}

	void shootThrowable() {
		// instantiate bullet prefab
		GameObject throwable = Network.Instantiate(throwablePrefab, transform.position + transform.right * gunRightOffset, transform.rotation, 0) as GameObject;
		
		//ignore collision with player and give it velocity in the players forward direction
		Physics.IgnoreCollision(throwable.collider, transform.collider);
		throwable.transform.forward = transform.forward;
		throwable.rigidbody.AddForce(throwSpeed/2 * throwable.transform.forward, ForceMode.VelocityChange);
		throwable.rigidbody.AddForce(throwSpeed/2 * throwable.transform.up, ForceMode.VelocityChange);
	}

	IEnumerator throwCooldown()
	{
		yield return new WaitForSeconds(throwRate);
		throwing = false;
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
