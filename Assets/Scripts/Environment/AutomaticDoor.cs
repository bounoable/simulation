using UnityEngine;
using System.Linq;
using Simulation.AI;
using Simulation.Core;

namespace Simulation.Environment
{
    class AutomaticDoor: Door
    {
        [SerializeField]
        float sensorRange = 10f;

        public void OpenIfApproached(GameManager game)
        {
            if (IsClosed && CharacterApproaches(game))
                Open();
        }

        bool CharacterApproaches(GameManager game) => GetNearbyCharacters(game).Length > 0;

        ICharacter[] GetNearbyCharacters(GameManager game)
        {
            ICharacter[] characters = game.Characters;

            return characters.Where(character => Vector3.Distance(
                transform.position,
                character.Position
            ) <= sensorRange).ToArray();
        }
    }
}
