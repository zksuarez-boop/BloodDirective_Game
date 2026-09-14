using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeAetherWallet : MonoBehaviour
    {
        private int _amount;

        public int Amount => _amount;

        public void Add(int amount)
        {
            _amount += Mathf.Max(0, amount);
        }

        public bool TrySpend(int amount)
        {
            int cost = Mathf.Max(0, amount);
            if (_amount < cost)
                return false;

            _amount -= cost;
            return true;
        }
    }
}
