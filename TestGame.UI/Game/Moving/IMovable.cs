﻿namespace TestGame.UI.Game.Moving
{
    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    internal interface IMovable
    {
        void Move();
    }

    internal interface IWalkable : IMovable
    {
        void StartMoving(MoveDirection direction);
        void FinishMoving(MoveDirection direction);
    }
}