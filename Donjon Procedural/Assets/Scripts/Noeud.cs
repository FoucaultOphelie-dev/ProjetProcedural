using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class Noeud : MonoBehaviour
{
    public enum TYPE_DE_SALLE
    {
        CLOSE,
        OPEN,
        SECRET
    };
    public TilemapGroup salle;
    public Vector2 position;
    public Dictionary<Noeud, TYPE_DE_SALLE> liens;
}
