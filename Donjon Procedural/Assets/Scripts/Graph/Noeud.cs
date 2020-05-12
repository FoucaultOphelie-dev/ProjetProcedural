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

    public enum TYPE_DE_NOEUD
    {
        START,
        END,
        KEY,
        OBSTACLE,
        INTERMEDIATE
    };

    public TYPE_DE_NOEUD type;
    public GameObject sallePrefab;
    public Vector2 position;
    public Dictionary<int, TYPE_DE_LIEN> liens;
    private Vector2 vector2;

    public Noeud(Vector2 vector2, TYPE_DE_NOEUD type)
    {
        this.position = vector2;
        this.type = type;
        liens = new Dictionary<int, TYPE_DE_LIEN>();
    }
}
