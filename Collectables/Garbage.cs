using System;
using UnityEngine;

namespace _Scripts.Collectables
{
    /**
     * Garbage increases Suction Sum WasteBin
     * and should add extra points
     */
    public class Garbage : MonoBehaviour, ICollectable
    {
        public int pointsAmount=10;

        public int GetPoints()
        {
            return pointsAmount;
        }

        public GameObject GameObject => gameObject;
        public CollectableTypeEnum Type => CollectableTypeEnum.Garbage;
    }
}