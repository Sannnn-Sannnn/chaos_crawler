using Player;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField] private PlayerMovement player;
    
    void Update()
    {
        transform.position = player.transform.position;
    }
}
