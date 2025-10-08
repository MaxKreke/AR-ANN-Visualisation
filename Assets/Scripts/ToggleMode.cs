using UnityEngine;
using TMPro;

public class ToggleMode : MonoBehaviour
{
    private bool training = true;
    public TMP_Text buttonText;

    public void SetMode()
    {
        training = !training;
        transform.GetChild(0).gameObject.SetActive(training);
        transform.GetChild(1).gameObject.SetActive(!training);
        if (training) buttonText.text = "Vorhersagemodus";
        else buttonText.text = "Trainingsmodus";
    }

}
