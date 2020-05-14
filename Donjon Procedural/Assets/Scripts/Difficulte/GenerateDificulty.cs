using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateDificulty
{
    public static List<RoomSettings.DANGERORITY> GenerateDifficulty(DificultySetting difficultySetting, Noeud[] graph)
    {
        List<float> listOfDifficulty = GenerateListNumberDificulty(difficultySetting,graph);
        List<RoomSettings.DANGERORITY> difficulty = new List<RoomSettings.DANGERORITY>();

        foreach (float dif in listOfDifficulty)
        {
            difficulty.Add(randomStat(difficultySetting.courbeEasy.Evaluate(dif), difficultySetting.courbeIntermediate.Evaluate(dif), difficultySetting.courbeDificult.Evaluate(dif)));
        }
        
        return difficulty;
    }

    private static List<float> GenerateListNumberDificulty(DificultySetting difficultySetting, Noeud[] graph)
    {
        int criticalPathLength = 0;
        float gap = 0;
        List<float> listOfDifficulty = new List<float>();
        for (int i = 0; graph[i].type != Noeud.TYPE_DE_NOEUD.END; i++)
        {
            criticalPathLength += 1;
        }
        criticalPathLength += 2;
        gap = 1 / criticalPathLength;

        for (float i = 0; i < 1; i += gap)
        {
            listOfDifficulty.Add(difficultySetting.courbeDeDifficulte.Evaluate(i));
        }

        for (int i = criticalPathLength; i < graph.Length; i++)
        {
            foreach (int key in graph[i].liens.Keys)
            {
                if (key != i+1)
                {
                    listOfDifficulty.Add(listOfDifficulty[key]);
                    break;
                }
            }
        }

        return listOfDifficulty;
    }

    private static RoomSettings.DANGERORITY randomStat(float Easy, float Intermediate, float Difficult)
    {
        float total = Easy + Intermediate + Difficult;
        float probaEasy = Easy / total;
        float probaIntermediate = Intermediate / total;
        float probaDifficult = Difficult / total;

        float proba = Random.Range(0, 1);
        if (proba < probaEasy)
        {
            return RoomSettings.DANGERORITY.EASY;
        }
        else if (proba < probaEasy + probaIntermediate)
        {
            return RoomSettings.DANGERORITY.INTERMEDIATE;
        }
        else
        {
            return RoomSettings.DANGERORITY.DIFFICULT;
        }
    }
}
