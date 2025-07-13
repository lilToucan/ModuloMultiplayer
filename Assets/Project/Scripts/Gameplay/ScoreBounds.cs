using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class ScoreBounds : NetworkBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField, Tooltip("0 = left 1 = right")] int scoreIndex;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!collision.transform.TryGetComponent(out BallMovement ball))
            return;

        scoreManager.OnScore?.Invoke(scoreIndex);
    }
}
