using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noeud
{
    public enum TYPE_DE_LIEN
    {
        CLOSE,
        OPEN,
        SECRET
    };
    public GameObject sallePrefab;
    public Vector2 position;
    public Dictionary<int, TYPE_DE_LIEN> liens;
    private Vector2 vector2;

    public Noeud(Vector2 vector2)
    {
        this.position = vector2;
        liens = new Dictionary<int, TYPE_DE_LIEN>();
    }
}
