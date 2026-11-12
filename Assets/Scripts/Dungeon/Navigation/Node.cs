using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connectedNodes;

    public float gScore;
    public float hScore;

    public float FScore()
    {
        return gScore + hScore;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Node node in connectedNodes){
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
