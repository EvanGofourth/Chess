using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Network : NetworkBehaviour
{
    public GameObject camera_holding_object;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray toMouse = this.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit rhInfo;
            bool didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);
            if (didHit)
            {
                GameObject.Find("GameControllerObj").GetComponent<GameController>().CmdGetFunky();
            }
            else
            {
                Debug.Log("clicked on nothing..");
            }
        }
    }
}
