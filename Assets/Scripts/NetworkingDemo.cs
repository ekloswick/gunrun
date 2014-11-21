using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkingDemo : MonoBehaviour {

    public Text ipText;
    public Text portText;

    private string ipAddress = "";
    private int port = 0;

    void Start()
    {
        Invoke("printDebug", 0.4f);
    }

    public void hostServer()
    {
        Network.InitializeServer(4, port, true);
        Debug.Log("Hosting on 127.0.0.1, listening on port " + port);
    }

    public void connectToServer()
    {
        ipAddress = ipText.text;
        int.TryParse(portText.text, out port);

        if (!string.IsNullOrEmpty(ipAddress) && !string.IsNullOrEmpty(port.ToString()))
        {
            Network.Connect(ipAddress, port);
        }
        else
        {
            Debug.Log("Connect error: invalid IP address or port");
        }
    }

    public void printDebug()
    {
        Debug.Log(ipAddress + ':' + port);
        Invoke("printDebug", 0.4f);
    }
}
