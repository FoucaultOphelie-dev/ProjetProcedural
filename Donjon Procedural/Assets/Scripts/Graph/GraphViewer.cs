using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteAlways]
public class GraphViewer : MonoBehaviour
{
    public enum DebugColorMode
    {
        Type,
        Dangerosity
    }
    public DebugColorMode debugColorMode;
    public GraphSetting currentSetting;
    public RoomsDatabase roomsPrefabs;
    public DifficultySetting difficultySetting;
    public bool showDangerosityValue;
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
            switch (debugColorMode)
            {
                case DebugColorMode.Type:
                    Gizmos.color = GetRoomColorByType(node.type);
                    break;
                case DebugColorMode.Dangerosity:
                    Gizmos.color = GetRoomColorByDangerosity(node.dangerosity);
                    break;
                default:
                    break;
            }
            if (!showDangerosityValue)
                Gizmos.DrawCube((Vector2)node.position * roomSize, roomSize);
#if UNITY_EDITOR
            if (showDangerosityValue)
            {
                Handles.DrawWireCube((Vector2)node.position * roomSize, roomSize);
                Handles.Label((Vector2)node.position * roomSize, node.dangerosityValue.ToString());
            }
#endif

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
        if (currentGraph == null)
        {
            Debug.LogError("Damn we are in trouble ...");
        }
        else
        {
            currentGraph = GenerateDifficultyTool.GenerateDifficultyValue(difficultySetting, currentGraph);
            currentGraph = DungeonGenerationTool.GenerateDungeon(difficultySetting, currentGraph, new List<GameObject>());
        }
        Debug.Log(System.Math.Round((Time.realtimeSinceStartup - starttime) * 1000, 2) + "ms (try:" + i + ")");
    }

    private Color GetRoomColorByType(Noeud.TYPE_DE_NOEUD type)
    {
        switch (type)
        {
            case Noeud.TYPE_DE_NOEUD.START:
                return Color.green;
            case Noeud.TYPE_DE_NOEUD.END:
                return Color.red;
            case Noeud.TYPE_DE_NOEUD.KEY:
                return Color.yellow;
            case Noeud.TYPE_DE_NOEUD.OBSTACLE:
                return Color.cyan;
            case Noeud.TYPE_DE_NOEUD.INTERMEDIATE:
                return Color.white;
            case Noeud.TYPE_DE_NOEUD.SECRET:
                return Color.magenta;
            default:
                return Color.white;
        }
    }

    private Color GetRoomColorByDangerosity(RoomSettings.DANGEROSITY dangerosity)
    {
        switch (dangerosity)
        {
            case RoomSettings.DANGEROSITY.EASY:
                return Color.green;
            case RoomSettings.DANGEROSITY.INTERMEDIATE:
                return new Color(255, 165, 0);
            case RoomSettings.DANGEROSITY.DIFFICULT:
                return Color.red;
            default:
                return Color.white;
        }
    }
}
