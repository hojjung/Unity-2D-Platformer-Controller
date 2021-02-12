using UnityEngine;
using UnityEditor;
namespace MyMarmot.Tools
{
    [CustomEditor(typeof(SaveLoadMonoBase))]
    public class SaveLoadGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SaveLoadMonoBase script = (SaveLoadMonoBase)target;

            if (GUILayout.Button("Save"))
            {
                script.Save();
            }
            if (GUILayout.Button("Load"))
            {
                script.Load();
            }
        }



    }
}