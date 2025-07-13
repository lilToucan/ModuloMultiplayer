using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Host")]
    [SerializeField]
    GameObject connectingPannel;

    public void HostServer()
    {
        NetworkManager.singleton.StartHost();
        connectingPannel.SetActive(false);
    }

}
