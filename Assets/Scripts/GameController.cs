using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public int num_players;
    public Player[] player_array;
    public GameObject active_player;
    public bool p1_active;

    public GameObject grid;

    public GameObject pawn_prefab;
    public GameObject rook_prefab;
    public GameObject knight_prefab;
    public GameObject bishop_prefab;
    public GameObject queen_prefab;
    public GameObject king_prefab;

    public int grid_size;
    public float shift_amount;
    public Vector2[,] position_array;

    public GameObject[,] tile_array;
    public GameObject tile_prefab;

    public Material tan;
    public Material black;
    public Material weird;

   // [SyncVar][SerializeField]
   // public GameObject THIS_ONE;

   // [Command]
    //public void CmdGetFunky()
    //{
      //  THIS_ONE.GetComponent<MeshRenderer>().material = weird;
        //THIS_ONE.transform.position += new Vector3(1, 1, 1);     
    //}
   /* [ClientRpc]
    public void RpcNextColor()
    {
        THIS_ONE.GetComponent<MeshRenderer>().material = weird;
        THIS_ONE.transform.position += new Vector3(1, 1, 1);
    } */

    public override void OnStartServer()
    {
        BuildBoard();
    }

    [Server]
    public void BuildBoard()
    {
    
        grid = GameObject.Find("Grid");

        // Create a 2d array of legal positions for pieces.
        // While we're at it, place game pieces AND tile objects.
        position_array = new Vector2[grid_size, grid_size];
        tile_array = new GameObject[grid_size, grid_size];
        for(int i = 0; i < grid_size; i++)
        {
            for(int j = 0; j < grid_size; j++)
            {
                //Build position_array.
                position_array[i, j] = new Vector2(i * shift_amount, j * shift_amount);
                // Instantiate Tiles.
                tile_array[i,j] = Instantiate(tile_prefab, grid.transform);
                tile_array[i, j].transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                tile_array[i, j].transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.4f, tile_array[i, j].transform.position.z);
                // Instantiate GamePieces.
                if(j == 1 || j == 6)
                {
                    // Place a pawn.
                    GameObject pawn = Instantiate(pawn_prefab, grid.transform);
                    NetworkServer.Spawn(pawn);
                   // THIS_ONE = pawn;
                    pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                    pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, -0.37f, tile_array[i, j].transform.position.z);
                    if (j == 1)
                    {
                        pawn.GetComponent<MeshRenderer>().material = tan;
                    }
                    else
                    {
                        pawn.GetComponent<MeshRenderer>().material = black;
                    }
                }
                if(j == 0 || j == 7)
                {
                    if(i == 0 || i == 7)
                    {
                        // Place a rook.
                        GameObject pawn = Instantiate(rook_prefab, grid.transform);
                        pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                        pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, 1, tile_array[i, j].transform.position.z);
                    }
                    if(i == 1 || i == 6)
                    {
                        // Place a knight.
                        GameObject pawn = Instantiate(knight_prefab, grid.transform);
                        pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                        pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, 1, tile_array[i, j].transform.position.z);
                    }
                    if(i == 2 || i == 5)
                    {
                        // Place a bishop.
                        GameObject pawn = Instantiate(bishop_prefab, grid.transform);
                        pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                        pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, 1, tile_array[i, j].transform.position.z);
                    }
                    if( i == 3)
                    {
                        // Place a queen.
                        GameObject pawn = Instantiate(queen_prefab, grid.transform);
                        pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                        pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, 1, tile_array[i, j].transform.position.z);
                    }
                    if (i == 4)
                    {
                        // Place a king.
                        GameObject pawn = Instantiate(king_prefab, grid.transform);
                        pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                        pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                        if (j == 0)
                        {
                            Material[] temp;
                            temp = new Material[2];
                            temp[0] = tan;
                            temp[1] = tan;
                            pawn.GetComponent<MeshRenderer>().materials = temp;
                            
                        }
                        else
                        {
                            Material[] temp;
                            temp = new Material[2];
                            temp[0] = black;
                            temp[1] = black;
                            pawn.GetComponent<MeshRenderer>().materials = temp;
                        }
                    }
                }


            }
        }
        
    }
    
    private void Update()
    {

    }

}
