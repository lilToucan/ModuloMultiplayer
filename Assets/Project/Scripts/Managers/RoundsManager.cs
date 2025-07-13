using UnityEngine;
using Mirror;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class RoundsManager : BaseManager
{
    // start the game
    [SerializeField] BallMovement ball;
    Vector2 ballDir;

    //EndCurrentRound
    [SerializeField] ScoreManager scoreManager;

    //end the game
    [SerializeField] GameObject winPannel;
    [SerializeField] TextMeshProUGUI winnerText;

    // reset positions
    [SerializeField] RacketMovement[] rackets = new RacketMovement[2];
    Vector3[] spawnPoints = new Vector3[2];

    // keep track of players
    List<Player> players = new();

    #region Client

    public override void OnStartServer()
    {
        base.OnStartServer();
        OnGameStart += StartGame;
        OnPlayerDisconnected += PlayerDisconnected;

        for (int i = 0; i < rackets.Length; i++)
        {
            RacketMovement racket = rackets[i];
            spawnPoints[i] = racket.transform.position;
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        foreach (Player player in players)
        {
            player.DisconnectRacket();
        }
        players.Clear();
    }

    private void PlayerDisconnected(Player player)
    {
        player.DisconnectRacket();
        players.Remove(player);
    }

    [Client]
    private void StartGame(List<Player> playersGiven)
    {
        players = playersGiven;

        StartCoroutine(WaitUntillNetworkIsReady());
    }

    private IEnumerator WaitUntillNetworkIsReady()
    {
        while (!NetworkClient.ready)
            yield return null;

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            if (!player.isOwned)
                continue;
            
            player.Racket = rackets[i];
            player.ConnectRacket();
        }

        ballDir = spawnPoints[0] - ball.transform.position;

        scoreManager.OnScoreUpdated += ResetPositions;
        scoreManager.OnWin += EndGame;

        StartRound();
    }

    private void EndGame(int index)
    {
        winnerText.text = players[index].name;
        winPannel.SetActive(true);
    }

    private void StartRound()
    {
        ballDir = -ballDir;
        ball.ResetVelocity(ballDir);
    }

    #endregion

    #region Server

    
    private void ResetPositions()
    {
        ball.transform.position = Vector3.zero;
        for (int i = 0; i < players.Count; i++)
        {
            rackets[i].transform.position = spawnPoints[i];
        }

        StartRound();
    }

    #endregion
}
