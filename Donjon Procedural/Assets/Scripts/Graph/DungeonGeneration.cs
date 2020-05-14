using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public GraphSetting currentSetting;
    public RoomsDatabase roomsPrefabs;
    public DifficultySetting difficultySetting;
    public Vector2 roomSize;
    public int iterationMax = 100;
    [HideInInspector] public Noeud[] currentGraph = null;
    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
    }
    public void GenerateDungeon()
    {
        int i = 0;
        do
        {
            currentGraph = GraphGenerationTool.GenerateGraph(currentSetting);
            i++;
        } while (currentGraph == null && i < iterationMax);
        if (currentGraph == null)
        {
            Debug.LogError("Damn we are in trouble ...");
        }
        else
        {
            currentGraph = DungeonGenerationTool.GenerateDungeon(difficultySetting, roomSize, currentGraph, roomsPrefabs.rooms.ToArray(), transform);
        }
    }
}
