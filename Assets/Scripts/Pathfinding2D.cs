using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding2D : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid2D grid;

    void Awake()
    {
        //Instantiate grid
        requestManager = GetComponent<PathRequestManager>(); 
        grid = GetComponent<Grid2D>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //get player and target position in grid coords
        //Debug.Log(grid.NodeFromWorldPoint(startPos));
        //Debug.Log("start: " + startPos);
        //Debug.Log("target: " + targetPos);

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node2D seekerNode = grid.NodeFromWorldPoint(startPos);
        //Debug.Log(grid.NodeFromWorldPoint(targetPos));
        Node2D targetNode = grid.NodeFromWorldPoint(targetPos);

        if (!seekerNode.obstacle && !targetNode.obstacle)
        {
            Heap<Node2D> openSet = new Heap<Node2D>(grid.MaxSize);
            HashSet<Node2D> closedSet = new HashSet<Node2D>();
            openSet.Add(seekerNode);

            //calculates path for pathfinding
            while (openSet.Count > 0)
            {

                //iterates through openSet and finds lowest FCost
                Node2D node = openSet.RemoveFirst();
                closedSet.Add(node);

                //If target found, retrace path
                if (node == targetNode)
                {
                    pathSuccess = true;
                    RetracePath(seekerNode, targetNode);
                    break;
                }

                //adds neighbor nodes to openSet
                foreach (Node2D neighbour in grid.GetNeighbors(node))
                {
                    if (neighbour.obstacle || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.Updateitem(neighbour);
                    }
                }
            }
        }

        yield return null;

        if(pathSuccess)
        {
            wayPoints = RetracePath(seekerNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    //reverses calculated path so first node is closest to seeker
    Vector3[] RetracePath(Node2D startNode, Node2D endNode)
    {
        List<Node2D> path = new List<Node2D>();
        Node2D currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] wayPoints = SimplifyPath(path);
        Array.Reverse(wayPoints);

        return wayPoints;
    }

    Vector3[] SimplifyPath(List<Node2D> path)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if(directionNew != directionOld)
            {
                wayPoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }

        return wayPoints.ToArray();
    }

    //gets distance between 2 nodes for calculating cost
    int GetDistance(Node2D nodeA, Node2D nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}