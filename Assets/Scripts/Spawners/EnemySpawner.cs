using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Prefabs")] [SerializeField]
        private GameObject[] enemyPrefabs;

        [Header("Room Setup")] [SerializeField]
        private int minEnemies;

        [SerializeField] private int maxEnemies;
        [SerializeField] private int roomHeight;
        [SerializeField] private int roomWidth;
        [SerializeField] private int minDoorDistance = 2;

        [Header("Collision Validation")] [SerializeField]
        private LayerMask[] obstacleLayers;

        [SerializeField] private float enemyCheckRadius = 0.4f; // Radio aproximado del hitbox del enemigo

        private int _enemyAmount;
        private int _enemyCount;
        private List<Vector3> _spawnPositions = new(); // Corregido a Vector3
        private Transform _spawnAnchor;
        private Vector3 _roomPositionZero;
        private DoorTrigger _trigger;

        private void Awake()
        {
            _trigger = GetComponent<DoorTrigger>();
            if (_trigger == null) Debug.LogError("DoorTrigger no encontrado en " + gameObject.name);

            if (minEnemies < 0) minEnemies = 0;
            if (maxEnemies < 0) maxEnemies = 0;
            if (minEnemies > maxEnemies) maxEnemies = minEnemies;

            _enemyAmount = Random.Range(minEnemies, maxEnemies + 1);

            _spawnAnchor = transform.parent != null ? transform.parent.Find("EnemySpawnAnchor") : null;

            if (_spawnAnchor != null)
            {
                _roomPositionZero = _spawnAnchor.position;
            }
            else
            {
                _roomPositionZero = transform.position;
                Debug.LogWarning("No se encontró 'EnemySpawnAnchor', usando la posición actual como centro.");
            }
        }

        public void SpawnEnemies()
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("No hay prefabs de enemigos asignados en el Spawner.");
                return;
            }

            for (int i = 0; i < _enemyAmount; i++)
            {
                Vector3 spawnPosition = GetRandomPosition();

                // Si no se encontró un lugar válido en la sala, saltamos esta iteración
                if (spawnPosition == Vector3.positiveInfinity) continue;

                int enemyVariant = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[enemyVariant];

                GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                if (enemyInstance.TryGetComponent<EnemyHealth>(out var enemyHealth))
                {
                    enemyHealth.SetSpawner(this);
                }

                _enemyCount++;
            }
        }

        private Vector3 GetRandomPosition()
        {
            const int maxAttempts = 100;
            int halfWidth = roomWidth / 2;
            int halfHeight = roomHeight / 2;

            for (int i = 0; i < maxAttempts; i++)
            {
                var isValidSpawn = true;

                int posX = Random.Range(-halfWidth + 1, halfWidth);
                int posY = Random.Range(-halfHeight + 1, halfHeight);

                Vector3 spawnPosition = _roomPositionZero + new Vector3(posX, posY, 0);

                // 1. Evita posiciones repetidas
                if (_spawnPositions.Contains(spawnPosition)) isValidSpawn = false;

                // 2. Evita las puertas
                if (IsCloseToDoor(spawnPosition)) isValidSpawn = false;

                // 3. NUEVO: Evita spawnear dentro de un Tilemap Collider o Pared
                foreach (LayerMask obstacleLayer in obstacleLayers)
                {
                    if (Physics2D.OverlapCircle(spawnPosition, enemyCheckRadius, obstacleLayer) != null)
                        isValidSpawn = false;
                }

                // Si pasa todas las validaciones, la posición es segura
                if (isValidSpawn)
                {
                    _spawnPositions.Add(spawnPosition);
                    return spawnPosition;
                }
            }

            Debug.LogWarning("No se encontró un punto de spawn válido sin colisión tras 100 intentos.");
            return Vector3.positiveInfinity; // Retorna un valor inválido controlado
        }

        private bool IsCloseToDoor(Vector3 pos)
        {
            Vector3 northDoor = _roomPositionZero + new Vector3(0, roomHeight / 2f, 0);
            Vector3 southDoor = _roomPositionZero + new Vector3(0, -roomHeight / 2f, 0);
            Vector3 eastDoor = _roomPositionZero + new Vector3(roomWidth / 2f, 0, 0);
            Vector3 westDoor = _roomPositionZero + new Vector3(-roomWidth / 2f, 0, 0);

            return Vector3.Distance(pos, northDoor) < minDoorDistance ||
                   Vector3.Distance(pos, southDoor) < minDoorDistance ||
                   Vector3.Distance(pos, eastDoor) < minDoorDistance ||
                   Vector3.Distance(pos, westDoor) < minDoorDistance;
        }

        public void Down()
        {
            _enemyCount--;
            if (_enemyCount <= 0)
            {
                if (_trigger != null) _trigger.Complete();
            }
        }
    }
}