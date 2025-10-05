using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public TMP_Text[] statusText;

    public void StatusPrint(int idx, object text)
    {
        statusText[idx].text = "" + text;
    }
}
