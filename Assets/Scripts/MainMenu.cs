using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button networkingButton;

    public void loadNetworkingScene()
    {
        Application.LoadLevel("NetworkingDemo");
    }
}
