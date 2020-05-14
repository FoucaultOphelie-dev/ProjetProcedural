using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newGraphSetting", menuName ="New Graph Setting")]
public class GraphSetting : ScriptableObject
{
    public enum SecretRoomPositionningMode
    {
        MostNeighbour,
        NextToLastKey
    }

    [MinMaxSlider(3,30)]
    public Vector2Int gridWidth;
    [MinMaxSlider(3, 30)]
    public Vector2Int gridHeight;
    [MinMaxSlider(3, 50)]
    public Vector2Int criticalPathLength;
    [MinMaxSlider(1, 5)]
    public Vector2Int obstacleCount;
    [MinMaxSlider(1, 50)]
    public Vector2Int secondaryPathLength;
    [MinMaxSlider(0, 3)]
    public Vector2Int secretNode;
    public SecretRoomPositionningMode secretRoomPositionningMode;

}
