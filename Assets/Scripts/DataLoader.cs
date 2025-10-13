using UnityEngine;
using System;
using System.Collections.Generic;


public class DataLoader : MonoBehaviour
{
    public List<Batch> trainBatches;
    public List<Batch> valBatches;

    private int batchSize;
    private int featureCount;

    //private string datasetName = "Dataset/oversampled_covtype";
    private string datasetName = "Dataset/reduced_dataset";

    public Batch GetRandomBatch(bool training)
    {
        if(training)return trainBatches[UnityEngine.Random.Range(0, trainBatches.Count)];
        else return valBatches[UnityEngine.Random.Range(0, valBatches.Count)];
    }

    private void Start()
    {
        batchSize = Consts.batchSize;
        featureCount = Consts.inputSize;
        LoadData();
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
        int dataCount = lines.Length;

        //Shuffle Data Points
        Utils.Shuffle(lines);


        //Create Batches
        int trainBatchCount = Mathf.FloorToInt((dataCount / batchSize) * .9f);
        int valBatchCount = Mathf.FloorToInt((dataCount / batchSize) * .1f);

        trainBatches = new List<Batch>();
        valBatches = new List<Batch>();

        for(int i = 0; i < trainBatchCount; i++)
        {
            trainBatches.Add(prepareBatch(lines, i * batchSize));
        }

        for (int i = 0; i < valBatchCount; i++)
        {
            valBatches.Add(prepareBatch(lines, trainBatchCount+i * batchSize));
        }

        Debug.Log("Training Batches: " + trainBatches.Count);
        Debug.Log("Validation Batches: " + valBatches.Count);
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

    private Batch prepareBatch(string[] lines, int offset)
    {
        //Create input/output matrices
        double[][] inputs = new double[batchSize][];
        double[][] outputs = new double[batchSize][];

        //Fill matrices
        prepareData(lines, inputs, outputs, offset, batchSize);

        //Create Batch from matrices
        return new Batch(inputs, outputs);
    }

}
