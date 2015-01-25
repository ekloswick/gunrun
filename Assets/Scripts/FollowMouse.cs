using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {

	public float playerRotateSpeed = 10.0f;
	public float cameraMoveSpeed = 0.01f;
	public float cameraHeightModifier = 0.5f;
	public float cameraBaseHeight = 6.0f;

	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {

	}

	// LateUpdate is called once per frame, but happens just before rendering (lowers lag by ~1 frame)

	void LateUpdate ()
	{
		// Acquire mouse position in world
		var mousePos = Input.mousePosition;
		mousePos.z = 15;
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(mousePos);

		pointToCursor(mouseWorldPoint);
		positionCamera(mouseWorldPoint);
	}

	void pointToCursor(Vector3 mousePoint)
	{
		Quaternion targetRotation = Quaternion.LookRotation (mousePoint - transform.position);
		targetRotation.z = 0;
		targetRotation.x = 0;
		float strength = Mathf.Min (playerRotateSpeed * Time.deltaTime, 1);
		Debug.Log (strength);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, strength);
	}

	void positionCamera(Vector3 mousePoint)
	{
		// Set vectors to same height
		Vector3 cameraPosition = Camera.main.transform.position;
		Vector3 playerPosition = new Vector3 (transform.position.x, cameraPosition.y, transform.position.z);
		mousePoint.y = cameraPosition.y;

		// Set new position to midpoint of vectors
		Vector3 newPosition = ((mousePoint - playerPosition) * 0.5f) + playerPosition;

		// Adjust height
		newPosition.y = cameraBaseHeight + Vector3.Distance (playerPosition, mousePoint) * cameraHeightModifier;

		// lerp to new position
		Camera.main.transform.position = Vector3.Lerp (cameraPosition, newPosition, cameraMoveSpeed);
	}
}