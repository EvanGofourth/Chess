using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Network : NetworkBehaviour
{
    public GameObject camera_holding_object;
    public GameObject[] pieces;

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
        pieces = GameObject.FindGameObjectsWithTag("Piece");
    }

    [ClientRpc]
    public void RpcNextColor(GameObject obj)
    {
        //GameObject THIS_ONE = GameObject.FindGameObjectWithTag("Piece");
      //  THIS_ONE.GetComponent<MeshRenderer>().material = weird;
        //THIS_ONE.transform.position += new Vector3(1, 1, 1);
        obj.transform.position += new Vector3(1, 1, 1);
    }

    [Command]
    public void CmdGetFunky(GameObject obj)
    {
        //GameObject THIS_ONE = GameObject.FindGameObjectWithTag("Piece");
       // THIS_ONE.GetComponent<MeshRenderer>().material = weird;
        //THIS_ONE.transform.position += new Vector3(1, 1, 1);
        obj.transform.position += new Vector3(1, 1, 1);
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
                    //GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().RpcNextColor();
                    RpcNextColor(rhInfo.collider.gameObject);
                }
              else
              {
                    //GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().CmdGetFunky();
                    CmdGetFunky(rhInfo.collider.gameObject);
              }
                
                
            }
            else
            {
                Debug.Log("clicked on nothing..");
            }
        }
    }
}
