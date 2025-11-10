using UnityEngine;
using TMPro;

public class ToggleMode : MonoBehaviour
{
    private bool training = true;
    private Transform helpField;
    public TMP_Text buttonText;
    public ANNManager annM;
    public ANNContainer annC;
    public InputValues iv;

    private void Awake()
    {
        helpField = GameObject.Find("MainCanvas").transform.GetChild(2);
        helpField.GetChild(0).gameObject.SetActive(true);
        helpField.GetChild(2).gameObject.SetActive(true);
    }

    private void UpdateTransform(Transform t)
    {
        t.GetChild(0).gameObject.SetActive(training);
        t.GetChild(1).gameObject.SetActive(!training);
    }

    public bool GetIsTraining()
    {
        return training;
    }

    public void SetMode()
    {
        training = !training;
        UpdateTransform(transform);
        UpdateTransform(helpField);
        if (training)
        {
            buttonText.text = "Vorhersagemodus";
            annM.ResetColors();
            annC.ClearConsole();
            return;
        }
        buttonText.text = "Trainingsmodus";

        //Paint Selected Node as soon as Vorhersagemodus is enabled
        iv.UpdateSelection();

        annC.PredictInput();
    }
}
