import random

"""
Best results:
MAX_ROOMS = 12

MIN_DISTANCE = 4
MAX_DISTANCE = 6
MIN_X = 0
MAX_ITER = 100 --> Not really important as the MAX_ROOMS is 12

INITIAL_P = 0.3
MIN_P = 0 --> Same as with MAX_ITER
P_DECREMENT = 0.0
"""

# Define constants
MAX_ROOMS = 12

MIN_DISTANCE = 4
MAX_DISTANCE = 6
MIN_X = 0
MAX_ITER = 100

INITIAL_P = 0.3
MIN_P = 0
P_DECREMENT = 0.0

MAX_DISTANCE = MAX_DISTANCE if MAX_DISTANCE < MAX_ROOMS else MAX_ROOMS


# Main algorithm
def generate_dungeon():
    # Define random distance
    i = random.randint(MIN_DISTANCE, MAX_DISTANCE)

    # Get ending point f
    x = random.randint(MIN_X, i - MIN_X)
    y = i - x
    f = (x * random.choice([-1, 1]), y * random.choice([-1, 1]))

    # Navigate from (0, 0) to f
    dungeon = []
    corridors = []
    next_iter = []
    step = (0, 0)

    while step != f:
        can_move_x = step[0] != f[0]
        can_move_y = step[1] != f[1]

        if can_move_x and can_move_y:
            target_value = random.choice(['x', 'y'])
        elif can_move_x:
            target_value = 'x'
        else:
            target_value = 'y'

        if target_value == 'x':
            next_step = (step[0] + (1 if f[0] > step[0] else -1), step[1])
        else:
            next_step = (step[0], step[1] + (1 if f[1] > step[1] else -1))
        next_iter.append(step)
        corridors.append((step, next_step))
        step = next_step
    next_iter.append(f)

    # Populate
    room_count = len(next_iter)
    p = INITIAL_P
    directions = [(1, 0), (0, -1), (-1, 0), (0, 1)]
    current_iter = 0

    while len(next_iter) > 0 and p >= MIN_P and current_iter < MAX_ITER:
        new_rooms = []
        for room in next_iter:
            for direction in directions:
                generate_new_room = random.random() < p
                if generate_new_room:
                    new_room = tuple(x + y for x, y in zip(room, direction))
                    corridors.append((room, new_room))
                    if new_room not in new_rooms and new_room not in next_iter and new_room not in dungeon:
                        new_rooms.append(new_room)
                        room_count += 1
                        if room_count == MAX_ROOMS:
                            return dungeon + next_iter + new_rooms, corridors, f
        dungeon = dungeon + next_iter
        next_iter = new_rooms
        p -= P_DECREMENT
        current_iter += 1
    return dungeon, corridors, f


def print_points(points, end):
    min_x = min(x for x, y in points)
    max_x = max(x for x, y in points)
    min_y = min(y for x, y in points)
    max_y = max(y for x, y in points)

    points_set = set(points)

    for y in range(max_y, min_y - 1, -1):
        row = ""
        for x in range(min_x, max_x + 1):
            row += " A " if (x, y) == (0, 0) else " Z " if (x, y) == end else " □ " if (x, y) in points_set else " . "
        print(row)


def print_points_corridors(points, corridors, end):
    min_x = min(x for x, y in points)
    max_x = max(x for x, y in points)
    min_y = min(y for x, y in points)
    max_y = max(y for x, y in points)

    points_set = set(points)
    corridors_set = {
        frozenset((a, b))
        for a, b in corridors
    }

    # Cada sala ocupa 3 caracteres de ancho/alto
    for y in range(max_y, min_y - 1, -1):
        # Línea de salas
        row = ""
        for x in range(min_x, max_x + 1):
            if (x, y) == (0, 0):
                room = " A "
            elif (x, y) == end:
                room = " Z "
            elif (x, y) in points_set:
                room = " □ "
            else:
                room = "   "

            row += room

            # Conexión hacia la derecha
            if (x, y) in points_set and (x + 1, y) in points_set:
                if frozenset(((x, y), (x + 1, y))) in corridors_set:
                    row += "───"
                else:
                    row += "   "
            else:
                row += "   "

        print(row)

        # Línea de conexiones verticales
        if y > min_y:
            row = ""
            for x in range(min_x, max_x + 1):
                if (x, y) in points_set and (x, y - 1) in points_set:
                    if frozenset(((x, y), (x, y - 1))) in corridors_set:
                        row += " │ "
                    else:
                        row += "   "
                else:
                    row += "   "

                # Espacio correspondiente entre columnas
                if x < max_x:
                    row += "   "

            print(row)


test = False

if test:
    dungeon = [(0, 0), (1, 0), (2, 0), (1, 1), (1, -1)]
    corridors = [((0, 0), (1, 0)), ((1, 0), (2, 0)), ((1, 0), (1, 1)), ((1, 0), (1, -1))]
    end = (2, 0)
else:
    dungeon, corridors, end = generate_dungeon()

# print(sorted(dungeon))
print(f"Rooms: {len(dungeon) - 2}")
print_points_corridors(dungeon, corridors, end)