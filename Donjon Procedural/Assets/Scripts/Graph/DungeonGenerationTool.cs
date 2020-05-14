using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DungeonGenerationTool
{
    public static Noeud[] GenerateDungeon(DifficultySetting difficultySetting, Noeud[] nodes, List<GameObject> roomsPrefabs)
    {
        List<RoomSettings.DANGEROSITY> dangerosityList = GenerateDifficultyTool.GenerateDifficulty(difficultySetting, nodes);
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].dangerosity = dangerosityList[i];
        }
        return nodes;
    }
}
