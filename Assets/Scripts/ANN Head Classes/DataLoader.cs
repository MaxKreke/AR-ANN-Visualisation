using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;


public class DataLoader : MonoBehaviour
{
    public List<Batch> trainBatches;
    public List<Batch> valBatches;

    private int batchSize;
    private int featureCount;

    public CanvasController cc;
    public ARSession session;

    private string datasetName = "Dataset/reduced_dataset";

    private float startTime; 

    private void Start()
    {
        batchSize = Consts.batchSize;
        featureCount = Consts.inputSize;
        LoadData();
    }

    public Batch GetRandomBatch(bool training)
    {
        if(training)return trainBatches[UnityEngine.Random.Range(0, trainBatches.Count)];
        else return valBatches[UnityEngine.Random.Range(0, valBatches.Count)];
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

        //Shuffle Data Points
        Utils.Shuffle(lines);

        //Create Batches in asyncrhonous as to not slow down loading of the scene.
        cc.StatusPrint(0, "Lade Daten...");

        startTime = Time.time;
        IEnumerator batchCreation = CreateBatches(lines); 
        StartCoroutine(batchCreation);
    }

    private IEnumerator CreateBatches(string[] lines)
    {
        int dataCount = lines.Length;

        //Create Batches
        int trainBatchCount = Mathf.FloorToInt((dataCount / batchSize) * .9f);
        int valBatchCount = Mathf.FloorToInt((dataCount / batchSize) * .1f);

        trainBatches = new List<Batch>();
        valBatches = new List<Batch>();


        for (int i = 0; i < trainBatchCount; i++)
        {
            trainBatches.Add(PrepareBatch(lines, i * batchSize));
            if (i % 25 == 0)
            {
                cc.StatusPrint(0, (((float)(100*i))/trainBatchCount).ToString("0") + "% der Trainingsdaten geladen.");
                yield return null;
            }
        }

        for (int i = 0; i < valBatchCount; i++)
        {
            valBatches.Add(PrepareBatch(lines, trainBatchCount + i * batchSize));
            if (i % 25 == 0)
            {
                cc.StatusPrint(0, (((float)(100 * i)) / valBatchCount).ToString("0") + "% der Validierungsdaten geladen.");
                yield return null;
            }
        }

        Debug.Log("Training Batches: " + trainBatches.Count);
        Debug.Log("Validation Batches: " + valBatches.Count);



        cc.StatusPrint(1, "Daten Geladen in "+(Time.time-startTime)+" Sekunden.");
        cc.StatusPrint(0, "Bewege die Kamera und visiere den Marker an.");
        session.enabled = true;
        yield return null;
    }

    private void PrepareData(string[] lines, double[][] inputs, double[][] outputs, int offset, int iterations)
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

    private Batch PrepareBatch(string[] lines, int offset)
    {
        //Create input/output matrices
        double[][] inputs = new double[batchSize][];
        double[][] outputs = new double[batchSize][];

        //Fill matrices
        PrepareData(lines, inputs, outputs, offset, batchSize);

        //Create Batch from matrices
        return new Batch(inputs, outputs);
    }

}
