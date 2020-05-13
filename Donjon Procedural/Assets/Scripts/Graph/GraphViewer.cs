using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GraphViewer : MonoBehaviour
{
    public GraphSetting currentSetting;
    public GameObject roomPrefabs;
    public Vector2 roomSize;
    public int iterationMax = 1;
    [Range(0f,1f)]
    public float RefreshTime = 0.5f;
    public bool loopGeneration;
    private float timer = 0f;
    [HideInInspector] public Noeud[] currentGraph = null;
    private void Start()
    {
        if(currentGraph != null)
        {
            DeleteGraph();
        }
        GenerateDungeon();
    }

    private void OnDrawGizmos()
    {
        if (currentGraph == null) return;
        //Draw Node
        foreach(Noeud node in currentGraph)
        {
            switch (node.type)
            {
                case Noeud.TYPE_DE_NOEUD.START:
                    Gizmos.color = Color.green;
                    break;
                case Noeud.TYPE_DE_NOEUD.END:
                    Gizmos.color = Color.red;
                    break;
                case Noeud.TYPE_DE_NOEUD.KEY:
                    Gizmos.color = Color.yellow;
                    break;
                case Noeud.TYPE_DE_NOEUD.OBSTACLE:
                    Gizmos.color = Color.cyan;
                    break;
                case Noeud.TYPE_DE_NOEUD.INTERMEDIATE:
                    Gizmos.color = Color.white;
                    break;
                case Noeud.TYPE_DE_NOEUD.SECRET:
                    Gizmos.color = Color.magenta;
                    break;
                default:
                    break;
            }
            Gizmos.DrawCube((Vector2)node.position * roomSize, roomSize);

            //Draw Link
            foreach (KeyValuePair<int, Noeud.TYPE_DE_LIEN> lien in node.liens)
            {
                switch (lien.Value)
                {
                    case Noeud.TYPE_DE_LIEN.CLOSE:
                        Gizmos.color = Color.red;
                        break;
                    case Noeud.TYPE_DE_LIEN.OPEN:
                        Gizmos.color = Color.white;
                        break;
                    case Noeud.TYPE_DE_LIEN.SECRET:
                        Gizmos.color = Color.magenta;
                        break;
                    default:
                        break;
                }
                Gizmos.DrawLine((Vector2)node.position * roomSize, (Vector2)currentGraph[lien.Key].position * roomSize);
            }
        }
    }

    public void DeleteGraph()
    {
        currentGraph = null;
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (loopGeneration && timer >= RefreshTime)
        {
            GenerateDungeon();
            timer = 0;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate")){
            GenerateDungeon();
        }

        if (GUILayout.Button("Generate YOLO Mode"))
        {
            loopGeneration = !loopGeneration;
        }
        GUILayout.Label("Mode : " + loopGeneration);
    }

    public void GenerateDungeon()
    {
        if (currentGraph != null)
            DeleteGraph();
        float starttime = Time.realtimeSinceStartup;
        int i = 0;
        do
        {
            currentGraph = GraphGenerationTool.GenerateGraph(currentSetting);
            //Camera.main.transform.position = new Vector3(gridWidth / 2, gridHeight / 2, -10);
            i++;
        } while (currentGraph == null && i < iterationMax);
        if (currentGraph != null && roomPrefabs != null)
        {
            foreach(Noeud node in currentGraph)
            {
                Instantiate(roomPrefabs, (node.position * roomSize) - roomSize/2, Quaternion.identity, transform);
            }
        }
        else
        {
            Debug.LogError("Damn we are in trouble ...");
        }
        Debug.Log(System.Math.Round((Time.realtimeSinceStartup - starttime) * 1000, 2) + "ms (try:" + i + ")");
    }
}
