using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct NeighbourNode
{
    public Vector2Int position;
    public int score;
    public bool[] access;
}
public static class GraphGenerationTool
{
    private static List<Noeud> nodes = new List<Noeud>();
    private static int[,] grid = null;
    private static int gridWidth = -1;
    private static int gridHeight = -1;
    private readonly static Vector2Int[] neighBours = new Vector2Int[4] { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down } ;

public static Noeud[] GenerateGraph(GraphSetting setting)
    {
        nodes = new List<Noeud>();
        gridWidth = Random.Range(setting.gridWidth.x, setting.gridWidth.y+1);
        gridHeight = Random.Range(setting.gridHeight.x, setting.gridHeight.y+1);
        grid = new int[gridWidth, gridHeight];
        //Create Root Node
        Vector2Int pos = new Vector2Int(gridWidth/2, gridHeight/2);
        Noeud rootNode = new Noeud(pos, Noeud.TYPE_DE_NOEUD.START);
        // Create new node
        grid[pos.x, pos.y] = 0;
        nodes.Add(rootNode);

        //Critical Path Generation
        int criticalpathLength = Random.Range(setting.criticalPathLength.x, setting.criticalPathLength.y+1);
        bool result = GeneratePath(0, criticalpathLength);
        if (!result)
            return null;
        nodes[nodes.Count - 1].type = Noeud.TYPE_DE_NOEUD.END;

        bool obstacles = GenerateObstacles(
            Random.Range(setting.obstacleCount.x, setting.obstacleCount.y + 1),
            Random.Range(setting.secondaryPathLength.x, setting.secondaryPathLength.y + 1));

        if (!obstacles)
            return null;

        GenerateSecretNode(Random.Range(setting.secretNode.x, setting.secretNode.y + 1));

        //Set graph at origin
        for (int i = 1; i < nodes.Count; i++)
        {
            nodes[i].position = nodes[i].position - nodes[0].position;
        }
        nodes[0].position = Vector2Int.zero;

        return nodes.ToArray();
    }

    private static void GenerateSecretNode(int secretNodeCount)
    {
        //init 
        int[,] gridNeighbour = new int[gridWidth, gridHeight];
        bool[,][] gridAccessNeighbour = new bool[gridWidth, gridHeight][];
        List<NeighbourNode> neighbourNodes = new List<NeighbourNode>();
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                gridAccessNeighbour[x, y] = new bool[4];
                for (int o = 0; o < neighBours.Length; o++)
                {
                    if (x + 1 < gridWidth && x - 1 >= 0 && y + 1 < gridHeight && y - 1 >= 0)
                    {
                        gridAccessNeighbour[x, y][o] = !IsCoordEmpty(pos + neighBours[o]);
                        if (gridAccessNeighbour[x, y][o])
                            gridNeighbour[x, y]++;
                    }
                }
                neighbourNodes.Add(new NeighbourNode { position = new Vector2Int(x, y), score = gridNeighbour[x, y], access = gridAccessNeighbour[x, y] });
            }
        }

        neighbourNodes = neighbourNodes.Where(node => IsCoordEmpty(node.position)).OrderByDescending(node => node.score).ToList();
        int i = 0;
        int k = 0;
        while(i < secretNodeCount)
        {
            Noeud node = new Noeud(neighbourNodes[k].position, Noeud.TYPE_DE_NOEUD.SECRET);
            nodes.Add(node);
            grid[neighbourNodes[k].position.x,
                neighbourNodes[k].position.y] = i;
            List<int> valideNeighbour = new List<int>();
            for (int j = 0; j < neighbourNodes[k].access.Length; j++)
            {
                if (neighbourNodes[k].access[j])
                {
                    int nodeIndex = GetNodeIndexFromPos(neighbourNodes[k].position + neighBours[j]);
                    if (nodes[nodeIndex].type != Noeud.TYPE_DE_NOEUD.END)
                        valideNeighbour.Add(nodeIndex);
                }
            }
            if(valideNeighbour.Count > 0)
            {
                int selectedIndex = valideNeighbour[Random.Range(0, valideNeighbour.Count)];
                node.liens.Add(selectedIndex, Noeud.TYPE_DE_LIEN.SECRET);
                nodes[selectedIndex].liens.Add(nodes.Count - 1, Noeud.TYPE_DE_LIEN.SECRET);
                i++;
            }
            k++;
        }
    }

    private static bool GenerateObstacles(int obstacleCount, int secondaryPath)
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

            bool resultSecondary = GeneratePath(index, secondaryPath);
            if (!resultSecondary)
                return false;

            nodes[nodes.Count - 1].type = Noeud.TYPE_DE_NOEUD.KEY;
        }
        return true;
    }

    #region Path

    private static bool GeneratePath(int rootNode, int pathLength)
    {
        Vector2Int pos = nodes[rootNode].position;
        Vector2Int direction = Vector2Int.zero;
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);
        int startIndex = nodes.Count;
        for (int i = startIndex; i < pathLength + startIndex; i++)
        {
            //Direction
            direction = NewDirection(pos, direction);

            // if no direction return no graph 
            if (direction == Vector2.zero)
            {
                return false;
            }
            else
                pos += direction;

            // Create new node
            Noeud node = new Noeud(pos, Noeud.TYPE_DE_NOEUD.INTERMEDIATE);
            grid[pos.x, pos.y] = i;

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
    private static Vector2Int NewDirection(Vector2Int position, Vector2Int previousDirection)
    {
        Vector2 direction = Vector2.zero;
        bool[] neighboursIsEmpty = new bool[4];
        for (int i = 0; i < neighboursIsEmpty.Length; i++)
        {
            neighboursIsEmpty[i] = IsCoordEmpty(position + neighBours[i]);
        } 
        if (!neighboursIsEmpty[0] &&
            !neighboursIsEmpty[1] &&
            !neighboursIsEmpty[2] &&
            !neighboursIsEmpty[3])
            return Vector2Int.zero;
        else
            return GenerateNewDirection(neighboursIsEmpty);
        /*
        //bool top, right, bottom, left;
        //  RIGHT
        Vector2Int rightNeighBour = position + new Vector2Int(1, 0);
        if (!IsCoordEmpty(rightNeighBour) ||
            previousDirection.x == -1)
            right = false;
        else
            right = true;

        //  LEFT
        Vector2Int leftNeighBour = position + new Vector2Int(-1, 0);
        if (!IsCoordEmpty(leftNeighBour) ||
            previousDirection.x == 1)
            left = false;
        else
            left = true;

        //  TOP
        Vector2Int topNeighBour = position + new Vector2Int(0, 1);
        if (!IsCoordEmpty(topNeighBour) ||
            previousDirection.y == -1)
            top = false;
        else
            top = true;

        //  BOTTOM
        Vector2Int bottomNeighBour = position + new Vector2Int(0, -1);
        if (!IsCoordEmpty(bottomNeighBour) ||
            previousDirection.y == 1)
            bottom = false;
        else
            bottom = true;
        // if no available direction return no direction
        if (!top && !right && !bottom && !left)
            return Vector2Int.zero;
        else
            return GenerateNewDirection(top, right, bottom, left);
        */
    }

    private static Vector2Int GenerateNewDirection(bool[] neighbours)
    {
        Vector2Int direction = Vector2Int.zero;
        if (neighbours[0] && neighbours[1])
            direction.x = Random.Range(0, 2) == 1 ? 1 : -1;
        else if (neighbours[0])
            direction.x = 1;
        else if (neighbours[1])
            direction.x = -1;
        else
            direction.x = 0;

        if (neighbours[2] && neighbours[3])
            direction.y = Random.Range(0, 2) == 1 ? 1 : -1;
        else if (neighbours[2])
            direction.y = 1;
        else if (neighbours[3])
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

    private static int GetNodeIndexFromPos(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= gridWidth)
            return -1;
        if (pos.y < 0 || pos.y >= gridHeight)
            return -1;
        return grid[pos.x, pos.y];
    }

    private static bool IsCoordEmpty(Vector2Int pos)
    {
        return !(GetNodeIndexFromPos(pos) != 0 || pos == new Vector2Int(gridWidth/2, gridHeight/2));
    }
}
