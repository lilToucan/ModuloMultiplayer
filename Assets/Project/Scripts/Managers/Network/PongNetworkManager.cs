using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class PongNetworkManager : NetworkManager
{
    List<BaseManager> managers = new List<BaseManager>();
    List<Player> players = new List<Player>();

    Action<List<Player>> OnGameStart;
    Action<Player> OnPlayerDisconeccted;

    public static Action OnClientConnected;
    public static Action OnClientDisconnected;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();

    }
    public override void OnClientDisconnect()
    {
        OnClientDisconnected?.Invoke();
        base.OnClientDisconnect();

    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        if (numPlayers > 2)
        {
            conn.Disconnect();
            return;

        }

        if (!conn.identity.TryGetComponent(out Player player))
            return;

        player.name = $"player {numPlayers}";
        players.Add(player);
        if (player.isOwned)
            return;
        DontDestroyOnLoad(player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (!conn.identity.TryGetComponent(out Player player))
            return;

        OnPlayerDisconeccted?.Invoke(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        foreach (var child in managers)
        {
            OnGameStart -= child.OnGameStart;
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {

        if (SceneManager.GetActiveScene().name == "Game")
        {
            BaseManager[] managersInScene = FindObjectsByType<BaseManager>(FindObjectsSortMode.None);
            foreach (var manager in managersInScene)
            {
                managers.Add(manager);

                OnGameStart += manager.OnGameStart;
                OnPlayerDisconeccted += manager.OnPlayerDisconnected;
            }

            OnGameStart?.Invoke(players);
        }
    }

    public void StartGame()
    {
        if (numPlayers < 2) return;

        ServerChangeScene("Game");
    }
}
