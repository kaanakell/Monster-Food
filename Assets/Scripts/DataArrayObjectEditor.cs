using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataArrayObject))]
public class DataArrayObjectEditor : Editor
{
    private SerializedProperty dataArray;
    private int previousSize = 0;

    private void OnEnable()
    {
        dataArray = serializedObject.FindProperty("dataArray");
        previousSize = dataArray.arraySize; // Store initial size
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Sync the data

        EditorGUILayout.PropertyField(dataArray, true); // Display the array in the inspector

        // Check if the array size has changed
        if (dataArray.arraySize != previousSize)
        {
            Debug.Log("Array size changed from " + previousSize + " to " + dataArray.arraySize);
            OnArraySizeChanged();
            previousSize = dataArray.arraySize; // Update previous size
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes to the ScriptableObject
    }

    private void OnArraySizeChanged()
    {
        // Put any additional logic you need to execute when the array size changes
        // Example: Logging or modifying other properties
          /*for(int i = 0; i < dataArrays.Length; i++){
               data.Id = i;
           }*/
        Debug.Log("Array size has been updated.");
    }
}
