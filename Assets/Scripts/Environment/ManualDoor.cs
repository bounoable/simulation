using UnityEngine;

namespace Simulation.Environment
{
    [RequireComponent(typeof(DoorHandle))]
    class ManualDoor: Door
    {
        DoorHandle handle;

        bool HandleTriggered()
        {
            var player = FindObjectOfType<Player.Player>();

            if (!player)
                return false;
            
            if (Vector3.Distance(transform.position, player.Position) > handle.Range)
                return false;
            
            return HandleClicked();
        }

        bool HandleClicked()
        {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (UnityEngine.Physics.Raycast(ray, out hit, handle.Range)) {
                    var handle = hit.collider.GetComponent<DoorHandle>();

                    return handle && handle == this.handle;
                }
            }

            return false;
        }

        void Awake()
        {
            handle = GetComponent<DoorHandle>();
        }

        void OnMouseDown()
        {
            if (HandleTriggered())
                Toggle();
        }
    }
}