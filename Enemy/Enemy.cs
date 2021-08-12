using _Scripts.Collectables;
using UnityEngine;

namespace _Scripts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int _points = 1000;
        
        public int Points => _points;
    }
}