using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "newRoomDatabase", menuName ="Room Database")]
public class RoomsDatabase : ScriptableObject
{
    public List<GameObject> rooms;
}
