using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DataArrayObject", order = 1)]
public class DataArrayObject : ScriptableObject
{
    public ItemClass[] dataArrays; // Array to hold data
}
