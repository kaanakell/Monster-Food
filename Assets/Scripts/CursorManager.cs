using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public GameObject cursorObject; // Assign your cursor GameObject here

    private void Start()
    {
        cursorObject.SetActive(false); // Hide cursor at start
    }

    public void ShowCursor(Sprite cursorSprite)
    {
        cursorObject.SetActive(true);
        cursorObject.GetComponent<Image>().sprite = cursorSprite;
    }

    public void HideCursor()
    {
        cursorObject.SetActive(false);
    }

    public void UpdateCursorPosition(Vector3 position)
    {
        cursorObject.transform.position = position;
    }
}
