using UnityEngine;
using UnityEngine.Events;

namespace MyMarmot.Tools
{
    [System.Serializable]
    public class InputEventIter
    {
        public UnityEvent InputToWantExecute;
        public KeyCode InputKey;
        public void TryExecute()
        {
            if (Input.GetKeyDown(InputKey))
            {
                InputToWantExecute?.Invoke();
            }
        }

    }
}