using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GraphViewer : MonoBehaviour
{
    public GraphSetting currentSetting;
    public int iterationMax = 1;
    [HideInInspector] public Noeud[] currentGraph = null;
    public bool everyFrame;

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
            Gizmos.DrawCube(new Vector3(node.position.x, node.position.y), new Vector2(0.5f, 0.5f));

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
                        Gizmos.color = Color.green;
                        break;
                    default:
                        break;
                }
                Gizmos.DrawLine(new Vector3(node.position.x, node.position.y), new Vector3(currentGraph[lien.Key].position.x, currentGraph[lien.Key].position.y));
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
        GUILayout.Label("Mode : " + everyFrame);
    }

    public void GenerateGraph()
    {
        int i = 0;
        do
        {
            Debug.Log("Generate");
            currentGraph = GraphGenerationTool.GenerateGraph(currentSetting);
            i++;
        } while (currentGraph == null && i < iterationMax);
        if (currentGraph == null)
        {
            Debug.LogError("Damn we are in trouble ...");
        }
    }
}
