using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class ScoreManager : BaseManager
{
    // keep score
    int[] scores = new int[2];
    public Action OnScoreUpdated;
    public Action<int> OnWin;
    public Action<int> OnScore;

    // know when the game ends
    [SerializeField]
    int maxScore = 3;

    [SerializeField]
    TextMeshProUGUI scoreLeft;
    [SerializeField]
    TextMeshProUGUI scoreRight;

    #region Server
    #endregion
    private void UpdateScore()
    {
        scoreLeft.text = scores[0].ToString();
        scoreRight.text = scores[1].ToString();
    }

    public override void OnStartServer()
    {
        OnScoreUpdated += UpdateScore;
        OnScore += Scored;
    }

    public override void OnStopServer()
    {
        OnScoreUpdated -= UpdateScore;
        OnScore -= Scored;
    }

    private void OnEnable()
    {
        OnGameStart += ResetScore;
    }
    private void OnDisable()
    {
        OnGameStart -= ResetScore;
    }

    private void Scored(int index)
    {
        int score = scores[index];
        score++;
        scores[index] = score;

        OnScoreUpdated?.Invoke();

        if (score < maxScore)
            return;

        OnWin?.Invoke(index);
    }

    private void ResetScore(List<Player> players)
    {
        StartCoroutine(WaitUntillNetworkIsReady());
    }

    private IEnumerator WaitUntillNetworkIsReady()
    {
        while (!NetworkClient.ready)
            yield return null;
        scores = new int[2];
        UpdateScore();
    }  
}
