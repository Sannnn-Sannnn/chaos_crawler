using UnityEngine;
using UnityEngine.AI;

public class NavMeshEnemy : MonoBehaviour
{
  private Transform _target;
  private NavMeshAgent _agent;

  void Start()
  {
    _target = GameObject.FindGameObjectWithTag("Player").transform;
    _agent = GetComponent<NavMeshAgent>();
    _agent.updateRotation = false;
    _agent.updateUpAxis = false;
  }

  void Update()
  {
    _agent.SetDestination(_target.position);
  }
}