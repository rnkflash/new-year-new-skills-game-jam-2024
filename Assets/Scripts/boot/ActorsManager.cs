using System;
using UnityEngine;

namespace boot
{
    public class ActorsManager : MonoBehaviour
    {
        public static ActorsManager instance;

        public Transform defaultActorsHolder;

        private void Awake()
        {
            instance = this;
        }
    }
}