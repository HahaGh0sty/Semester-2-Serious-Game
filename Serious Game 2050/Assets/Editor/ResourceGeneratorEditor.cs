using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceGenerator))]
public class ResourceGeneratorEditor : Editor
{
    private string[] resourceNames;

    private void OnEnable()
    {
        resourceNames = System.Enum.GetNames(typeof(ResourceGenerator.ResourceType));
    }

    public override void OnInspectorGUI()
    {
        // Draw default inspector except arrays
        DrawDefaultInspectorExcept("resourceLose", "resourceGet");

        ResourceGenerator generator = (ResourceGenerator)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Resource Lose", EditorStyles.boldLabel);
        DrawResourceArray(generator.resourceLose, ref generator.resourceLose);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Resource Get", EditorStyles.boldLabel);
        DrawResourceArray(generator.resourceGet, ref generator.resourceGet);

        // Apply changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(generator);
        }
    }

    void DrawResourceArray(int[] array, ref int[] backingArray)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = EditorGUILayout.IntField(resourceNames[i], array[i]);
        }
    }

    void DrawDefaultInspectorExcept(params string[] exclude)
    {
        SerializedProperty prop = serializedObject.GetIterator();
        prop.NextVisible(true);
        do
        {
            bool shouldExclude = false;
            foreach (string s in exclude)
            {
                if (prop.name == s)
                {
                    shouldExclude = true;
                    break;
                }
            }

            if (!shouldExclude)
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        } while (prop.NextVisible(false));

        serializedObject.ApplyModifiedProperties();
    }
}
