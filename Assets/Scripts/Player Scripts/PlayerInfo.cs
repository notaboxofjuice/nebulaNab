using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    static PlayerInfo instance;

    public static bool isRedTeam = false;
    public static bool isBlueTeam = false;


    private void Start()
    {
        instance = this;
    }
}
