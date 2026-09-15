using UnityEngine;
using UnityEngine.UI;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeAetherDisplay : MonoBehaviour
    {
        [SerializeField] private PrototypeAetherWallet _wallet;
        [SerializeField] private Text _label;

        private int _lastDisplayedAmount = -1;

        private void Update()
        {
            int amount = _wallet != null ? _wallet.Amount : 0;
            if (amount == _lastDisplayedAmount)
                return;

            _lastDisplayedAmount = amount;
            if (_label != null)
                _label.text = $"AETHER: {amount}";
        }
    }
}
