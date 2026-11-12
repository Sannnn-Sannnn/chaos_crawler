using System.Collections.Generic;
using Player;
using UnityEngine;

public class AStarEnemyTest : MonoBehaviour
{
  [SerializeField] private float speed;

  [SerializeField] private NodeMesh mesh;

  public Node currentNode;
  public List<Node> path = new();
  
  public PlayerHealth playerHealth;
  public Vector3 target;

  void Start()
  {
    currentNode = mesh.FindClosestNode(transform.position);
    playerHealth = GameObject.FindGameObjectWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
    target = playerHealth.transform.position;
  }
  
  void Update()
  {
    ChasePlayer();
    FindPath();
  }

  void ChasePlayer()
  {
    
  }

  void FindPath()
  {
    path = mesh.FindPath(currentNode, mesh.FindClosestNode(playerHealth.transform.position));
    if (path.Count > 0)
    {
      int x = 0;
      transform.position = Vector3.MoveTowards(transform.position, path[x].transform.position, speed * Time.deltaTime);
      if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
      {
        currentNode =  path[x];
        path.RemoveAt(x);
      }
    }
  }
}