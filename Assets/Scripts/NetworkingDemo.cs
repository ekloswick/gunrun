using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkingDemo : MonoBehaviour {

    public Text ipText;
    public Text portText;
    public Text connectionsText;

    public Button disconnectButton;

    private string ipAddress = "";
    private int port = 0;
    private bool connected = false;

    void Update()
    {
        if (connected)
        {
            connectionsText.text = "Connections: " + Network.connections.Length.ToString();
            disconnectButton.interactable = true;
        }
        else
        {
            connectionsText.text = "Not connected";
            disconnectButton.interactable = false;
        }
    }

    public void hostServer()
    {
        if (!connected)
        {
            if (!string.IsNullOrEmpty(portText.text))
            {
                int.TryParse(portText.text, out port);
                Network.InitializeServer(4, port, true);
                Debug.Log("Listening on port " + port);
            }
            else
            {
                Debug.Log("Invalid port number");
            }

        }
        else
        {
            Debug.Log("Already connected to a server");
        }
    }

    public void connectToServer()
    {
        if (!connected)
        {
            ipAddress = ipText.text;
            int.TryParse(portText.text, out port);

            // try to connect
            if (!string.IsNullOrEmpty(ipAddress) && !string.IsNullOrEmpty(port.ToString()))
            {
                Network.Connect(ipAddress, port);
            }
            else
            {
                Debug.Log("Connect error: invalid IP address or port");
            }
        }
        else
        {
            Debug.Log("Already connected to a server");
        }
        
    }

    void OnServerInitialized()
    {
        Debug.Log("Hosting on 127.0.0.1, listening on port " + port);
        connected = true;
    }

    void OnConnectedToServer()
    {
        Debug.Log("Connected to " + ipAddress + ":" + port);
        connected = true;
    }

    void OnPlayerConnected()
    {
        Debug.Log("Player has connected");
    }

    void OnPlayerDisconnected()
    {
        Debug.Log("Player has disconnected");
    }

    public void disconnectFromServer()
    {
        Network.Disconnect();
        if (Network.isServer)
        {
            Debug.Log("Server successfully closed");
        }
        else
        {
            Debug.Log("Disconnected from server");
        }
    }

    void OnDisconnectedFromServer()
    {
        connected = false;
    }
}
