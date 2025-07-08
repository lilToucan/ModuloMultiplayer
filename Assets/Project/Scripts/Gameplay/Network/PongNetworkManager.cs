using UnityEngine;
using Mirror;
using System;
using Random = UnityEngine.Random;

public class PongNetworkManager : NetworkManager
{
    // start the game
    [SerializeField]
    BallMovement ball;
    int numberOfPlayers = 2;
    int clientsConnected = 0;

    // keep score
    [SerializeField] UiScoreManager uiScore;
    int[] score = new int[2];
    public Action<int[]> OnScoreUpdated;

    // reset positions
    [SerializeField]
    RacketMovement[] rackets = new RacketMovement[2];
    [SerializeField]
    Vector3[] spawnPoints = new Vector3[2];

    // know when the game ends
    [SerializeField]
    int maxScore = 3;

    // keep track of players
    Player[] players = new Player[2];

    public override void OnStartServer()
    {
        base.OnStartServer();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            spawnPoints[i] = rackets[i].transform.position;
        }

        OnScoreUpdated += uiScore.UpdateScore;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        // get the connected player
        if (!conn.identity.TryGetComponent(out Player player))
            return;

        player.racket = rackets[clientsConnected];
        player.ConnectRacket();
        players[clientsConnected] = player;
        clientsConnected++;

        if (clientsConnected < 2)
            return;

        // start game
        StartGame();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        foreach (Player player in players)
        {
            player.DisconectRacket();
        }
    }

    void StartGame()
    {
        Vector3 ballPos = ball.transform.position = Vector3.zero;

        Vector2 dir = Vector2.zero;
        foreach (Player p in players)
        {
            if (Random.Range(0, 1f) < .5f)
                continue;

            dir = (p.transform.position - ballPos).normalized;
            break;
        }

        ball.StartGame(dir);
    }
}
