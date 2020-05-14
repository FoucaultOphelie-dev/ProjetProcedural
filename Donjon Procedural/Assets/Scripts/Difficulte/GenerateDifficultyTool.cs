using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateDifficultyTool
{

    public static Noeud[] GenerateDifficulty(DifficultySetting difficultySetting, Noeud[] graph)
    {
        List<float> listOfDifficulty = GenerateListNumberDificulty(difficultySetting, graph);
        
        for (int i = 0; i < listOfDifficulty.Count; i++)
        {
            graph[i].dangerosityValue = listOfDifficulty[i];
            graph[i].dangerosity = randomStat(
                difficultySetting.courbeEasy.Evaluate(listOfDifficulty[i]),
                difficultySetting.courbeIntermediate.Evaluate(listOfDifficulty[i]),
                difficultySetting.courbeDificult.Evaluate(listOfDifficulty[i]));
        }
        
        return graph;
    }

    private static List<float> GenerateListNumberDificulty(DifficultySetting difficultySetting, Noeud[] graph)
    {
        //CRITICAL PATH LENGTH
        int criticalPathLength = 0;
        List<float> listOfDifficulty = new List<float>();
        for (int i = 0; graph[i].type != Noeud.TYPE_DE_NOEUD.END; i++)
        {
            criticalPathLength++;
        }
        criticalPathLength++;

        //CRITICAL PATH DANGEROSITY
        float dangerosityTotal = 0;
        float dangerosityStep = 1f / criticalPathLength;
        for (int i = 0; i < criticalPathLength; i++)
        {
            listOfDifficulty.Add(difficultySetting.courbeDeDifficulte.Evaluate(dangerosityTotal));
            dangerosityTotal += dangerosityStep;
        }

        //SECONDARY PATH DANGEROSITY
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
