using System;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.RecyclingStation
{
    public class RecyclingStation : MonoBehaviour
    {
        public int maxCapacity = 20;
        public int freeCapacity = 20;
        public bool IsFull => freeCapacity < 1;

        public Action OnIsFull;
        public Action<int> OnFreeCapacityChanged;

        public void PutOneGarbage(IGarbageContainer from)
        {
            if (IsFull)
            {
                OnIsFull?.Invoke();
                return;
            }

            if (!from.HasGarbage) return;

            
            from.GetOutOneGarbage();
            freeCapacity--;
            OnFreeCapacityChanged?.Invoke(freeCapacity);
        }
        
    }
}