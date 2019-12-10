using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Piece : NetworkBehaviour
{
    public bool white;
    [SyncVar] public int x;
    [SyncVar] public int y;

    public bool is_pawn;
    public bool is_rook;
    public bool is_knight;
    public bool is_bishop;
    public bool is_queen;
    public bool is_king;

    public bool first_move;

    private void Start()
    {
        first_move = true;
    }

}
