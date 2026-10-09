using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration
{
    public static (List<Vector2Int>, List<(Vector2Int, Vector2Int)>, Vector2Int) GenerateDungeon(
        int maxRooms,
        int maxIterations,
        int minDistance,
        int maxDistance,
        int minX,
        float initialProbability,
        float minProbability,
        float probabilityDecrement,
        bool branchFromStart,
        bool branchFromEnd
    )
    {
        // 1. Validar y calcular fin
        int safeMaxDistance = Mathf.Min(maxDistance, maxRooms - 1);
        int distance = Random.Range(minDistance, safeMaxDistance + 1);

        // Asegurar que minX no exceda la mitad de la distancia
        int clampedMinX = Mathf.Clamp(minX, 0, distance / 2);
        int maxX = distance - clampedMinX;
        int fx = (clampedMinX >= maxX) ? clampedMinX : Random.Range(clampedMinX, maxX + 1);
        int fy = distance - fx;

        int[] signs = { -1, 1 };
        Vector2Int end = new Vector2Int(
            fx * signs[Random.Range(0, signs.Length)],
            fy * signs[Random.Range(0, signs.Length)]
        );

        // 2. Trazar camino inicial desde (0, 0) hasta end
        HashSet<Vector2Int> occupiedRooms = new HashSet<Vector2Int>();
        List<(Vector2Int, Vector2Int)> corridors = new List<(Vector2Int, Vector2Int)>();
        List<Vector2Int> frontier = new List<Vector2Int>();

        Vector2Int current = Vector2Int.zero;
        occupiedRooms.Add(current);
        frontier.Add(current);

        while (current != end)
        {
            bool canMoveX = current.x != end.x;
            bool canMoveY = current.y != end.y;
            Vector2Int nextStep;

            bool moveHorizontally = (canMoveX && canMoveY) ? (Random.value > 0.5f) : canMoveX;

            if (moveHorizontally)
            {
                nextStep = new Vector2Int(current.x + (current.x > end.x ? -1 : 1), current.y);
            }
            else
            {
                nextStep = new Vector2Int(current.x, current.y + (current.y > end.y ? -1 : 1));
            }

            corridors.Add((current, nextStep));
            occupiedRooms.Add(nextStep);
            frontier.Add(nextStep);
            current = nextStep;
        }

        // 3. Ramificaciones
        float p = initialProbability;
        int currentIteration = 0;
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

        while (frontier.Count > 0 && p >= minProbability && currentIteration < maxIterations)
        {
            List<Vector2Int> nextFrontier = new List<Vector2Int>();

            foreach (Vector2Int room in frontier)
            {
                if (!branchFromStart && room == Vector2Int.zero) continue;
                if (!branchFromEnd && room == end) continue;

                foreach (Vector2Int dir in directions)
                {
                    if (Random.value < p)
                    {
                        Vector2Int newRoom = room + dir;

                        // Solo se crea si la celda está vacía
                        if (!occupiedRooms.Contains(newRoom))
                        {
                            occupiedRooms.Add(newRoom);
                            nextFrontier.Add(newRoom);
                            corridors.Add((room, newRoom));

                            if (occupiedRooms.Count >= maxRooms)
                            {
                                return (new List<Vector2Int>(occupiedRooms), corridors, end);
                            }
                        }
                    }
                }
            }

            frontier = nextFrontier;
            p -= probabilityDecrement;
            currentIteration++;
        }

        return (new List<Vector2Int>(occupiedRooms), corridors, end);
    }
}