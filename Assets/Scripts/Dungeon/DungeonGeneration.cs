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
        float probabilityDecrement
        )
    {
        // 1. Create ending room
        int safeMaxDistance = maxDistance > maxRooms ? maxRooms : maxDistance;
        int distance = Random.Range(minDistance, safeMaxDistance + 1);
        int fx = Random.Range(minX, distance - minX);
        int fy = distance - fx;

        int[] negat = { -1, 1 };

        Vector2Int end = new Vector2Int(fx * negat[Random.Range(0, negat.Length)], fy * negat[Random.Range(0, negat.Length)]);

        // 2. Navigate from (0, 0) to end
        List<Vector2Int> dungeon = new List<Vector2Int>();
        List<(Vector2Int, Vector2Int)> corridors = new List<(Vector2Int, Vector2Int)>();
        List<Vector2Int> nextIteration = new List<Vector2Int>();
        Vector2Int step = new Vector2Int(0, 0);

        while (!step.Equals(end))
        {
            bool canMoveX = step.x != end.x;
            bool canMoveY = step.y != end.y;
            char[] values = { 'x', 'y' };
            char targetValue;
            Vector2Int nextStep;

            if (canMoveX && canMoveY)
            {
                targetValue = values[Random.Range(0, values.Length)];
            }
            else if (canMoveX)
            {
                targetValue = 'x';
            }
            else
            {
                targetValue = 'y';
            }

            if (targetValue == 'x')
            {
                nextStep = step.x > end.x ? new Vector2Int(step.x - 1, step.y) : new Vector2Int(step.x + 1, step.y);
            }
            else
            {
                nextStep = step.y > end.y ? new Vector2Int(step.x, step.y - 1) : new Vector2Int(step.x, step.y + 1);
            }
            nextIteration.Add(step);
            corridors.Add((step, nextStep));
            step = nextStep;
        }

        nextIteration.Add(end);
        
        // 3. Populate dungeon
        int roomCount = nextIteration.Count;
        float p = initialProbability;
        Vector2Int[] directions = { new (1, 0), new (0, 1), new (1, 0), new (0, -1) };
        int currentIteration = 0;

        while (nextIteration.Count > 0 && p >= minProbability && currentIteration < maxIterations)
        {
            List<Vector2Int> newRooms = new List<Vector2Int>();
            foreach (Vector2Int room in nextIteration)
            {
                foreach (Vector2Int direction in directions)
                {
                    if (Random.Range(0f, 1f) < p)
                    {
                        Vector2Int newRoom = new Vector2Int(room.x + direction.x, room.y + direction.y);
                        corridors.Add((room, newRoom));
                        if (!newRooms.Contains(newRoom) && !nextIteration.Contains(newRoom) && !dungeon.Contains(newRoom))
                        {
                            newRooms.Add(newRoom);
                            roomCount += 1;
                            if (roomCount == maxRooms)
                            {
                                dungeon.AddRange(nextIteration);
                                dungeon.AddRange(newRooms);
                                return (dungeon, corridors, end);
                            }
                        }
                    }
                }
            }

            dungeon.AddRange(nextIteration);
            nextIteration = newRooms;
            p -= probabilityDecrement;
            currentIteration++;
        }

        return (dungeon, corridors, end);
    }
}