using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class Player : NetworkBehaviour
{
    // needs to read inputs
    InputActions inputs;

    // needs to call Racket to move
    [HideInInspector]
    public RacketMovement racket;

    #region Server
    [Server]
    public void ConnectRacket()
    {
        inputs = new();
        inputs.Gameplay.Movement.performed += racket.MovementPerformed;
        inputs.Gameplay.Movement.canceled += racket.MovementCanceled;
    }

    [Server]
    public void DisconectRacket()
    {
        inputs.Gameplay.Movement.performed -= racket.MovementPerformed;
        inputs.Gameplay.Movement.canceled -= racket.MovementCanceled;
    }
    #endregion

    #region Client




    #endregion
}
