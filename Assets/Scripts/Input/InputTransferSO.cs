using UnityEngine;

namespace Inputs
{
    [CreateAssetMenu(fileName = "InputTransfer", menuName = "ScriptableObjects/Input/InputTransfer")]
    public class InputTransferSO : ScriptableObject
    {
        public IInputTransfer Input { get; private set; }

        public void SetInputTransfer(IInputTransfer input)
        {
            Input = input;
        }
    }
}
