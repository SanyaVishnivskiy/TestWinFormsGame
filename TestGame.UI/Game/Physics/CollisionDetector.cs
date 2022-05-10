namespace TestGame.UI.Game.Physics;

public class CollisionDetector
{
    private IGameMap _map;

    public CollisionDetector(IGameMap map)
    {
        _map = map;
    }

    public List<Collision> CalculateCollisionsWithMap(ICollidable entity, Position newPosition)
    {
        var collisions = new List<Collision>();

        var collidable = new OverridenCollidableAdapter(entity, newPosition);
        var direction = DirectionCalculator.CalculateDirection(collidable.OldPosition, collidable.NewPosition);

        GroundTile[,] tiles = CalculateEntityPositionOnTiles(collidable);
        tiles.Loop((row, column) =>
        {
            GroundTile tile = tiles[row, column];
            if (tile != null && tile is ICollidable collidableTile)
            {
                (var collides, var collistionDirections) = CheckCollides(collidable, collidableTile, direction);
                if (collides)
                {
                    collisions.Add(new Collision(collidable, collidableTile, collistionDirections));
                }
            }
        });

        return collisions;
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

    private bool CheckHitboxesCollides(RectangleF hitbox, RectangleF anotherHitbox)
    {
        var rectangle = Intersects(hitbox, anotherHitbox);
        return !rectangle.IsEmpty;
    }

    private (bool, Direction) CheckCollides(
        OverridenCollidableAdapter collidable,
        ICollidable anotherCollidable,
        Direction direction)
    {
        if (!CheckHitboxesCollides(collidable.NewHitbox, anotherCollidable.Hitbox))
        {
            return (false, new Direction());
        }

        var resultDirection = new Direction();
        if (CheckCollisionToDirection(collidable, anotherCollidable, direction.Horizontal))
        {
            resultDirection.Add(direction.Horizontal);
        }

        if (CheckCollisionToDirection(collidable, anotherCollidable, direction.Vertical))
        {
            resultDirection.Add(direction.Vertical);
        }

        if (resultDirection.Directions.Count == 2)
        {
            // above we checked that we cannot move in diagonal direction
            // so if we have still both, give priority to horizontal movement
            resultDirection.Remove(resultDirection.Horizontal);
        }

        return (true, resultDirection);
    }

    public bool CheckCollisionToDirection(
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

    private RectangleF Intersects(RectangleF hitbox1, RectangleF hitbox2)
    {
        hitbox1.Intersect(hitbox2);
        return hitbox1;
    }

    private RectangleF MoveHitboxToOneDirection(RectangleF oldHitbox, RectangleF newHitbox, MoveDirection direction)
    {
        var point = new PointF();
        if (direction == MoveDirection.Left || direction == MoveDirection.Right)
        {
            point.X = newHitbox.X;
            point.Y = oldHitbox.Y;
        }
        else if (direction == MoveDirection.Up || direction == MoveDirection.Down)
        {
            point.X = oldHitbox.X;
            point.Y = newHitbox.Y;
        }

        return new RectangleF(point, oldHitbox.Size);
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
