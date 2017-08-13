using Game.Models;
using UnityEngine;

namespace Game.Scripts
{
    public abstract class GameScript : MonoBehaviour
    {
        public GameModel Model { get; set; }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }
    }
}