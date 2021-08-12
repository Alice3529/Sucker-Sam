using System;
using _Scripts.Collectables;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerWasteBin : MonoBehaviour, IGarbageContainer
    {
        public int maxCapacity = 5;
        public int freeCapacity = 5;
        public bool IsFull => freeCapacity < 1;
        public bool HasGarbage => freeCapacity != maxCapacity;
        public int garbageCount => maxCapacity - freeCapacity;

        public Action OnWasteBinIsFull;
        public Action<int> OnFreeCapacityChanged;

        private void Start()
        {
            freeCapacity = maxCapacity;
        }

        public void ConsumeWaste(ICollectable collectable)
        {
            if (IsFull)
            {
                OnWasteBinIsFull?.Invoke();
                return;
            };

            freeCapacity--;
            OnFreeCapacityChanged?.Invoke(freeCapacity);
        }

        public void GetOutOneGarbage()
        {
            var garbageToRemove = Math.Min(garbageCount, 1);
            freeCapacity += garbageToRemove;
            OnFreeCapacityChanged?.Invoke(freeCapacity);
        }
    }
}