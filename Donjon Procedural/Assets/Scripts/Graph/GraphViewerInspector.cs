using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GraphViewer))]
public class GraphViewerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        GraphViewer viewer = target as GraphViewer;
        if (GUILayout.Button("Generate Dungeon"))
        {
            viewer.GenerateDungeon();
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
        if (GUILayout.Button("Delete Graph"))
        {
            viewer.DeleteGraph();
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
        if (viewer.loopGeneration)
        {
            // Ensure continuous Update calls.
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif