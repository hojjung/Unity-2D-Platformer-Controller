using UnityEngine;
using UnityEngine.UI;

namespace MyMarmot.Tools {
    public class TextUpdater : MonoBehaviour
    {
        [Header("Default is CurrentObjText")]
        [SerializeField]
        private Text _targetText;
        public Text m_TargetText { get => _targetText; private set => _targetText = value; }

        void Awake()
        {
            m_TargetText = GetComponent<Text>();
        }

        public void SetString(string textStr)
        {
            m_TargetText.text = textStr;
        }
    }
}