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

    private void Awake()
    {
        size = values.Length;
        SetInitialValues();
        //CopyValue();
        UpdateSlider();
        UpdateText();
    }

    private void SetInitialValues(){
        for(int i = 0; i < size; i++){
            values[i] = 2.0f*((float)Consts.mean[i]-(float)Consts.floor[i])/((float)Consts.ceil[i]-(float)Consts.floor[i])-1.0f;
        }
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

    public float GetValue(int i)
    {
        return values[i];
    }

    private int GetUnnormalizedValue(int idx)
    {
        return Mathf.RoundToInt(GetUnnormalizedValueF(idx));
        // return Mathf.RoundToInt((GetValue() * (float)Consts.stdDev[idx]) + (float)Consts.mean[idx]);
    }

    private float GetUnnormalizedValueF(int idx)
    {
        return ((float)Consts.floor[idx] + (float)Consts.ceil[idx] + (GetValue(idx) * ((float)Consts.ceil[idx]-(float)Consts.floor[idx])))/2.0f;
    }

    public void UpdateText()
    {
        //Display name of Attribute
        nameText.text = Consts.names[currentIndex];

        //Display value as string with 2 significant digits
        displayText.text = GetUnnormalizedValue(currentIndex).ToString() + Consts.units[currentIndex];
    }

    public void UpdateSelection()
    {
        ann.ColorBySelection(currentIndex);
    }

    public void SetCurrentIndex(int newIdx)
    {
        currentIndex = newIdx; 
        UpdateSlider();
        UpdateText();
        UpdateSelection();
    }

    public void Next()
    {
        SetCurrentIndex((currentIndex + 1) % size);
    }

    public void Prev()
    {
        SetCurrentIndex((currentIndex + size - 1) % size);
    }

    public double[] GetInput()
    {
        double[] input = new double[size];
        for(int i = 0; i < size; i++){
            float trueValue = GetUnnormalizedValueF(i);
            input[i]=(trueValue-Consts.mean[i])/Consts.stdDev[i];
        }
        return input;
    }
}
