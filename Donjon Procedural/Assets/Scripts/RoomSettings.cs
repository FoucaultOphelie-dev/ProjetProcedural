using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSettings : MonoBehaviour
{
    public Noeud.TYPE_DE_NOEUD type;
    public int dangerosity = 1;
    public Door doorUp;
    public Door doorDown;
    public Door doorLeft;
    public Door doorRight;
    private string flag;

    [HideInInspector]
    public int roomFlag;
    
    

    // Start is called before the first frame update
    void Start()
    {
        flag = "0x" + (int)doorRight.obligations + (int)doorLeft.obligations + (int)doorUp.obligations + (int)doorDown.obligations;
        roomFlag = Convert.ToInt32(flag, 16);
    }
}
