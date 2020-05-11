using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GraphViewer : MonoBehaviour
{
    public GraphSetting currentSetting;
    private Noeud[] currentGraph = null;
    private bool everyFrame;

    private void OnDrawGizmos()
    {
        if (currentGraph == null) return;
        //Draw Node
        foreach(Noeud node in currentGraph)
        {
            Gizmos.DrawCube(node.position, new Vector2(0.5f, 0.5f));

            //Draw Link
            foreach (KeyValuePair<int, Noeud.TYPE_DE_LIEN> lien in node.liens)
            {
                Gizmos.DrawLine(node.position, currentGraph[lien.Key].position);
            }
        }
    }

    private void Update()
    {
        if(everyFrame)
            GenerateGraph();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate")){
            GenerateGraph();
        }

        if (GUILayout.Button("Generate YOLO Mode"))
        {
            everyFrame = !everyFrame;
        }
    }

    private void GenerateGraph()
    {
        int i = 0;
        int maxIteration = 10;
        do
        {
            currentGraph = GraphGenerationTool.GenerateGraph(currentSetting);
            i++;
        } while (currentGraph == null && i < maxIteration);
        if (currentGraph == null)
        {
            Debug.LogError("Damn we are in trouble ...");
        }
    }
}
