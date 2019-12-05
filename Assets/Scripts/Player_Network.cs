using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Network : NetworkBehaviour
{
    public GameObject camera_holding_object;

    public override void OnStartLocalPlayer()
    {
        GetComponent<CameraScript>().enabled = true;
        camera_holding_object.SetActive(true);

        

    }
}
