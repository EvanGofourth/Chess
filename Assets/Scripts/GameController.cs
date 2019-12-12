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
    
    public GameObject pawn_black_prefab;
    public GameObject rook_black_prefab;
    public GameObject knight_black_prefab;
    public GameObject bishop_black_prefab;
    public GameObject queen_black_prefab;
    public GameObject king_black_prefab;

    public int grid_size;
    public float shift_amount;
   // public Vector2[,] position_array;

    public GameObject[,] tile_array;
    public GameObject tile_prefab;


    public Material tan;
    public Material black;
    public Material weird;


    public override void OnStartServer()
    {
        BuildBoard();
        for (int i = 0; i < grid_size; i++)
        {
            for (int j = 0; j < grid_size; j++)
            {
                if(j+1 < grid_size)
                {
                    tile_array[i, j].GetComponent<Tile>().up_tile = tile_array[i, j + 1];
                }
                else
                {
                    //tile_array[i, j].GetComponent<Tile>().up_tile = null;
                }
                if(j-1 >-1)
                {
                    tile_array[i, j].GetComponent<Tile>().down_tile = tile_array[i, j - 1];
                }
                else
                {
                    //tile_array[i, j].GetComponent<Tile>().up_tile = null;
                }
                if (i + 1 < grid_size)
                {
                    tile_array[i, j].GetComponent<Tile>().right_tile = tile_array[i + 1, j];
                }
                else
                {
                   // tile_array[i, j].GetComponent<Tile>().up_tile = null;
                }
                if (i - 1 > -1)
                {
                    tile_array[i, j].GetComponent<Tile>().left_tile = tile_array[i - 1, j];
                }
                else
                {
                   // tile_array[i, j].GetComponent<Tile>().up_tile = null;
                }
            }

        }


    }

    [Server]
    public void BuildBoard()
    {
        grid = GameObject.Find("Grid");

        // Create a 2d array of legal positions for pieces.
        // While we're at it, place game pieces AND tile objects.
        tile_array = new GameObject[grid_size, grid_size];
        for(int i = 0; i < grid_size; i++)
        {
            for(int j = 0; j < grid_size; j++)
            {
                //Build position_array.
               // position_array[i, j] = new Vector2(i * shift_amount, j * shift_amount);
                // Instantiate Tiles.
                tile_array[i,j] = Instantiate(tile_prefab, grid.transform);             
                NetworkServer.Spawn(tile_array[i, j]);
                tile_array[i, j].transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                tile_array[i, j].transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.4f, tile_array[i, j].transform.position.z);
                tile_array[i, j].GetComponent<Tile>().x = i;
                tile_array[i, j].GetComponent<Tile>().y = j;

                // Instantiate GamePieces.
                if (j == 1 || j == 6)
                {                                                        
                    if (j == 1)
                    {
                        // Place a pawn.
                        GameObject pawn = Instantiate(pawn_prefab, grid.transform);
                        NetworkServer.Spawn(pawn);
                        tile_array[i, j].GetComponent<Tile>().occupied = true;
                        tile_array[i, j].GetComponent<Tile>().game_piece = pawn;
                        pawn.GetComponent<Piece>().my_tile = tile_array[i, j];
                        pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                        pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, -0.37f, tile_array[i, j].transform.position.z);
                        pawn.GetComponent<Piece>().x = i;
                        pawn.GetComponent<Piece>().y = j;
                    }
                    else
                    {
                        // Place a pawn.
                        GameObject pawn = Instantiate(pawn_black_prefab, grid.transform);
                        
                        NetworkServer.Spawn(pawn);
                        tile_array[i, j].GetComponent<Tile>().occupied = true;
                        tile_array[i, j].GetComponent<Tile>().game_piece = pawn;
                        pawn.GetComponent<Piece>().my_tile = tile_array[i, j];
                        pawn.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                        pawn.transform.position = new Vector3(tile_array[i, j].transform.position.x, -0.37f, tile_array[i, j].transform.position.z);
                        pawn.GetComponent<Piece>().x = i;
                        pawn.GetComponent<Piece>().y = j;
                    }
                }
                if(j == 0 || j == 7)
                {
                    if(i == 0 || i == 7)
                    {
                        if(j == 0)
                        {
                            // Place a rook.
                            GameObject rook = Instantiate(rook_prefab, grid.transform);
                            NetworkServer.Spawn(rook);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = rook;
                            rook.GetComponent<Piece>().my_tile = tile_array[i, j];
                            rook.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            rook.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            rook.GetComponent<Piece>().x = i;
                            rook.GetComponent<Piece>().y = j;
                        }
                        else
                        {
                            // Place a rook.
                            GameObject rook = Instantiate(rook_black_prefab, grid.transform);
                            NetworkServer.Spawn(rook);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = rook;
                            rook.GetComponent<Piece>().my_tile = tile_array[i, j];
                            rook.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            rook.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            rook.GetComponent<Piece>().x = i;
                            rook.GetComponent<Piece>().y = j;
                        }
                        
                    }
                    if(i == 1 || i == 6)
                    {
                        if (j == 0)
                        {
                            // Place a knight.
                            GameObject knight = Instantiate(knight_prefab, grid.transform);
                            NetworkServer.Spawn(knight);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = knight;
                            knight.GetComponent<Piece>().my_tile = tile_array[i, j];
                            knight.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            knight.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            knight.GetComponent<Piece>().x = i;
                            knight.GetComponent<Piece>().y = j;
                            knight.transform.Rotate(0f, 180f, 0f, Space.World);
                        }
                        else
                        {
                            // Place a knight.
                            GameObject knight = Instantiate(knight_black_prefab, grid.transform);
                            NetworkServer.Spawn(knight);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = knight;
                            knight.GetComponent<Piece>().my_tile = tile_array[i, j];
                            knight.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            knight.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            knight.GetComponent<Piece>().x = i;
                            knight.GetComponent<Piece>().y = j;
                            //knight.transform.Rotate(0f, -90f, 0f, Space.World);
                        }
                    }
                    if(i == 2 || i == 5)
                    {
                        if (j == 0)
                        {
                            // Place a bishop.
                            GameObject bishop = Instantiate(bishop_prefab, grid.transform);
                            NetworkServer.Spawn(bishop);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = bishop;
                            bishop.GetComponent<Piece>().my_tile = tile_array[i, j];
                            bishop.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            bishop.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            bishop.GetComponent<Piece>().x = i;
                            bishop.GetComponent<Piece>().y = j;
                        }
                        else
                        {
                            // Place a bishop.
                            GameObject bishop = Instantiate(bishop_black_prefab, grid.transform);
                            NetworkServer.Spawn(bishop);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = bishop;
                            bishop.GetComponent<Piece>().my_tile = tile_array[i, j];
                            bishop.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            bishop.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            bishop.GetComponent<Piece>().x = i;
                            bishop.GetComponent<Piece>().y = j;
                        }
                    }
                    if( i == 3)
                    {
                        if(j==0)
                        {
                            // Place a queen.
                            GameObject queen = Instantiate(queen_prefab, grid.transform);
                            NetworkServer.Spawn(queen);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = queen;
                            queen.GetComponent<Piece>().my_tile = tile_array[i, j];
                            queen.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            queen.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            queen.GetComponent<Piece>().x = i;
                            queen.GetComponent<Piece>().y = j;
                        }
                        else
                        {
                            // Place a queen.
                            GameObject queen = Instantiate(queen_black_prefab, grid.transform);
                            NetworkServer.Spawn(queen);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = queen;
                            queen.GetComponent<Piece>().my_tile = tile_array[i, j];
                            queen.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            queen.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            queen.GetComponent<Piece>().x = i;
                            queen.GetComponent<Piece>().y = j;
                        }
                        
                    }
                    if (i == 4)
                    {

                        if (j == 0)
                        {
                            // Place a king.
                            GameObject king = Instantiate(king_prefab, grid.transform);
                            NetworkServer.Spawn(king);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = king;
                            king.GetComponent<Piece>().my_tile = tile_array[i, j];
                            king.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            king.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            king.GetComponent<Piece>().x = i;
                            king.GetComponent<Piece>().y = j;
                        }
                        else
                        {
                            // Place a king.
                            GameObject king = Instantiate(king_black_prefab, grid.transform);
                            NetworkServer.Spawn(king);
                            tile_array[i, j].GetComponent<Tile>().occupied = true;
                            tile_array[i, j].GetComponent<Tile>().game_piece = king;
                            king.GetComponent<Piece>().my_tile = tile_array[i, j];
                            king.transform.position += new Vector3(i * shift_amount, 0, j * shift_amount);
                            king.transform.position = new Vector3(tile_array[i, j].transform.position.x, 0.542f, tile_array[i, j].transform.position.z);
                            king.GetComponent<Piece>().x = i;
                            king.GetComponent<Piece>().y = j;
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
