using UnityEngine;
using UnityEngine.UI;
using Simulation.Player;

namespace Simulation.UI
{
    class BloodyScreen: MonoBehaviour
    {
        [SerializeField]
        Image image;

        public void UpdateScreen(Player.Player player)
        {
            Color color = image.color;

            color.a = 1 - (float)player.Health / (float)player.MaxHealth;

            image.color = color;
        }

        void Awake()
        {
            if (!image)
                Destroy(gameObject);
        }

        void Start()
        {
            image.gameObject.SetActive(true);
        }
    }
}
