using UnityEngine;
using System.Collections;

public class TestPlayerScript : MonoBehaviour {

    public NetworkView playerNetworkView;
    public float speed = 8f;
	public Color playerColor;
	public int hitpoints = 10;

	public float playerRotateSpeed = 10.0f;
	public float cameraMoveSpeed = 0.01f;
	public float cameraHeightModifier = 0.5f;
	public float cameraBaseHeight = 6.0f;

	private Vector3 mousePosition;

    private float syncDelay = 0f;
    private float syncTime = 0f;
    private float lastSyncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Quaternion syncRotation = Quaternion.identity;
    //private Quaternion syncStartRotation = Quaternion.identity;
    //private Quaternion syncEndRotation = Quaternion.identity;
	private Color colorHit = Color.red;


	// Use this for initialization
	void Start ()
	{
		ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
	}

    void Update()
    {
        if (playerNetworkView.isMine)
        {
            inputMovement();
            inputColor();
        }
        else
        {
            syncMovement();
        }
    }

	// LateUpdate is called after Update. Happens just before rendering (lowers lag by ~1 frame)

	void LateUpdate ()
	{
		if (playerNetworkView.isMine)
		{
			inputMouse();
		}
	}

    void inputMovement()
    {
        if (Input.GetKey(KeyCode.W))
            rigidbody.MovePosition(rigidbody.position + Vector3.forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            rigidbody.MovePosition(rigidbody.position - Vector3.forward * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            rigidbody.MovePosition(rigidbody.position + Vector3.right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            rigidbody.MovePosition(rigidbody.position - Vector3.right * speed * Time.deltaTime);
    }

    void inputMouse()
    {
    	// Acquire mouse position in world
		var mousePos = Input.mousePosition;
		mousePos.z = 15;
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(mousePos);

		pointToCursor(mouseWorldPoint);
		positionCamera(mouseWorldPoint);
    }

    void inputColor()
    {
    	Color color = renderer.material.color;
    	ChangeColorTo(new Vector3(color.r, color.g, color.b));
    }

    private void syncMovement()
    {
        syncTime += Time.deltaTime;
        rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        transform.rotation = Quaternion.Slerp(transform.rotation, syncRotation, syncTime / syncDelay);
    }

	void pointToCursor(Vector3 mousePoint)
	{
		Quaternion targetRotation = Quaternion.LookRotation (mousePoint - transform.position);

		targetRotation.z = 0;
		targetRotation.x = 0;

		float strength = Mathf.Min (playerRotateSpeed * Time.deltaTime, 1);

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

    // Overridden/Impelemented functions

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;
        syncRotation = Quaternion.identity;

        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);

            syncVelocity = rigidbody.velocity;
            stream.Serialize(ref syncVelocity);

            syncRotation = transform.rotation;
            stream.Serialize(ref syncRotation);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);
            stream.Serialize(ref syncRotation);

            syncTime = 0f;
            syncDelay = Time.time - lastSyncTime;
            lastSyncTime = Time.time;

            syncStartPosition = rigidbody.position;
            syncEndPosition = syncPosition + syncVelocity * syncDelay;
        }
    }

    // RPCs

    [RPC]
    void ChangeColorTo(Vector3 color)
    {
    	renderer.material.color = new Color(color.x, color.y, color.z);

        if (networkView.isMine)
        {
            networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
        }
    }

	[RPC]
	void triggerHit(int damage)
	{
		hitpoints -= damage;
		renderer.material.color = Color.Lerp(colorHit, playerColor, 0.5f);

		if (networkView.isMine)
		{
			networkView.RPC("triggerHit", RPCMode.Others, damage);
		}
	}
}
