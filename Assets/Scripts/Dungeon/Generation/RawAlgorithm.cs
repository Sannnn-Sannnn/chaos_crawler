using System;
using System.Collections.Generic;

/*
public struct Vector2Int
{
    public int x;
    public int y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        return obj is Vector2Int other &&
               x == other.x &&
               y == other.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !(a == b);
    }
}

public class DungeonGeneration
{
    private static Random random = new Random();

    public static (
        List<Vector2Int>,
        List<(Vector2Int, Vector2Int)>,
        Vector2Int
    ) GenerateDungeon(
        int maxRooms,
        int maxIterations,
        int minDistance,
        int maxDistance,
        int minX,
        double initialProbability,
        double minProbability,
        double probabilityDecrement
    )
    {
        // 1. Create ending room

        int safeMaxDistance = Math.Min(maxDistance, maxRooms);

        int distance = random.Next(
            minDistance,
            safeMaxDistance + 1
        );

        int fx = random.Next(
            minX,
            distance - minX
        );

        int fy = distance - fx;

        int[] negat = { -1, 1 };

        Vector2Int end = new Vector2Int(
            fx * negat[random.Next(negat.Length)],
            fy * negat[random.Next(negat.Length)]
        );


        // 2. Navigate from (0, 0) to end

        List<Vector2Int> dungeon =
            new List<Vector2Int>();

        List<(Vector2Int, Vector2Int)> corridors =
            new List<(Vector2Int, Vector2Int)>();

        List<Vector2Int> nextIteration =
            new List<Vector2Int>();

        Vector2Int step =
            new Vector2Int(0, 0);


        while (!step.Equals(end))
        {
            bool canMoveX = step.x != end.x;
            bool canMoveY = step.y != end.y;

            char targetValue;
            Vector2Int nextStep;

            if (canMoveX && canMoveY)
            {
                targetValue =
                    random.Next(2) == 0 ? 'x' : 'y';
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
                nextStep =
                    step.x > end.x
                    ? new Vector2Int(step.x - 1, step.y)
                    : new Vector2Int(step.x + 1, step.y);
            }
            else
            {
                nextStep =
                    step.y > end.y
                    ? new Vector2Int(step.x, step.y - 1)
                    : new Vector2Int(step.x, step.y + 1);
            }


            nextIteration.Add(step);

            corridors.Add(
                (step, nextStep)
            );

            step = nextStep;
        }

        nextIteration.Add(end);


        // 3. Populate dungeon

        int roomCount = nextIteration.Count;

        double p = initialProbability;

        Vector2Int[] directions =
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
        };

        int currentIteration = 0;


        while (
            nextIteration.Count > 0 &&
            p >= minProbability &&
            currentIteration < maxIterations
        )
        {
            List<Vector2Int> newRooms =
                new List<Vector2Int>();


            foreach (Vector2Int room in nextIteration)
            {
                foreach (Vector2Int direction in directions)
                {
                    if (random.NextDouble() < p)
                    {
                        Vector2Int newRoom =
                            new Vector2Int(
                                room.x + direction.x,
                                room.y + direction.y
                            );


                        corridors.Add(
                            (room, newRoom)
                        );


                        if (
                            !newRooms.Contains(newRoom) ||
                            !nextIteration.Contains(newRoom) ||
                            !dungeon.Contains(newRoom)
                        )
                        {
                            newRooms.Add(newRoom);

                            roomCount++;


                            if (roomCount == maxRooms)
                            {
                                dungeon.AddRange(
                                    nextIteration
                                );

                                dungeon.AddRange(
                                    newRooms
                                );

                                return (
                                    dungeon,
                                    corridors,
                                    end
                                );
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


        return (
            dungeon,
            corridors,
            end
        );
    }


    public static void PrintPointsCorridors(
        List<Vector2Int> points,
        List<(Vector2Int, Vector2Int)> corridors,
        Vector2Int end
    )
    {
        if (points.Count == 0)
        {
            Console.WriteLine("Dungeon vacío.");
            return;
        }


        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;


        foreach (Vector2Int point in points)
        {
            minX = Math.Min(minX, point.x);
            maxX = Math.Max(maxX, point.x);

            minY = Math.Min(minY, point.y);
            maxY = Math.Max(maxY, point.y);
        }


        HashSet<Vector2Int> pointsSet =
            new HashSet<Vector2Int>(points);


        HashSet<(Vector2Int, Vector2Int)> corridorsSet =
            new HashSet<(Vector2Int, Vector2Int)>();


        foreach (var corridor in corridors)
        {
            Vector2Int a = corridor.Item1;
            Vector2Int b = corridor.Item2;

            corridorsSet.Add((a, b));
            corridorsSet.Add((b, a));
        }


        // Dibujar de arriba hacia abajo

        for (int y = maxY; y >= minY; y--)
        {
            string row = "";


            // Salas

            for (int x = minX; x <= maxX; x++)
            {
                Vector2Int current =
                    new Vector2Int(x, y);


                if (current == new Vector2Int(0, 0))
                {
                    row += " A ";
                }
                else if (current == end)
                {
                    row += " F ";
                }
                else if (pointsSet.Contains(current))
                {
                    row += " # ";
                }
                else
                {
                    row += "   ";
                }


                // Corredor horizontal

                Vector2Int right =
                    new Vector2Int(x + 1, y);


                if (
                    pointsSet.Contains(current) &&
                    pointsSet.Contains(right) &&
                    corridorsSet.Contains(
                        (current, right)
                    )
                )
                {
                    row += "---";
                }
                else
                {
                    row += "   ";
                }
            }


            Console.WriteLine(row);


            // Corredores verticales

            if (y > minY)
            {
                row = "";


                for (int x = minX; x <= maxX; x++)
                {
                    Vector2Int current =
                        new Vector2Int(x, y);

                    Vector2Int down =
                        new Vector2Int(x, y - 1);


                    if (
                        pointsSet.Contains(current) &&
                        pointsSet.Contains(down) &&
                        corridorsSet.Contains(
                            (current, down)
                        )
                    )
                    {
                        row += " | ";
                    }
                    else
                    {
                        row += "   ";
                    }


                    if (x < maxX)
                    {
                        row += "   ";
                    }
                }


                Console.WriteLine(row);
            }
        }
    }


    public static void Main()
    {
        var result = GenerateDungeon(
            maxRooms: 20,
            maxIterations: 1000,
            minDistance: 5,
            maxDistance: 10,
            minX: 1,
            initialProbability: 0.75,
            minProbability: 0.05,
            probabilityDecrement: 0.05
        );


        List<Vector2Int> points = result.Item1;
        List<(Vector2Int, Vector2Int)> corridors = result.Item2;
        Vector2Int end = result.Item3;


        PrintPointsCorridors(
            points,
            corridors,
            end
        );
    }
}
*/