using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinMenu : MonoBehaviour
{
    [Header("Join")]
    [SerializeField] GameObject joinMenuPannel;
    [SerializeField] GameObject lobbyMenuPannel;
    [SerializeField] TMP_InputField adressInput;
    [SerializeField] Button joinButton;

    private void OnEnable()
    {
        adressInput.text = "localhost";
        PongNetworkManager.OnClientConnected += HandleClientConnected;
        PongNetworkManager.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        PongNetworkManager.OnClientConnected -= HandleClientConnected;
        PongNetworkManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinServer()
    {
        string address = adressInput.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        joinMenuPannel.SetActive(false);
        lobbyMenuPannel.SetActive(true);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;

        joinMenuPannel.SetActive(true);
        lobbyMenuPannel.SetActive(false);
    }
}
