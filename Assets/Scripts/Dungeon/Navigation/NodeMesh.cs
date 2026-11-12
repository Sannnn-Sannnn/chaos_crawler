using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeMesh : MonoBehaviour
{
  [Header("Node data")] [SerializeField] private GameObject nodePrefab;
  private Dictionary<Vector2, Node> _nodes;

  [Header("Room data")] [SerializeField] private int roomSizeX;
  [SerializeField] private int roomSizeY;

  private float _coordModx;
  private float _coordModY;

  [Header("Obstacles data")] [SerializeField]
  private LayerMask[] obstacleLayers;

  [SerializeField] private float overlapCheckRadius;

  void Awake()
  {
    _nodes = new Dictionary<Vector2, Node>();
    
    _coordModx = roomSizeX % 2 == 0 ? 0.5f : 0f;
    _coordModY = roomSizeY % 2 == 0 ? 0.5f : 0f;

    GenerateMesh();
  }

  // MESH
  
  void GenerateMesh()
  {
    GenerateNodes();
    ConnectNodes();
  }

  void RemoveMesh()
  {
    foreach (var node in _nodes.Values) Destroy(node.gameObject);
    _nodes.Clear();
  }

  void GenerateNodes()
  {
    for (int i = -roomSizeX / 2; i < roomSizeX / 2; i++)
    {
      for (int j = -roomSizeY / 2; j < roomSizeY / 2; j++)
      {
        var spawnPosition = new Vector2(i + _coordModx, j + _coordModY);
        var isValidNodePosition = true;

        foreach (LayerMask obstacleLayer in obstacleLayers)
        {
          if (Physics2D.OverlapCircle(spawnPosition, overlapCheckRadius, obstacleLayer) != null)
            isValidNodePosition = false;
        }

        if (isValidNodePosition)
        {
          var nodeInstance = Instantiate(nodePrefab, transform);
          nodeInstance.transform.position = spawnPosition;
          _nodes.Add(spawnPosition, nodeInstance.GetComponent<Node>());
        }
      }
    }
  }

  void ConnectNodes()
  {
    foreach (Node currentNode in _nodes.Values)
    {
      foreach (Node node in _nodes.Values)
      {
        if (currentNode == node) continue;

        if (Vector2.Distance(currentNode.transform.position, node.transform.position) <= Math.Sqrt(2))
        {
          currentNode.connectedNodes.Add(node);
        }
      }
    }
  }
  
  // UTILITIES
  
  public List<Node> FindPath(Node start, Node end)
  {
    List<Node> openSet = new List<Node>();

    foreach (Node node in FindObjectsByType<Node>())
    {
      node.gScore = float.MaxValue;
    }

    start.gScore = 0;
    start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
    openSet.Add(start);

    while (openSet.Count > 0)
    {
      int lowestF = 0;

      for (int i = 1; i < openSet.Count; i++)
      {
        if (openSet[i].FScore() < openSet[lowestF].FScore())
        {
          lowestF = i;
        }
      }

      Node currentNode = openSet[lowestF];
      openSet.Remove(currentNode);

      if (currentNode == end)
      {
        List<Node> path = new List<Node>();

        path.Insert(0, end);

        while (currentNode != start)
        {
          currentNode = currentNode.cameFrom;
          path.Add(currentNode);
        }

        path.Reverse();
        return path;
      }

      foreach (Node node in currentNode.connectedNodes)
      {
        float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, node.transform.position);

        if (heldGScore < node.gScore)
        {
          node.cameFrom = currentNode;
          node.gScore = heldGScore;
          node.hScore = Vector2.Distance(node.transform.position, end.transform.position);

          if (!openSet.Contains(node))
          {
            openSet.Add(node);
          }
        }
      }
    }

    return null;
  }

  public Node FindClosestNode(Vector3 position)
  {
    Node foundNode = null;
    float minDistance = float.MaxValue;
    foreach (Node node in _nodes.Values)
    {
      float currentDistance = Vector2.Distance(position, node.transform.position);
      if (currentDistance <= minDistance)
      {
        minDistance = currentDistance;
        foundNode = node;
      }
    }
    return foundNode;
  }
}