using Mirror;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkIdentity))]
public class RacketMovement : NetworkBehaviour
{
    // needs to move
    [SerializeField]
    float speed = 12f;
    Vector2 moveDir;
    public Vector2 currentVelocity { get => speed * Time.deltaTime * moveDir; }

    #region Server

    [Command]
    private void CmdMove()
    {
        // already checked isOwner 

        Vector3 pos = transform.position;
        pos.y += moveDir.y * speed * Time.deltaTime;

        //? check on the server if pos is out of bounds before aplaying it?
        transform.position = pos;
    }

    #endregion


    #region Client

    [ClientCallback]
    private void Update()
    {
        if (!isOwned || moveDir == Vector2.zero)
            return;

        CmdMove();
    }

    internal void MovementPerformed(InputAction.CallbackContext context)
    {
        if (!isOwned)
            return;
        moveDir.y = context.ReadValue<float>();
    }

    internal void MovementCanceled(InputAction.CallbackContext context)
    {
        if (!isOwned)
            return;
        moveDir = Vector2.zero;
    }

    #endregion

}
