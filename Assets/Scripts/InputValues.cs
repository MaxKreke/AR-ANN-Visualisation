using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputValues : MonoBehaviour
{
    public Slider slider;
    public TMP_Text displayText;
    private int currentIndex = 0;
    private int size = 0;

    private float[] values = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

    private void Start()
    {
        size = values.Length;
    }

    public void UpdateText()
    {
        SetValue(slider.value);

        //Display value as string with 4 significant digits
        displayText.text = GetValue().ToString("F4");
    }

    public void UpdateSlider()
    {
        slider.value = GetValue();
    }

    public void SetValue(float value)
    {
        values[currentIndex]=value;
    }

    public float GetValue()
    {
        return values[currentIndex];
    }

    public void Next()
    {
        currentIndex=(currentIndex+1)%size;
        UpdateSlider();
    }

    public void Prev()
    {
        currentIndex = (currentIndex + size - 1)%size;
        UpdateSlider();
    }

    public double[] GetInput()
    {
        double[] input = new double[size];
        for(int i = 0; i < size; i++)input[i]=values[i];
        return input;
    }

}
