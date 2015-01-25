using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Networking : MonoBehaviour {

    //public Text ipText;
    public Text portText;
    //public Text connectionsText;

    public Button disconnectButton;

    public GameObject serverListEntryPrefab;
    public GameObject serverListPanel;
    public GameObject playerPrefab;

    private string typeName = "TypeEliKloswick";
    private string gameName = "MyGameName";
    private GameObject canvasUIObj;

    private int portNumber;

    private HostData[] serverList;
    private ArrayList serverListUIPrefabs;
    private int playerCount;

    // default Unity functions

    void Start()
    {
        //MasterServer.ipAddress = "127.0.0.1";

        canvasUIObj = GameObject.Find("Canvas");

        serverListUIPrefabs = new ArrayList();
    }

    void Update()
    {
        // enables/disables disconnect button
        if (!Network.isClient && !Network.isServer)
        {
            disconnectButton.interactable = false;
        }
        else
        {
            disconnectButton.interactable = true;
        }

        // shows/hides server browser
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasUIObj.activeSelf == false)
            {
                canvasUIObj.SetActive(true);
            }
            else
            {
                canvasUIObj.SetActive(false);
            }
        }
    }

    // custom functions

    public void startServer()
    {
        int.TryParse(portText.text, out portNumber);
        Network.InitializeServer(1, portNumber, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
        playerCount = 0;
    }

    public void stopServer()
    {
        if (Network.isServer)
        {
            MasterServer.UnregisterHost();
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Test"))
        {
            Destroy(obj);
        }

        Network.Disconnect();
    }

    public void refreshServerList()
    {
        Debug.Log("Refreshing server list");
        MasterServer.RequestHostList(typeName);
    }

    public void joinServer(HostData hData)
    {
        Debug.Log("Joining server");
        Network.Connect(hData);
    }

    public void spawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0f, 3f, 0f), Quaternion.identity, 0);
    }

    // Overridden/Implemented functions

    void OnServerInitialized()
    {
        Application.LoadLevelAdditive("TestFloor");
        spawnPlayer();
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server joined");

        Application.LoadLevelAdditive("TestFloor");
        spawnPlayer();
    }

    void OnDisconnectFromServer()
    {
        Debug.Log("Disconnected from server");
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        playerCount += 1;
        Debug.Log("There are now " + playerCount + " players connected");
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        playerCount -= 1;
        Debug.Log("There are now " + playerCount + " players connected");

        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        Debug.Log("MS Event received: " + msEvent);
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            // clean up previous server entries
            foreach (GameObject obj in serverListUIPrefabs)
            {
                Destroy(obj);
            }

            serverListUIPrefabs.Clear();

            serverList = MasterServer.PollHostList();

            Debug.Log(serverList.Length + " servers returned");

            for (int i = 0; i < serverList.Length; i += 1)
            {
                // getting around closures
                var iteration = i;
                var data = serverList[iteration];

                // create server entry and position it to the serverListPanel
                GameObject obj = Instantiate(serverListEntryPrefab) as GameObject;
                obj.transform.SetParent(serverListPanel.transform, false);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40 - iteration * 80);

                // set up connect button function
                obj.GetComponentInChildren<Button>().onClick.AddListener(() => joinServer(data));

                // add to list for easy discarding later
                serverListUIPrefabs.Add(obj);
            }
        }
    }
}
