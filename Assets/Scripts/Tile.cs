using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tile : NetworkBehaviour
{
    public GameObject game_piece;
    public bool occupied;
    [SyncVar] public int x;
    [SyncVar] public int y;
    public bool tan_spawn;

}
