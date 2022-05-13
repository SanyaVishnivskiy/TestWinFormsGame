namespace TestGame.UI.Game.Physics;

public class CollisionDetector
{
    private IGameMap _map;

    public CollisionDetector(IGameMap map)
    {
        _map = map;
    }

    public List<Collision> CalculateCollisions(ICollidable entity, Position newPosition)
    {
        var collidable = new OverridenCollidableAdapter(entity, newPosition);

        return CalculateCollisionsWithMap(collidable)
            .Concat(CalculateCollisionsWithMapObjects(collidable))
            .ToList();
    }

    public IEnumerable<Collision> CalculateCollisionsWithMap(OverridenCollidableAdapter entity)
    {
        GroundTile[,] tiles = CalculateEntityPositionOnTiles(entity);

        return CalculateCollisionsIfCollidable(entity, tiles.Traverse());
    }

    private GroundTile[,] CalculateEntityPositionOnTiles(OverridenCollidableAdapter collidable)
    {
        var topLeftIntersectedTile = TileToPositionConverter.ToTile(new Position(collidable.NewHitbox.X, collidable.NewHitbox.Y));
        var horizontalIntersectionInTiles = TileToPositionConverter.GetTilesCount(collidable.NewHitbox.X, collidable.NewHitbox.Width, Constants.TileWidth);
        var verticalIntersectionInTiles = TileToPositionConverter.GetTilesCount(collidable.NewHitbox.Y, collidable.NewHitbox.Height, Constants.TileHeight);
        GroundTile[,] result = new GroundTile[horizontalIntersectionInTiles, verticalIntersectionInTiles];
        for (int i = 0; i < horizontalIntersectionInTiles; i++)
        {
            for (int j = 0; j < verticalIntersectionInTiles; j++)
            {
                result[i, j] = _map.GetGroundTile(topLeftIntersectedTile.Y + j, topLeftIntersectedTile.X + i);
            }
        }

        return result;
    }

    private IEnumerable<Collision> CalculateCollisionsIfCollidable(
        OverridenCollidableAdapter entity,
        IEnumerable<object> objects)
    {
        var collidableObjects = objects.Where(x => x is not null).OfType<ICollidable>();

        return CalculateCollisions(entity, collidableObjects);
    }

    private IEnumerable<Collision> CalculateCollisions(
        OverridenCollidableAdapter entity,
        IEnumerable<ICollidable> otherEntities)
    {
        var collisions = new List<Collision>();

        var direction = DirectionCalculator.CalculateDirection(entity.OldPosition, entity.NewPosition);
        foreach (var otherEntity in otherEntities)
        {
            (var collides, var collisionDirections) = CheckCollides(entity, otherEntity, direction);
            if (collides)
            {
                var tileCollisions = collisionDirections.Select(x => new Collision(entity.Collidable, otherEntity, x));
                collisions.AddRange(tileCollisions);
            }
        }

        return collisions;
    }

    private (bool, List<Direction>) CheckCollides(
        OverridenCollidableAdapter collidable,
        ICollidable anotherCollidable,
        Direction direction)
    {
        if (!CheckHitboxesCollides(collidable.NewHitbox, anotherCollidable.Hitbox))
        {
            return (false, new List<Direction>());
        }

        var resultDirections = new List<Direction>() { direction };
        if (!direction.IsDiagonal)
        {
            return (true, resultDirections);
        }

        if (CheckCollisionOnDirection(collidable, anotherCollidable, direction.Horizontal))
        {
            resultDirections.Add(new Direction(direction.Horizontal));
        }

        if (CheckCollisionOnDirection(collidable, anotherCollidable, direction.Vertical))
        {
            resultDirections.Add(new Direction(direction.Vertical));
        }

        return (true, resultDirections);
    }

    private bool CheckHitboxesCollides(RectangleF hitbox, RectangleF anotherHitbox)
    {
        var rectangle = Intersects(hitbox, anotherHitbox);
        return !rectangle.IsEmpty;
    }

    private RectangleF Intersects(RectangleF hitbox1, RectangleF hitbox2)
    {
        hitbox1.Intersect(hitbox2);
        return hitbox1;
    }

    public bool CheckCollisionOnDirection(
        OverridenCollidableAdapter collidable,
        ICollidable anotherCollidable,
        MoveDirection direction)
    {
        if (direction == MoveDirection.None)
        {
            return false;
        }

        var movedHitbox = MoveHitboxToOneDirection(collidable.Hitbox, collidable.NewHitbox, direction);
        return CheckHitboxesCollides(movedHitbox, anotherCollidable.Hitbox);
    }

    private RectangleF MoveHitboxToOneDirection(RectangleF oldHitbox, RectangleF newHitbox, MoveDirection direction)
    {
        var point = new PointF();
        if (Direction.CheckHorizontal(direction))
        {
            point.X = newHitbox.X;
            point.Y = oldHitbox.Y;
        }
        else if (Direction.CheckVertical(direction))
        {
            point.X = oldHitbox.X;
            point.Y = newHitbox.Y;
        }

        return new RectangleF(point, oldHitbox.Size);
    }

    private IEnumerable<Collision> CalculateCollisionsWithMapObjects(OverridenCollidableAdapter entity)
    {
        var mapObjects = _map.GetMapObjects().Cast<object>();

        return CalculateCollisionsIfCollidable(entity, mapObjects);
    }
}

public class Collision
{
    public Collision(ICollidable entity, ICollidable anotherEntity, Direction direction)
    {
        Entity = entity;
        AnotherEntity = anotherEntity;
        Direction = direction;
    }

    public ICollidable Entity { get; }
    public ICollidable AnotherEntity { get; }
    public Direction Direction { get; }
}
