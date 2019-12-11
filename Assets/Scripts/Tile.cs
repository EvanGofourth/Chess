using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tile : NetworkBehaviour
{
    [SyncVar] public GameObject game_piece;
    [SyncVar] public bool occupied;
    [SyncVar] public int x;
    [SyncVar] public int y;
     public bool tan_spawn;
    [SyncVar] public GameObject up_tile;
    [SyncVar] public GameObject down_tile;
    [SyncVar] public GameObject left_tile;
    [SyncVar] public GameObject right_tile;

}
