using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button serverBrowserButton;

    public void loadServerBrowser()
    {
        Application.LoadLevel("ServerBrowser");
    }
}
