using UnityEngine;
using System;

public class DataLoader : MonoBehaviour
{
    public double[][] dpInputs;
    public double[][] dpOutputs;
    public double[][] valInputs;
    public double[][] valOutputs;
    //public List<Batch> trainBatches;
    //public List<Batch> valBatches;

    private int totalDataAmount = 1000;
    private int trainingDataAmount;
    private int validatingDataAmount;
    private int featureCount = 7;

    //private string datasetName = "Dataset/oversampled_covtype";
    private string datasetName = "Dataset/reduced_dataset";

    public double[][] GetInputs()
    {
        return dpInputs;
    }

    public double[][] GetOutputs()
    {
        return dpOutputs;
    }

    public double[][] GetValInputs()
    {
        return valInputs;
    }

    public double[][] GetValOutputs()
    {
        return valOutputs;
    }

    public int GetFeatureCount()
    {
        return featureCount;
    }

    private void Start()
    {
        trainingDataAmount = Mathf.FloorToInt(totalDataAmount * .7f);
        validatingDataAmount = Mathf.FloorToInt(totalDataAmount * .3f);
        LoadData();
    }

    public int GetDataAmount()
    {
        return dpInputs.Length;
    }

    public void LoadData()
    {
        //Load Dataset Asset into array of strings
        TextAsset fileData = Resources.Load<TextAsset>(datasetName);
        if (!fileData)
        {
            Debug.LogError("Problem loading Dataset!");
            return;
        }
        string content = fileData.text;
        string[] lines = content.Split(new string[] { "\r\n", "\r", "\n" },StringSplitOptions.None);

        Debug.Log(lines.Length + " Zeilen.");
        Utils.Shuffle(lines);
        Debug.Log(lines.Length + " Zeilen.");

        //Create inputs and Outputs for Training out of dingens
        dpInputs = new double[trainingDataAmount][];
        dpOutputs = new double[trainingDataAmount][];
        valInputs = new double[validatingDataAmount][];
        valOutputs = new double[validatingDataAmount][];

        prepareData(lines, dpInputs, dpOutputs, 0, trainingDataAmount);
        prepareData(lines, valInputs, valOutputs, trainingDataAmount, validatingDataAmount);

        //NOT RELEVANT FOR MAIN APP; ONLY EXECUTED WHEN FEATURE ELIMINATION IS PRESENT
        FeatureElimination test = GetComponent<FeatureElimination>();
        if (test != null) {
            Debug.Log("Initializing and Starting Network");
            Debug.Log("Loaded " + dpInputs.Length + " Training Data and " + valInputs.Length + "Validation Data.");
            test.InitializeAndStart();
        }
    }

    private void prepareData(string[] lines, double[][] inputs, double[][] outputs, int offset, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            string[] dates = lines[i+ offset].Split(',');
            if (dates.Length < featureCount + 1)
            {
                Debug.LogError("Weniger Daten als Dimensionen. Vorhandene daten: " + dates.Length + ". i = " + (i+ offset));
                return;
            }
            //Create Input arrays
            inputs[i] = new double[featureCount];
            for (int j = 0; j < featureCount; j++)
            {
                //Normalize Value and write it into input matrixs
                double normalizedValue = (double.Parse(dates[j]) - Consts.mean[j]) / Consts.stdDev[j];
                inputs[i][j] = normalizedValue;
            }

            //Create output labels
            //Sets label to 1 in the correct column
            outputs[i] = new double[3];
            outputs[i][int.Parse(dates[featureCount])] = 1;
        }
    }


}
