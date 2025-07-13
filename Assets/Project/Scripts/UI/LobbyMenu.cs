using Mirror;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyMenu : MonoBehaviour
{

    [SerializeField] GameObject lobbyUI;

    private void OnEnable()
    {
        PongNetworkManager.OnClientConnected += HandleClientConnected;
        PongNetworkManager.OnClientDisconnected += HandleClientDisconeccted;
    }

    private void HandleClientDisconeccted()
    {
        LeaveLobby();
    }

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.OnStopClient();
        }
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        PongNetworkManager.OnClientConnected -= HandleClientConnected;
        PongNetworkManager.OnClientDisconnected -= HandleClientDisconeccted;
    }
}