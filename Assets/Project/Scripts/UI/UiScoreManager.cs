using UnityEngine;
using Mirror;
using TMPro;

public class UiScoreManager : NetworkBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreLeft;
    [SerializeField]
    TextMeshProUGUI scoreRight;

    public void UpdateScore(int[] scores)
    {
        scoreLeft.text = scores[0].ToString();
        scoreRight.text = scores[1].ToString();
    }
}
