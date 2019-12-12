using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Network : NetworkBehaviour
{
    public GameObject camera_holding_object;
    public GameObject[] pieces;

    public Material tan;
    public Material black;
    public Material weird;

    public bool white_player;
    public bool my_turn;

    [SyncVar] public GameObject selected_piece;

    public GameObject game_controller;

    public float shift_amount;


    public Tile GetTileAt(GameObject start, int x, int y)
    {
        if (x > 7 || x < 0 || y > 7 || y < 0) return null;
        Piece p = start.GetComponent<Piece>();
        Tile p_tile = p.my_tile.GetComponent<Tile>();
        Tile target_tile = p_tile;
        while (!(target_tile.x == 0 && target_tile.y == 0))
        {
                if(target_tile.left_tile)
                {
                    if (target_tile.left_tile.GetComponent<Tile>().x > -1)
                    {
                        target_tile = target_tile.left_tile.GetComponent<Tile>();
                    }
                }

                if(target_tile.down_tile)
                {
                    if (target_tile.down_tile.GetComponent<Tile>().y > -1)
                    {
                        target_tile = target_tile.down_tile.GetComponent<Tile>();
                    }
                }

        }
        // now target tile is bottom left.
        for (int i = 0; i < x; i++)
        {
            if(target_tile.right_tile)
                target_tile = target_tile.right_tile.GetComponent<Tile>();
        }
        for (int i = 0; i < y; i++)
        {
            if(target_tile.up_tile)
                target_tile = target_tile.up_tile.GetComponent<Tile>();
        }
        Debug.Log(target_tile.x);
        Debug.Log(target_tile.y);
        return target_tile;
    }
    public override void OnStartLocalPlayer()
    {  
        this.GetComponentInChildren<Camera>().enabled = true;
        GetComponent<CameraScript>().enabled = true;
        camera_holding_object.SetActive(true);

    }

    public override void OnStartServer()
    {
        Debug.Log("Server Started!");
    }

    public void Start()
    {
        game_controller = GameObject.Find("GameControllerObj");
        shift_amount = GameObject.Find("GameControllerObj").GetComponent<GameController>().shift_amount;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 2)
        {
            white_player = true;
            my_turn = true;
        }
        else
        {
            white_player = false;
            my_turn = false;
        }
    }

    [ClientRpc]
    public void RpcNextColor(GameObject obj)
    {
        obj.transform.position += new Vector3(1, 1, 1);
    }

    [Command]
    public void CmdGetFunky(GameObject obj)
    {
        obj.transform.position += new Vector3(1, 1, 1);
    }

    public bool PieceCanMoveTo(GameObject obj, int x_dest, int y_dest)
    {
        Piece p = obj.GetComponent<Piece>();
        if (p.x == x_dest && p.y == y_dest) return false;// you can not move to where you already are.
        if(p.is_pawn)
        {
            if(p.white)
            {
                if (p.first_move)
                {
                    
                    if(p.my_tile.GetComponent<Tile>().up_tile)
                        if (y_dest == (p.y + 2) && x_dest == p.x && p.my_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>().occupied == false) return true; //move forward two is allowed.

                    if (y_dest == (p.y + 1) && x_dest == p.x) return true; //move forward one is allowed.
                    if(p.my_tile.GetComponent<Tile>().up_tile)
                        if(p.my_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>().right_tile)
                            if (y_dest == p.y + 1 && x_dest == p.x + 1 && p.my_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>().right_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move up-right ok if piece on it.
                    if(p.my_tile.GetComponent<Tile>().up_tile)
                        if(p.my_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>().left_tile)
                            if (y_dest == p.y + 1 && x_dest == p.x - 1 && p.my_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>().left_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move up-left ok if piece on it.
                }
                else
                {
                    if (y_dest == (p.y + 1) && x_dest == p.x) return true; //move forward one is allowed.
                    if (y_dest == p.y + 1 && x_dest == p.x + 1 && p.my_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>().right_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move up-right ok if piece on it.
                    if (y_dest == p.y + 1 && x_dest == p.x - 1 && p.my_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>().left_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move up-left ok if piece on it.

                }
            }
            else
            {
                if (p.first_move)
                {
                    if (y_dest == (p.y - 2) && x_dest == p.x && p.my_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>().occupied == false) return true; //move forward two is allowed.
                    if (y_dest == (p.y - 1) && x_dest == p.x) return true; //move forward one is allowed.
                    if (y_dest == p.y - 1 && x_dest == p.x + 1 && p.my_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>().right_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move down-right ok if piece on it.
                    if (y_dest == p.y - 1 && x_dest == p.x - 1 && p.my_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>().left_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move down-left ok if piece on it.

                }
                else
                {
                    if (y_dest == (p.y - 1) && x_dest == p.x) return true; //move forward one is allowed.
                    if (y_dest == p.y - 1 && x_dest == p.x + 1 && p.my_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>().right_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move down-right ok if piece on it.
                    if (y_dest == p.y - 1 && x_dest == p.x - 1 && p.my_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>().left_tile.GetComponent<Tile>().occupied == true) return true; // diagonal move down-left ok if piece on it.

                }
            }
        }
        else if(p.is_king)
        {
            if (x_dest == p.x && y_dest == p.y + 1) return true; // move up one is fine.
            if (x_dest == p.x && y_dest == p.y - 1) return true; // move down one is fine.
            if (x_dest == p.x + 1 && y_dest == p.y) return true; // move right one is fine.
            if (x_dest == p.x - 1 && y_dest == p.y) return true; // move left one is fine.
            if (x_dest == p.x + 1 && y_dest == p.y + 1) return true; // move up and right is fine.
            if (x_dest == p.x - 1 && y_dest == p.y + 1) return true; // move up and left is fine.
            if (x_dest == p.x + 1 && y_dest == p.y - 1) return true; // move down and right is fine.
            if (x_dest == p.x - 1 && y_dest == p.y - 1) return true; // move down and left is fine.
        }
        else if(p.is_queen)
        {
            //step 1, check rook lines.
            Tile start_tile = p.my_tile.GetComponent<Tile>();
            for(int i = p.y; i < 7; i++)
            {
                start_tile = start_tile.up_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            for (int i = p.y; i > 0; i--)
            {
                start_tile = start_tile.down_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit down like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            for (int i = p.x; i < 7; i++)
            {
                start_tile = start_tile.right_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit right like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            for (int i = p.x; i > 0; i--)
            {
                start_tile = start_tile.left_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit left like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            //step 2, check bishop lines.
            while(start_tile.right_tile)
            {
                if (!start_tile.right_tile.GetComponent<Tile>().up_tile) break; // reached the edge.
                start_tile = start_tile.right_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            while (start_tile.right_tile)
            {
                if (!start_tile.right_tile.GetComponent<Tile>().down_tile) break; // reached the edge.
                start_tile = start_tile.right_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            while (start_tile.left_tile)
            {
                if (!start_tile.left_tile.GetComponent<Tile>().down_tile) break; // reached the edge.
                start_tile = start_tile.left_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            while (start_tile.left_tile)
            {
                if (!start_tile.left_tile.GetComponent<Tile>().up_tile) break; // reached the edge.
                start_tile = start_tile.left_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
        }
        else if(p.is_bishop)
        {
            Tile start_tile = p.my_tile.GetComponent<Tile>();
            while (start_tile.right_tile)
            {
                if (!start_tile.right_tile.GetComponent<Tile>().up_tile) break; // reached the edge.
                start_tile = start_tile.right_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            while (start_tile.right_tile)
            {
                if (!start_tile.right_tile.GetComponent<Tile>().down_tile) break; // reached the edge.
                start_tile = start_tile.right_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            while (start_tile.left_tile)
            {
                if (!start_tile.left_tile.GetComponent<Tile>().down_tile) break; // reached the edge.
                start_tile = start_tile.left_tile.GetComponent<Tile>().down_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            while (start_tile.left_tile)
            {
                if (!start_tile.left_tile.GetComponent<Tile>().up_tile) break; // reached the edge.
                start_tile = start_tile.left_tile.GetComponent<Tile>().up_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up right diagonal like bishop.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
        }
        else if(p.is_rook)
        {
            
            Tile start_tile = p.my_tile.GetComponent<Tile>();
            for (int i = p.y; i < 7; i++)
            {
                start_tile = start_tile.up_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit up like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            for (int i = p.y; i > 0; i--)
            {
                start_tile = start_tile.down_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit down like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            for (int i = p.x; i < 7; i++)
            {
                start_tile = start_tile.right_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit right like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
            start_tile = p.my_tile.GetComponent<Tile>();
            for (int i = p.x; i > 0; i--)
            {
                start_tile = start_tile.left_tile.GetComponent<Tile>();
                if (start_tile.GetComponent<Tile>().x == x_dest && start_tile.GetComponent<Tile>().y == y_dest) return true; // hit left like rook.
                if (start_tile.GetComponent<Tile>().occupied == true) break; // someone got in the way.
            }
        }
        else if(p.is_knight)
        {
            if (x_dest == p.x + 1 && y_dest == p.y + 2) return true; // one of two up and right positions.
            if (x_dest == p.x + 2 && y_dest == p.y + 1) return true; // one of two up and right positions.
            if (x_dest == p.x - 1 && y_dest == p.y + 2) return true; // one of two up and left positions.
            if (x_dest == p.x - 2 && y_dest == p.y + 1) return true; // one of two up and left positions.

            if (x_dest == p.x + 1 && y_dest == p.y - 2) return true; // one of two down and right positions.
            if (x_dest == p.x + 2 && y_dest == p.y - 1) return true; // one of two down and right positions.
            if (x_dest == p.x - 1 && y_dest == p.y - 2) return true; // one of two down and left positions.
            if (x_dest == p.x - 2 && y_dest == p.y - 1) return true; // one of two down and left positions.

        }
        return false;
    }

    public bool CanAttack(GameObject obj, int x_dest, int y_dest)
    {
        Piece p = obj.GetComponent<Piece>();
        Tile p_tile = p.my_tile.GetComponent<Tile>();
        Tile target_tile = p_tile;
        target_tile = GetTileAt(obj, x_dest, y_dest);
        if (p.x == x_dest && p.y == y_dest) return false;// attack self is not permitted.
        if (target_tile.occupied == false) return false; // Slashing at nothing eh?
        if(p.is_pawn)
        {
            if (p.white)
            {
                if (x_dest == p.x + 1 && y_dest == p.y + 1) return true; //valid diagonal attack.
                if (x_dest == p.x - 1 && y_dest == p.y + 1) return true; //valid diagonal attack.
            }
            else
            {
                if (x_dest == p.x + 1 && y_dest == p.y - 1) return true; //valid diagonal attack.
                if (x_dest == p.x - 1 && y_dest == p.y - 1) return true; //valid diagonal attack.
            }
        }
        else
        {
            if (PieceCanMoveTo(obj, x_dest, y_dest)) return true;
        }
        return false;
    }

    [ClientRpc]
    public void RpcClickHandler(GameObject obj)
    {
        if(obj)
        {
            if (obj.tag == "Piece") // if the thing you clicked was a piece...
            {
                Debug.Log("Clicked a piece.");
                if (this.white_player && obj.GetComponent<Piece>().white) // if you are white clicking a white piece, it is now the selected piece.
                {   
                    selected_piece = obj;
                }
                else if (!this.white_player && !obj.GetComponent<Piece>().white)// if you are black clicking a black piece, it is now the selected piece.
                {                   
                    selected_piece = obj;                   
                }
                else
                {
                    //check if i have a selected piece.
                    //if I do, see if I can attack the piece i clicked.
                    //if not, unselect my currently selected piece.
                    if (selected_piece)
                    {
                        // if can attack piece i clicked attack
                        if(PieceCanMoveTo(selected_piece, obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().x, obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().y))
                        {
                            if(CanAttack(selected_piece, obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().x, obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().y))
                            {
                                if (isServer)
                                {
                                    RpcDestroy(obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().game_piece);
                                    RpcMove(selected_piece, obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().gameObject);
                                }
                                else
                                {
                                    CmdDestroy(obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().game_piece);
                                    CmdMove(selected_piece, obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().gameObject);
                                }
                            }
                        }
                        // else selected_piece = null;
                        
                        selected_piece = null;

                    }
                }
            }
            else if (obj.tag == "Tile") // if the thing you clicked was a tile...
            {
                if (selected_piece) // if you have a selected piece...
                {
                    // check if you can move to that tile.
                    // if you can, and it is unoccupied, move there.
                    if (PieceCanMoveTo(selected_piece, obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y) && obj.GetComponent<Tile>().occupied == false)
                    {
                        //if server rpcmove if client cmdmove.
                        if (isServer)
                        {
                            RpcMove(selected_piece, obj);
                        }
                        else
                        {
                            CmdMove(selected_piece, obj);
                        }
                    }
                    else if (PieceCanMoveTo(selected_piece, obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y)) // if you can but the tile is occupied...
                    {
                        if(white_player)
                        {
                            if (GetTileAt(selected_piece, obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y).game_piece.GetComponent<Piece>().white == false)
                            {
                                // Since you are white and the piece is NOT white, check if you can attack it.
                                if (CanAttack(selected_piece, obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y))
                                {   // if you can then take the piece.
                                    if (isServer)
                                    {
                                        RpcDestroy(obj.GetComponent<Tile>().game_piece);
                                        RpcMove(selected_piece, obj);
                                    }
                                    else
                                    {
                                        CmdDestroy(obj.GetComponent<Tile>().game_piece);
                                        CmdMove(selected_piece, obj);
                                    }
                                }
                            }
                            else
                            {
                                // The tile you tried to move to is occupied by a friendly piece.
                            }
                        }
                        else // if you are black...
                        {
                            // if the tile possesses a white piece...
                            if (GetTileAt(selected_piece, obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y).game_piece.GetComponent<Piece>().white == true)
                            {
                                // the piece is fair game. Check if you are able to attack it.
                                if (CanAttack(selected_piece, obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y))
                                {
                                    // if so, take the piece.
                                    if (isServer)
                                    {
                                        RpcDestroy(obj.GetComponent<Tile>().game_piece);
                                        RpcMove(selected_piece, obj);
                                    }
                                    else
                                    {
                                        CmdDestroy(obj.GetComponent<Tile>().game_piece);
                                        CmdMove(selected_piece, obj);
                                    }
                                }
                            }
                            else
                            {
                                // The tile you tried to move to is occupied by a friendly piece.
                            }
                        }
                    }
                    else
                    {
                        // Your piece was unable to move there.
                        selected_piece = null;
                    }
                }
            }
        }
        
    }

    [Command]
    public void CmdClickHandler(GameObject obj)
    {  
        RpcClickHandler(obj);
    }

    [ClientRpc]
    public void RpcMove(GameObject obj, GameObject dest)
    {
        int x = dest.GetComponent<Tile>().x;
        int y = dest.GetComponent<Tile>().y;

        obj.transform.position = new Vector3(dest.transform.position.x, obj.transform.position.y, dest.transform.position.z);
        obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().occupied = false;
        obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().game_piece = null;
        obj.GetComponent<Piece>().x = dest.GetComponent<Tile>().x;
        obj.GetComponent<Piece>().y = dest.GetComponent<Tile>().y;

        Tile dest_tile = dest.GetComponent<Tile>();
        dest_tile.occupied = true;
        dest_tile.game_piece = obj;
        obj.GetComponent<Piece>().my_tile = dest;

        selected_piece = null;
    }

    [Command]
    public void CmdMove(GameObject obj, GameObject dest)
    {
        int x = dest.GetComponent<Tile>().x;
        int y = dest.GetComponent<Tile>().y;

        obj.transform.position = new Vector3(dest.transform.position.x, obj.transform.position.y, dest.transform.position.z);
        obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().occupied = false;
        obj.GetComponent<Piece>().my_tile.GetComponent<Tile>().game_piece = null;
        obj.GetComponent<Piece>().x = dest.GetComponent<Tile>().x;
        obj.GetComponent<Piece>().y = dest.GetComponent<Tile>().y;

        Tile dest_tile = dest.GetComponent<Tile>();
        dest_tile.occupied = true;
        dest_tile.game_piece = obj;
        obj.GetComponent<Piece>().my_tile = dest;

        selected_piece = null;
    }

    [ClientRpc]
    public void RpcDestroy(GameObject obj)
    {
        NetworkServer.Destroy(obj);
        Destroy(obj);
    }

    [Command]
    public void CmdDestroy(GameObject obj)
    {
        NetworkServer.Destroy(obj);
        Destroy(obj);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray toMouse = this.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit rhInfo;
            bool didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);
            if (didHit)
            {
                // This section is for the 'selection' circle that glows beneath a piece.
                if (rhInfo.collider.gameObject)
                {
                    if (rhInfo.collider.gameObject.GetComponent<Piece>())
                    {
                        if (rhInfo.collider.gameObject.GetComponent<Piece>().white && white_player && !selected_piece)
                        {
                            rhInfo.collider.gameObject.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = true;
                        }
                        else if (rhInfo.collider.gameObject.GetComponent<Piece>().white && white_player && selected_piece)
                        {
                            selected_piece.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = false;
                            rhInfo.collider.gameObject.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = true;
                        }
                        else if (!rhInfo.collider.gameObject.GetComponent<Piece>().white && !white_player && !selected_piece)
                        {
                            rhInfo.collider.gameObject.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = true;
                        }
                        else if (!rhInfo.collider.gameObject.GetComponent<Piece>().white && !white_player && selected_piece)
                        {
                            selected_piece.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = false;
                            rhInfo.collider.gameObject.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = true;
                        }
                        else if (!rhInfo.collider.gameObject.GetComponent<Piece>().white && white_player && selected_piece)
                        {
                            selected_piece.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = false;
                        }
                        else if (rhInfo.collider.gameObject.GetComponent<Piece>().white && !white_player && selected_piece)
                        {
                            selected_piece.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                    else
                    {
                        if (selected_piece)
                            selected_piece.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = false;
                    }
                }
              if (isServer)
              {                           
                    RpcClickHandler(rhInfo.collider.gameObject);
              }
              else
              {
                    CmdClickHandler(rhInfo.collider.gameObject);
              }
             
               
                
            }
            else
            {
                if(selected_piece)
                    selected_piece.transform.Find("ColorCircle").GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
