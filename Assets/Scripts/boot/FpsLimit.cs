using UnityEngine;

namespace boot
{
    public class FpsLimit : MonoBehaviour
    {
        public int fps = 60;
        private void Awake()
        {
            Application.targetFrameRate = fps;
        }

        private void Update()
        {
            if (Application.targetFrameRate != fps)
                Application.targetFrameRate = fps;
        }
    }
}
