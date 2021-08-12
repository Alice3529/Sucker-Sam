using System;
using UnityEngine;

namespace _Scripts.Player
{
    public interface IPlayerMovement
    {
        void MoveUp();
        void MoveDown();
        void MoveLeft();
        void MoveRight();
        Vector3 Position { get; }
        
        bool IsPoweredUp { get; }
        bool HasDied { get; }
        float TimeToDismissPowerUp { get; }

        event Action<bool, float> OnIsPoweredUpChanged;
        event Action<Enemy.Enemy> OnDiedBy;
        event Action<Enemy.Enemy> OnConsumedEnemy;
    }
}