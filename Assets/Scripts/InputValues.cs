using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputValues : MonoBehaviour
{
    public Slider slider;
    public TMP_Text nameText;
    public TMP_Text displayText;

    public ANNManager ann;

    private int currentIndex = 0;
    private int size = 0;

    private float[] values = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

    private void Start()
    {
        size = values.Length;
        CopyValue();
        UpdateText();
    }

    public void SetValue(float value)
    {
        values[currentIndex]=value;
    }

    public float GetValue()
    {
        return values[currentIndex];
    }

    //Copy value from slider into array
    public void CopyValue()
    {
        SetValue(slider.value);
    }

    //Write Value from array into slider
    public void UpdateSlider()
    {
        slider.value = GetValue();
    }

    public void UpdateText()
    {
        //Display name of Attribute
        nameText.text = Consts.names[currentIndex];

        //Display value as string with 2 significant digits
        displayText.text = GetUnnormalizedValue().ToString() + Consts.units[currentIndex];
    }

    public void UpdateSelection()
    {
        ann.ColorBySelection(currentIndex);
    }

    public int GetUnnormalizedValue()
    {
        return Mathf.RoundToInt(((GetValue()) * (float)Consts.stdDev[currentIndex]) + (float)Consts.mean[currentIndex]);
    }

    public void Next()
    {
        currentIndex=(currentIndex+1)%size;
        UpdateSlider();
        UpdateText();
        UpdateSelection();
    }

    public void Prev()
    {
        currentIndex = (currentIndex + size - 1)%size;
        UpdateSlider();
        UpdateText();
        UpdateSelection();
    }

    public double[] GetInput()
    {
        double[] input = new double[size];
        for(int i = 0; i < size; i++)input[i]=values[i];
        return input;
    }
}
