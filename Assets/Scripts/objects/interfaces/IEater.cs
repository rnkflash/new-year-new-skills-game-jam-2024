using UnityEngine;

namespace objects.interfaces
{
    public interface IEater
    {
        public void StartEating(IEatable food);
        public int Eat();
        public void DropFood();
        public void FinishEating();
        public void Blurp();
        public Transform GetFoodHolder();
    }
}