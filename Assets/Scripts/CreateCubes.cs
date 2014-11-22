using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCubes : MonoBehaviour {

    public GameObject cubePrefab = null;

    private List<GameObject> cubeList = new List<GameObject>();

	// Update is called once per frame
	void Update () {
        if (NetworkingDemo.connected && Input.GetKeyDown(KeyCode.Space))
        {
            cubeList.Add(Network.Instantiate(cubePrefab, Vector3.zero, Quaternion.identity, 0) as GameObject);
        }

	}

    void OnDisconnectedFromServer()
    {
        foreach (GameObject obj in cubeList)
        {
            Network.Destroy(obj);
        }

        cubeList.Clear();
    }
}
