using UnityEngine;
using Mirror;
using System;

public class PongNetworkManager : NetworkManager
{
    // start the game
    int numberOfPlayers = 2;

    // keep score
    int[] score = new int[2];
    public Action<int[]> OnScoreUpdated;

    // reset positions
    struct PlayerSpawn
    {
        public Vector3 spawnPoint;
        // public Racket racket
    }
    PlayerSpawn[] spawns = new PlayerSpawn[2];

    // know when the game ends
    [SerializeField]
    int maxScore = 3;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        // get the connected player
    }


}
