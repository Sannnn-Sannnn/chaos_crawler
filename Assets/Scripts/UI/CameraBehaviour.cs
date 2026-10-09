using Player;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField] private PlayerMovement player;

    private float _z;
    
    void Awake()
    {
        _z = transform.position.z;
    }
    
    void FixedUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, _z);
    }
}
