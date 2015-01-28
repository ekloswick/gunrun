using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour {

	public float fuseTime = 3f;
	public GameObject explosionPrefab;
	public AudioClip explosionSound;

	// Use this for initialization
	void Start () {
		StartCoroutine(detonate());
	}

	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator detonate()
	{
		yield return new WaitForSeconds(fuseTime);
		AudioSource.PlayClipAtPoint(explosionSound, transform.position);
		Network.Instantiate(explosionPrefab, transform.position, transform.rotation, 0);
		Destroy(gameObject, 0f);
	}
}
