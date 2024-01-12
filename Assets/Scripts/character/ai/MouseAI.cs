using UnityEngine;

namespace character.ai
{
    public class MouseAI : MonoBehaviour
    {
        public Camera cam;
        public CavemanController caveman;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    caveman.SetDestination(hit.point);
                }
            }
        }
    }
}
