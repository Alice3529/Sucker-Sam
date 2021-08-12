using UnityEngine;

namespace _Scripts.Collectables
{
    public class Dot : MonoBehaviour, ICollectable
    {
        public int pointsAmount=10;

        public int GetPoints()
        {
            return pointsAmount;
        }

        public GameObject GameObject => gameObject;
        public CollectableTypeEnum Type => CollectableTypeEnum.DotOrGarbage;
    }
}
