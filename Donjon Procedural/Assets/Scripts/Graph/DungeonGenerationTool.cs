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

    public static Noeud[] FindRooms(GameObject[] dataBase, Noeud[] graph)
    {
        List<int> criteres = new List<int>() { 0x4000, 0x0400, 0x0040, 0x0004 };
        foreach (Noeud node in graph)
        {
            List<GameObject> roomsPossible = new List<GameObject>();
            foreach (int key in node.liens.Keys)
            {
                if (graph[key].position.x > node.position.x)
                {
                    criteres[0] = 0x1000;
                }
                if (graph[key].position.x < node.position.x)
                {
                    criteres[0] = 0x0100;
                }
                if (graph[key].position.y > node.position.y)
                {
                    criteres[0] = 0x0010;
                }
                if (graph[key].position.y < node.position.y)
                {
                    criteres[0] = 0x0001;
                }
            }

            foreach (GameObject go in dataBase)
            {
                bool result = true;
                foreach (int critere in criteres)
                {
                    if ((go.GetComponent<RoomSettings>().roomFlag | critere) == go.GetComponent<RoomSettings>().roomFlag)
                    {
                        result = false;
                    }
                }
                if (result && node.type == go.GetComponent<RoomSettings>().type && node.dangerosity == go.GetComponent<RoomSettings>().dangerosity)
                {
                    roomsPossible.Add(go);
                }
                else if (node.type == Noeud.TYPE_DE_NOEUD.OBSTACLE)
                {
                    if(result && node.dangerosity == go.GetComponent<RoomSettings>().dangerosity && go.GetComponent<RoomSettings>().type==Noeud.TYPE_DE_NOEUD.INTERMEDIATE)
                    {
                        roomsPossible.Add(go);
                    }
                }
            }

            node.sallePrefab = roomsPossible[Random.Range(0, roomsPossible.Count + 1)];
        }
        return graph;
    }

    private static Noeud[] placeDoor(Noeud[] graph)
    {
        foreach (Noeud node in graph)
        {
            foreach (int key in node.liens.Keys)
            {
                if (graph[key].position.x > node.position.x)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            node.sallePrefab.GetComponent<RoomSettings>().doorLeft.closedGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorLeft.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            node.sallePrefab.GetComponent<RoomSettings>().doorLeft.openGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorLeft.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            node.sallePrefab.GetComponent<RoomSettings>().doorLeft.secretGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorLeft.wallGo.SetActive(false);
                            break;
                    }
                }
                if (graph[key].position.x < node.position.x)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            node.sallePrefab.GetComponent<RoomSettings>().doorRight.closedGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorRight.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            node.sallePrefab.GetComponent<RoomSettings>().doorRight.openGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorRight.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            node.sallePrefab.GetComponent<RoomSettings>().doorRight.secretGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorRight.wallGo.SetActive(false);
                            break;
                    }
                }
                if (graph[key].position.y > node.position.y)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            node.sallePrefab.GetComponent<RoomSettings>().doorUp.closedGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorUp.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            node.sallePrefab.GetComponent<RoomSettings>().doorUp.openGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorUp.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            node.sallePrefab.GetComponent<RoomSettings>().doorUp.secretGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorUp.wallGo.SetActive(false);
                            break;
                    }
                }
                if (graph[key].position.y < node.position.y)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            node.sallePrefab.GetComponent<RoomSettings>().doorDown.closedGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorDown.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            node.sallePrefab.GetComponent<RoomSettings>().doorDown.openGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorDown.wallGo.SetActive(false);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            node.sallePrefab.GetComponent<RoomSettings>().doorDown.secretGo.SetActive(true);
                            node.sallePrefab.GetComponent<RoomSettings>().doorDown.wallGo.SetActive(false);
                            break;
                    }
                }
            }
        }
        return graph;
    }

}
