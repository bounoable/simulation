using UnityEngine;

namespace Simulation.Core
{
    abstract class GameStage: ScriptableObject, IGameLoop
    {
        public enum Stage
        {
            AI,
            Maze,
        }

        public GameManager Game { get; set; }
        public Stage StageName => stageName;
        
        [SerializeField]
        Stage stageName;

        virtual public void Start()
        {}

        virtual public void Update()
        {}
        
        virtual public void FixedUpdate()
        {}
    }
}