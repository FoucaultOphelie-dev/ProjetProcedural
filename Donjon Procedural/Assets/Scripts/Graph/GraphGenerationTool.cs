using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerationTool
{
    public static Noeud[] GenerateGraph(GraphSetting setting)
    {
        Debug.Log("Generate");
        List<Noeud> nodes = new List<Noeud>();
        int gridWidth = Random.Range(setting.gridWidth.x, setting.gridWidth.y);
        int gridHeight = Random.Range(setting.gridHeight.x, setting.gridHeight.y);
        int[,] grid = new int[gridWidth, gridHeight];
        Vector2 pos = Vector2.zero;
        Vector2 direction = Vector2.zero;
        int criticalpathLength = Random.Range(setting.criticalPathLength.x, setting.criticalPathLength.y);
        for (int i = 0; i < criticalpathLength; i++)
        {
            // Create new node
            Noeud node = new Noeud(pos);
            grid[(int)(pos.x) + gridWidth / 2, (int)(pos.y) + gridHeight / 2] = i;

            //Bottom Link
            if (i > 0)
                node.liens.Add(i - 1, Noeud.TYPE_DE_LIEN.OPEN);
            //Up Link
            if(i < setting.criticalPathLength.x - 1)
                node.liens.Add(i + 1, Noeud.TYPE_DE_LIEN.OPEN);

            //Direction
            direction = NewDirection(nodes, grid, gridWidth, gridHeight, pos, direction);

            if (direction == Vector2.zero)
            {
                return null;
            }
            else
                pos += direction;

            nodes.Add(node);
        }
        int obstacleCount = Random.Range(setting.obstacleCount.x, setting.obstacleCount.y);
        int[] obstacle = new int[obstacleCount];
        return nodes.ToArray();
    }

    private static Vector2 NewDirection(List<Noeud> nodes, int[,] grid, int gridWidth, int gridHeight, Vector2 position, Vector2 previousDirection)
    {
        Vector2 direction = Vector2.zero;
        bool top, right, bottom, left;

        if (Mathf.Abs(position.x + 1) >= gridWidth / 2 ||
            previousDirection.x == -1 ||
                (position.x + 1 == 0 && position.y == 0))
            right = false;
        else if (grid[(int)(position.x + 1) + gridWidth / 2, (int)(position.y) + gridHeight / 2] != 0)
            right = false;
        else
            right = true;

        if (Mathf.Abs(position.x - 1) >= gridWidth / 2 ||
            previousDirection.x == 1 ||
                (position.x - 1 == 0 && position.y == 0))
            left = false;
        else if (grid[(int)(position.x - 1) + gridWidth / 2, (int)(position.y) + gridHeight / 2] != 0)
            left = false;
        else
            left = true;

        if (Mathf.Abs(position.y + 1) >= gridHeight / 2 ||
            previousDirection.y == -1 ||
                (position.x == 0 && position.y + 1 == 0))
            top = false;
        else if (grid[(int)(position.x) + gridWidth / 2, (int)(position.y + 1) + gridHeight / 2] != 0)
            top = false;
        else
            top = true;

        if (Mathf.Abs(position.y - 1) >= gridHeight / 2 ||
            previousDirection.y == 1 ||
                (position.x == 0 && position.y - 1 == 0))
            bottom = false;
        else if (grid[(int)(position.x) + gridWidth / 2, (int)(position.y - 1) + gridHeight / 2] != 0)
            bottom = false;
        else
            bottom = true;

        if (!top && !right && !bottom && !left)
            return Vector2.zero;
        else
            return GenerateNewDirection(top, right, bottom, left);
    }

    private static Vector2 GenerateNewDirection(bool top, bool right, bool bottom, bool left)
    {
        Vector2 direction = Vector2.zero;
        if (right && left)
            direction.x = Random.Range(0, 2) == 1 ? 1 : -1;
        else if (right)
            direction.x = 1;
        else if (left)
            direction.x = -1;
        else
            direction.x = 0;

        if (top && bottom)
            direction.y = Random.Range(0, 2) == 1 ? 1 : -1;
        else if (top)
            direction.y = 1;
        else if (bottom)
            direction.y = -1;
        else
            direction.y = 0;

        if (direction.x != 0 && direction.y != 0)
        {
            if(Random.Range(0, 2) == 0)
            {
                //Keep horizontal & trash vertical
                direction.y = 0;
            }
            else
            {
                //Keep vertical & trash horizontal
                direction.x = 0;
            }
        }
        return direction;
    }
}
