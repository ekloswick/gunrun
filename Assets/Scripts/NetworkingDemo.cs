using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkingDemo : MonoBehaviour {

    //public Text ipText;
    public Text portText;
    //public Text connectionsText;

    public Button disconnectButton;

    public GameObject serverListEntryPrefab;
    public GameObject serverListPanel;

    private string typeName = "TypeEliKloswick";
    private string gameName = "MyGameName";

    private int portNumber;

    private HostData[] serverList;
    private ArrayList serverListUIPrefabs;
    private int playerCount;

    // default Unity functions

    void Start()
    {
        //MasterServer.ipAddress = "127.0.0.1";

        serverListUIPrefabs = new ArrayList();
    }

    void Update()
    {
        if (!Network.isClient && !Network.isServer)
        {
            disconnectButton.interactable = false;
        }
        else
        {
            disconnectButton.interactable = true;
        }
    }

    // custom functions

    public void startServer()
    {
        int.TryParse(portText.text, out portNumber);
        Network.InitializeServer(10, portNumber, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
        playerCount = 0;
    }

    public void stopServer()
    {
        if (Network.isServer)
        {
            MasterServer.UnregisterHost();
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


    // Overridden functions

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

    void OnConnectedToServer()
    {
        Debug.Log("Server joined");
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
    }
}
