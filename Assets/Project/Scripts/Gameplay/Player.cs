using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class Player : NetworkBehaviour
{
    // needs to read inputs
    InputActions inputs;

    // needs to call Racket to move
    RacketMovement racket;
    public RacketMovement Racket { set => racket = value; }

    #region Server

    public void ConnectRacket()
    {
        if (!isOwned)
            return;

        inputs = new();
        inputs.Enable();
        inputs.Gameplay.Enable();
        inputs.Gameplay.Movement.performed += racket.MovementPerformed;
        inputs.Gameplay.Movement.canceled += racket.MovementCanceled;
    }


    public void DisconnectRacket()
    {
        if (!isOwned)
            return;

        inputs.Gameplay.Movement.performed -= racket.MovementPerformed;
        inputs.Gameplay.Movement.canceled -= racket.MovementCanceled;
        inputs.Gameplay.Disable();
        inputs.Disable();
    }
    #endregion


    #region Client


    private void Awake()
    {
        if (isServer && !isOwned)
            return;

        DontDestroyOnLoad(gameObject);
    }




    #endregion
}
