using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(NetworkIdentity))]
public class BaseManager : NetworkBehaviour
{
    public Action<List<Player>> OnGameStart;
    public Action<Player> OnPlayerDisconnected;
}
