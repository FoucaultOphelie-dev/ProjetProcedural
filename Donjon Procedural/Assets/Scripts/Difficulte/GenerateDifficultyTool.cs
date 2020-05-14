using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateDifficultyTool
{
    public static Noeud[] GenerateDifficultyValue(DifficultySetting difficultySetting, Noeud[] graph)
    {
        List<float> nodeDifficultyValue = GenerateListNumberDificulty(difficultySetting, graph);
        for (int i = 0; i < graph.Length; i++)
        {
            graph[i].dangerosityValue = nodeDifficultyValue[i];
        }
        return graph;
    }

    public static List<RoomSettings.DANGEROSITY> GenerateDifficulty(DifficultySetting difficultySetting, Noeud[] graph)
    {
        List<float> listOfDifficulty = GenerateListNumberDificulty(difficultySetting, graph);
        List<RoomSettings.DANGEROSITY> difficulty = new List<RoomSettings.DANGEROSITY>();

        foreach (float dif in listOfDifficulty)
        {
            difficulty.Add(randomStat(
                difficultySetting.courbeEasy.Evaluate(dif),
                difficultySetting.courbeIntermediate.Evaluate(dif),
                difficultySetting.courbeDificult.Evaluate(dif)));
        }
        
        return difficulty;
    }

    private static List<float> GenerateListNumberDificulty(DifficultySetting difficultySetting, Noeud[] graph)
    {
        int criticalPathLength = 0;
        float gap = 0;
        List<float> listOfDifficulty = new List<float>();
        for (int i = 0; graph[i].type != Noeud.TYPE_DE_NOEUD.END; i++)
        {
            criticalPathLength++;
        }
        criticalPathLength++;
        float dangerosityStep = 1f / criticalPathLength;
        for (int i = 0; i < criticalPathLength; i++)
        {
            listOfDifficulty.Add(difficultySetting.courbeDeDifficulte.Evaluate(gap));
            gap += dangerosityStep;
        }

        for (int i = criticalPathLength; i < graph.Length; i++)
        {
            foreach (int key in graph[i].liens.Keys)
            {
                if (key < i)
                {
                    listOfDifficulty.Add(listOfDifficulty[key]);
                    break;
                }
            }
        }

        return listOfDifficulty;
    }

    private static RoomSettings.DANGEROSITY randomStat(float Easy, float Intermediate, float Difficult)
    {
        float total = Easy + Intermediate + Difficult;
        float probaEasy = Easy / total;
        float probaIntermediate = Intermediate / total;
        float probaDifficult = Difficult / total;

        float proba = Random.Range(0f, 1f);
        if (proba < probaEasy)
        {
            return RoomSettings.DANGEROSITY.EASY;
        }
        else if (proba < probaEasy + probaIntermediate)
        {
            return RoomSettings.DANGEROSITY.INTERMEDIATE;
        }
        else
        {
            return RoomSettings.DANGEROSITY.DIFFICULT;
        }
    }
}
