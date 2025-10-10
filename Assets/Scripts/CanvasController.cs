using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public TMP_Text[] statusText;
    public TMP_Text toggleButtonText;
    public GameObject toggleButton;
    private ANNContainer ann;
    private bool menuVisible = true;

    public void StatusPrint(int idx, object text)
    {
        statusText[idx].text = "" + text;
    }

    public void ActivateToggleButton()
    {
        toggleButton.SetActive(true);
    }

    public void AssignANNContainer(ANNContainer _ann)
    {
        ann = _ann;
    }

    public void ToggleMenu()
    {
        menuVisible = !menuVisible;
        ann.ToggleMenu(menuVisible);
        if (menuVisible) toggleButtonText.text = "Menü Ausblenden";
        else toggleButtonText.text = "Menü Einblenden";
    }

}
