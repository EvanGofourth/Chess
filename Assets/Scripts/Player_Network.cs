using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Network : NetworkBehaviour
{
    public GameObject camera_holding_object;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }
}
