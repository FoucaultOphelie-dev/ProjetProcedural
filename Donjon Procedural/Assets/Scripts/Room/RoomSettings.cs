using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSettings : MonoBehaviour
{
    public enum DANGEROSITY
    {
        EASY,
        INTERMEDIATE,
        DIFFICULT,
    }
    public Noeud.TYPE_DE_NOEUD type = Noeud.TYPE_DE_NOEUD.INTERMEDIATE;
    public DANGEROSITY dangerosity= DANGEROSITY.EASY;
    [HideInInspector]
    public Door doorUp;
    [HideInInspector]
    public Door doorDown;
    [HideInInspector]
    public Door doorLeft;
    [HideInInspector]
    public Door doorRight;
    private Door[] listDoor;
    private string flag;

    [HideInInspector]
    public int roomFlag;
    
    

    // Start is called before the first frame update
    void Start()
    {
        listDoor = GetComponentsInChildren<Door>();
        foreach (Door door in listDoor)
        {
            if (door.gameObject.transform.localRotation.z == 0)
            {
                doorUp = door;
            }
            else if (door.gameObject.transform.localRotation.eulerAngles.z == 180)
            {
                doorDown = door;

            }
            else if (door.gameObject.transform.localRotation.eulerAngles.z == 270)
            {
                doorRight = door;

            }
            else
            {
                doorLeft = door;

            }
        }
        flag = "0x" + (int)doorRight.obligations + (int)doorLeft.obligations + (int)doorUp.obligations + (int)doorDown.obligations;
        roomFlag = Convert.ToInt32(flag, 16);
        Debug.Log(flag);
    }
}
