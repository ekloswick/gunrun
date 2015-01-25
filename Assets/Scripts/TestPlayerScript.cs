using UnityEngine;
using System.Collections;

public class TestPlayerScript : MonoBehaviour {

    public NetworkView playerNetworkView;
    public float speed = 8f;
	public Color playerColor;
	public int hitpoints = 10;

    private float syncDelay = 0f;
    private float syncTime = 0f;
    private float lastSyncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
	private Color colorHit = Color.red;


	// Use this for initialization
	void Start () {
		playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		renderer.material.color = playerColor;
	}

    void Update()
    {
        if (playerNetworkView.isMine)
        {
            inputMovement();

            /* RPC test function
            inputColorChange();
            */
        }
        else
        {
            syncMovement();
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

    private void syncMovement()
    {
        syncTime += Time.deltaTime;
        rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }

	/* RPC test function
    private void inputColorChange()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        }
    }
	*/

    // Overridden/Impelemented functions

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;

        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);

            syncVelocity = rigidbody.velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncPosition);

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
