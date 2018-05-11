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

        public void AutoOpenClose(GameManager game, ref bool toggled)
        {
            bool approached = CharacterApproaches(game);

            if (IsClosed && approached) {
                Open();

                toggled = true;
            } else if (IsOpen && !approached) {
                Close();

                toggled = true;
            }
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
