using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerationTool
{
    public static Noeud[] GenerateGraph(GraphSetting setting)
    {
        List<Noeud> nodes = new List<Noeud>();
        int gridWidth = Random.Range(setting.gridWidth.x, setting.gridWidth.y);
        int gridHeight = Random.Range(setting.gridHeight.x, setting.gridHeight.y);
        int[,] grid = new int[gridWidth, gridHeight];

        //Create Root Node
        Vector2 pos = Vector2.zero;
        Noeud rootNode = new Noeud(pos, Noeud.TYPE_DE_NOEUD.START);
        // Create new node
        grid[(int)(pos.x) + gridWidth / 2, (int)(pos.y) + gridHeight / 2] = 0;
        nodes.Add(rootNode);

        //Critical Path Generation
        int criticalpathLength = Random.Range(setting.criticalPathLength.x, setting.criticalPathLength.y+1);
        bool result = GeneratePath(ref nodes, ref grid, 0, criticalpathLength);
        if (!result)
            return null;
        nodes[nodes.Count - 1].type = Noeud.TYPE_DE_NOEUD.END;

        bool obstacles = GenerateObstacles(
            ref nodes,
            ref grid,
            Random.Range(setting.obstacleCount.x, setting.obstacleCount.y + 1),
            Random.Range(setting.secondaryPathLength.x, setting.secondaryPathLength.y + 1));

        if (!obstacles)
            return null;

        return nodes.ToArray();
    }

    private static bool GenerateObstacles(ref List<Noeud> nodes, ref int[,] grid, int obstacleCount, int secondaryPath)
    {
        List<int> obstacle = new List<int>();
        int indexObstacle = -1;
        while (obstacle.Count < obstacleCount)
        {
            do
            {
                indexObstacle = Random.Range(1, nodes.Count - 1);
            } while (obstacle.Contains(indexObstacle));
            obstacle.Add(indexObstacle);
        }
        foreach (int index in obstacle)
        {
            nodes[index].type = Noeud.TYPE_DE_NOEUD.OBSTACLE;
            nodes[index].liens[index + 1] = Noeud.TYPE_DE_LIEN.CLOSE;
            nodes[index + 1].liens[index] = Noeud.TYPE_DE_LIEN.CLOSE;

            bool resultSecondary = GeneratePath(ref nodes, ref grid, index, secondaryPath);
            if (!resultSecondary)
                return false;

            nodes[nodes.Count - 1].type = Noeud.TYPE_DE_NOEUD.KEY;
        }
        return true;
    }
    #region Path

    private static bool GeneratePath(ref List<Noeud> nodes , ref int[,] grid, int rootNode, int pathLength)
    {
        Vector2 pos = nodes[rootNode].position;
        Vector2 direction = Vector2.zero;
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);
        int startIndex = nodes.Count;
        for (int i = startIndex; i < pathLength + startIndex; i++)
        {
            //Direction
            direction = NewDirection(nodes, grid, pos, direction);

            // if no direction return no graph 
            if (direction == Vector2.zero)
            {
                return false;
            }
            else
                pos += direction;

            // Create new node
            Noeud node = new Noeud(pos, Noeud.TYPE_DE_NOEUD.INTERMEDIATE);
            grid[(int)(pos.x) + gridWidth / 2, (int)(pos.y) + gridHeight / 2] = i;

            //Bottom Link
            if(i == startIndex)
            {
                node.liens.Add(rootNode, Noeud.TYPE_DE_LIEN.OPEN);
            }
            else
            {
                node.liens.Add(i - 1, Noeud.TYPE_DE_LIEN.OPEN);
            }

            //Up Link
            if (i < pathLength + startIndex - 1)
            {
                node.liens.Add(i + 1, Noeud.TYPE_DE_LIEN.OPEN);
            }

            nodes.Add(node);
        }
        return true;
    }
    #endregion

    #region Direction
    private static Vector2 NewDirection(List<Noeud> nodes, int[,] grid, Vector2 position, Vector2 previousDirection)
    {
        Vector2 direction = Vector2.zero;
        bool top, right, bottom, left;
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        //  RIGHT
        if (Mathf.Abs(position.x + 1) >= gridWidth / 2 ||
            previousDirection.x == -1 ||
                (position.x + 1 == 0 && position.y == 0))
            right = false;
        else if (grid[(int)(position.x + 1) + gridWidth / 2, (int)(position.y) + gridHeight / 2] != 0)
            right = false;
        else
            right = true;
        
        //  LEFT
        if (Mathf.Abs(position.x - 1) >= gridWidth / 2 ||
            previousDirection.x == 1 ||
                (position.x - 1 == 0 && position.y == 0))
            left = false;
        else if (grid[(int)(position.x - 1) + gridWidth / 2, (int)(position.y) + gridHeight / 2] != 0)
            left = false;
        else
            left = true;

        //  TOP
        if (Mathf.Abs(position.y + 1) >= gridHeight / 2 ||
            previousDirection.y == -1 ||
                (position.x == 0 && position.y + 1 == 0))
            top = false;
        else if (grid[(int)(position.x) + gridWidth / 2, (int)(position.y + 1) + gridHeight / 2] != 0)
            top = false;
        else
            top = true;

        //  BOTTOM
        if (Mathf.Abs(position.y - 1) >= gridHeight / 2 ||
            previousDirection.y == 1 ||
                (position.x == 0 && position.y - 1 == 0))
            bottom = false;
        else if (grid[(int)(position.x) + gridWidth / 2, (int)(position.y - 1) + gridHeight / 2] != 0)
            bottom = false;
        else
            bottom = true;

        // if no available direction return no direction
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
    #endregion
}
