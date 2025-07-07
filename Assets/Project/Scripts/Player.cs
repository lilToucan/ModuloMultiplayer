using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class Player : NetworkBehaviour
{
    // needs to read inputs
    // needs to call Racket to move

    #region Server



    #endregion

    #region Client

    private void Awake()
    {
        
    }


    #endregion
}
