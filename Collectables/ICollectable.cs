using UnityEngine;

namespace _Scripts.Collectables
{
    public interface ICollectable
    {
        GameObject GameObject { get; }
        CollectableTypeEnum Type { get; }
    }

    public enum CollectableTypeEnum
    {
        DotOrGarbage, 
        PowerUp,
        Garbage
    }
}