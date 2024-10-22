using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DataArrayObject", order = 1)]
public class DataArrayObject : ScriptableObject
{
    public ItemClass[] dataArray; // Array to hold data

    public void ChangeIds()
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            var idProperty = dataArray[i];
            if (idProperty != null)
            {
                idProperty.Id= i;
            }
        }
    }
}
