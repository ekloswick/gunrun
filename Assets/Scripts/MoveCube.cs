using UnityEngine;
using System.Collections;

public class MoveCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (networkView.isMine)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(Vector3.up * Time.deltaTime * 10);
                //transform.position += Vector3.up * Time.deltaTime * 10;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(Vector3.up * Time.deltaTime * -10);
                //transform.position -= Vector3.up * Time.deltaTime * 10;
            }
        }
	}
}
