using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Header("Referencia Principal")]
    [Tooltip("El jugador desde el cual se calculará la cercanía de los objetivos")]

    [Header("Configuración de Búsqueda")] [SerializeField]
    private string targetTag;
    [SerializeField] private float searchInterval = 0.2f;

    [Header("Movimiento")]
    [SerializeField] private bool smoothFollow = true;
    [SerializeField] private float smoothSpeed = 5f;
    
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;

    private Transform _currentTarget;
    private float _z;
    private float _nextSearchTime;
    private Transform _playerTransform;

    void Awake()
    {
        _z = transform.position.z;

        // Búsqueda de respaldo si no se asignó el jugador en el Inspector
        if (_playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                _playerTransform = playerObj.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (_playerTransform == null) return;

        // Actualiza el objetivo más cercano por intervalos para optimizar rendimiento
        if (Time.time >= _nextSearchTime)
        {
            _currentTarget = FindClosestTargetToPlayer();
            _nextSearchTime = Time.time + searchInterval;
        }

        // Si no encuentra ningún objetivo con el Tag, enfoca al propio jugador por defecto
        Transform targetToFollow = _currentTarget != null ? _currentTarget : _playerTransform;

        Vector3 targetPosition = new Vector3(targetToFollow.position.x + xOffset, targetToFollow.position.y + yOffset, _z);

        if (smoothFollow)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    private Transform FindClosestTargetToPlayer()
    {
        GameObject[] candidates = GameObject.FindGameObjectsWithTag(targetTag);

        if (candidates == null || candidates.Length == 0) return null;

        Transform closest = null;
        float minDistanceSqr = Mathf.Infinity;
        Vector2 playerPos = _playerTransform.position;

        foreach (GameObject candidate in candidates)
        {
            // Se ignora al jugador en caso de que comparta el mismo tag
            if (candidate.transform == _playerTransform) continue;

            // Medición de distancia respecto a la posición del jugador
            float distSqr = ((Vector2)candidate.transform.position - playerPos).sqrMagnitude;
            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                closest = candidate.transform;
            }
        }

        return closest;
    }
}