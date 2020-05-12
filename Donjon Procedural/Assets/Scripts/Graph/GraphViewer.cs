using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GraphViewer : MonoBehaviour
{
    public GraphSetting currentSetting;
    public int iterationMax = 1;
    [Range(0f,1f)]
    public float RefreshTime = 0.5f;
    private float timer = 0f;
    [HideInInspector] public Noeud[] currentGraph = null;
    public bool everyFrame;

    private int gridWidth = 0;
    private int gridHeight = 0;

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
                        Gizmos.color = Color.magenta;
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
        if(everyFrame && timer >= RefreshTime)
        {
            GenerateGraph();
            timer = 0;
        }
        timer += Time.deltaTime;
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
            currentGraph = GraphGenerationTool.GenerateGraph(currentSetting, ref gridWidth, ref gridHeight);
            Camera.main.transform.position = new Vector3(gridWidth / 2, gridHeight / 2, -10);
            i++;
        } while (currentGraph == null && i < iterationMax);
        if (currentGraph == null)
        {
            Debug.LogError("Damn we are in trouble ...");
        }
    }
}
