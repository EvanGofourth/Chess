using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Network : NetworkBehaviour
{
    public GameObject camera_holding_object;
    public GameObject[] pieces;

    public GameObject[,] tiles;

    public Vector2[,] position_arr;

    public Material tan;
    public Material black;
    public Material weird;

    public bool white_player;
    public bool my_turn;

    public GameObject selected_piece;

    public float shift_amount;


    public override void OnStartLocalPlayer()
    {
        this.GetComponentInChildren<Camera>().enabled = true;
        GetComponent<CameraScript>().enabled = true;
        camera_holding_object.SetActive(true);
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

    public override void OnStartServer()
    {
        Debug.Log("Server Started!");
        // color all pieces.

    }

    public void Start()
    {
        tiles = GameObject.Find("GameControllerObj").GetComponent<GameController>().tile_array;
        position_arr = GameObject.Find("GameControllerObj").GetComponent<GameController>().position_array;
        shift_amount = GameObject.Find("GameControllerObj").GetComponent<GameController>().shift_amount;
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

    /*[ClientRpc]
    public bool RpcPieceCanMoveTo(GameObject obj)
    {
        return false;
    }

    [Command]
    public bool CmdPieceCanMoveTo(GameObject obj)
    {
        return false;
    }*/

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
                    if (y_dest == (p.y + 2) && x_dest == p.x) return true; //move forward two is allowed.
                    if (y_dest == (p.y + 1) && x_dest == p.x) return true; //move forward one is allowed.
                }
                else
                {
                    if (y_dest == (p.y + 1) && x_dest == p.x) return true; //move forward one is allowed.
                }
            }
            else
            {
                if (p.first_move)
                {
                    if (y_dest == (p.y - 2) && x_dest == p.x) return true; //move forward two is allowed.
                    if (y_dest == (p.y - 1) && x_dest == p.x) return true; //move forward one is allowed.
                }
                else
                {
                    if (y_dest == (p.y - 1) && x_dest == p.x) return true; //move forward one is allowed.
                }
            }
        }
        return false;
    }

    [ClientRpc]
    public void RpcClickHandler(GameObject obj)
    {
        Debug.Log("RpcClickHandler called..");
        if(obj.tag == "Piece")
        {
            Debug.Log("Clicked a piece.");
            if(white_player && obj.GetComponent<Piece>().white)
            {
                selected_piece = obj;
            }
            else if(!white_player && !obj.GetComponent<Piece>().white)
            {
                selected_piece = obj;
            }
            else
            {
                //check if i have a selected piece.
                //if I do, see if I can attack the piece i clicked.
                //if not, unselect my currently selected piece.
                if(selected_piece)
                {
                    // if can attack piece i clicked attack
                    // else selected_piece = null;
                    selected_piece = null;
                }
            }
        }
        else if(obj.tag == "Tile")
        {
            Debug.Log("Clicked a tile.");
            if(selected_piece)
            {
                // check if you can move to that tile.
                // if you can, do it. If not, deselect the currently selected piece.
                if(PieceCanMoveTo(selected_piece, obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y))
                {
                    Debug.Log("Piece CAN move there.");
                    //if server rpcmove if client cmdmove.
                    if(isServer)
                    {
                        RpcMove(selected_piece, obj);
                    }
                    else
                    {
                        CmdMove(selected_piece, obj);
                    }
                }
                else
                {
                    Debug.Log("Piece can NOT move there.");
                    selected_piece = null;
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
        obj.transform.position = new Vector3(dest.transform.position.x, obj.transform.position.y, dest.transform.position.z);
    }

    [Command]
    public void CmdMove(GameObject obj, GameObject dest)
    {
        obj.transform.position = new Vector3(dest.transform.position.x, obj.transform.position.y, dest.transform.position.z);
        // RpcMove(obj,x,y);
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
              if(isServer)
              {
                    // RpcNextColor(rhInfo.collider.gameObject);                  
                    RpcClickHandler(rhInfo.collider.gameObject);
                }
              else
              {
                    // CmdGetFunky(rhInfo.collider.gameObject);
                    CmdClickHandler(rhInfo.collider.gameObject);
                }               
            }
            else
            {
                Debug.Log("clicked on nothing..");
            }
        }
    }
}
