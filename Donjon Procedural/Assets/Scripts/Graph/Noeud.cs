using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noeud : MonoBehaviour
{
    public enum TYPE_DE_SALLE
    {
        CLOSE,
        OPEN,
        SECRET
    };
    public GameObject sallePrefab;
    public Vector2 position;
    public Dictionary<Noeud, TYPE_DE_SALLE> liens;
}
