using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DungeonGenerationTool
{
    public static Noeud[] GenerateDungeon(DifficultySetting difficultySetting, Vector2 roomSize, Noeud[] graph, GameObject[] roomsPrefabs, Transform parent)
    {
        graph = GenerateDifficultyTool.GenerateDifficulty(difficultySetting, graph);
        foreach ( GameObject go in roomsPrefabs)
        {
            go.GetComponent<RoomSettings>().Initialisation();
        }
        graph = FindRooms(roomsPrefabs, graph);
        foreach (Noeud node in graph)
        {
            node.sallePrefab = UnityEngine.Object.Instantiate(node.sallePrefab, (Vector2)node.position * roomSize - roomSize / 2, Quaternion.identity, parent);
            node.sallePrefab.GetComponent<Room>().position = node.position;
        }
        graph = PlaceDoor(graph);
        return graph;
    }

    private static Noeud[] FindRooms(GameObject[] dataBase, Noeud[] graph)
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
            node.sallePrefab = roomsPossible[UnityEngine.Random.Range(0, roomsPossible.Count)];
        }
        return graph;
    }

    private static Noeud[] PlaceDoor(Noeud[] graph)
    {
        foreach (Noeud node in graph)
        {
            RoomSettings roomSettings = node.sallePrefab.GetComponent<RoomSettings>();
            foreach (int key in node.liens.Keys)
            {
                if (graph[key].position.x < node.position.x)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            roomSettings.doorLeft.SetState(Door.STATE.CLOSED);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            roomSettings.doorLeft.SetState(Door.STATE.OPEN);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            roomSettings.doorLeft.SetState(Door.STATE.SECRET);
                            break;
                    }
                }

                if (graph[key].position.x > node.position.x)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            roomSettings.doorRight.SetState(Door.STATE.CLOSED);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            roomSettings.doorRight.SetState(Door.STATE.OPEN);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            roomSettings.doorRight.SetState(Door.STATE.SECRET);
                            break;
                    }
                }

                if (graph[key].position.y > node.position.y)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            roomSettings.doorUp.SetState(Door.STATE.CLOSED);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            roomSettings.doorUp.SetState(Door.STATE.OPEN);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            roomSettings.doorUp.SetState(Door.STATE.SECRET);
                            break;
                    }
                }

                if (graph[key].position.y < node.position.y)
                {
                    switch (node.liens[key])
                    {
                        case Noeud.TYPE_DE_LIEN.CLOSE:
                            roomSettings.doorDown.SetState(Door.STATE.CLOSED);
                            break;
                        case Noeud.TYPE_DE_LIEN.OPEN:
                            roomSettings.doorDown.SetState(Door.STATE.OPEN);
                            break;
                        case Noeud.TYPE_DE_LIEN.SECRET:
                            roomSettings.doorDown.SetState(Door.STATE.SECRET);
                            break;
                    }
                }
            }
        }
        return graph;
    }

}
