using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.ActivationFunctions;

public class DataSetTest : MonoBehaviour
{
    private List<int[]> reducedData;

    //private ActivationNetwork network;
    //private double error = 1.0;
    //private int epoch = 0;
    //private double threshold = 0.01;
    //private BackPropagationLearning teacher;

    public TMP_Text consoleText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int[] classCounts = { 0, 0, 0, 0, 0, 0, 0 };
        
        //Creating reduced dataset
        //10 input parameters, 11 is output parameter
        reducedData = new List<int[]>();

        TextAsset fileData = Resources.Load<TextAsset>("Dataset/covtype");
        if(!fileData) WriteToCanvas("FileNotFound");

        string content = fileData.text;

        string[] lines = content.Split(new char[] { '\n', '\r' });

        int lineCount = 0;
        foreach (string line in lines)
        {
            int[] datapoint = new int[11];
            string[] dates = line.Split(',');
            if(dates.Length < 20)
            {
                Debug.Log(dates.Length);
                break;
            }
            classCounts[int.Parse(dates[54]) - 1]++;
            for (int i = 0; i < 10; i++)
            {
                datapoint[i] = int.Parse(dates[i]);
            }
            datapoint[10] = int.Parse(dates[54]);
            reducedData.Add(datapoint);

            lineCount++;
            if (lineCount % 100000 == 0) WriteToCanvas(lineCount);
        }
        Debug.Log("Marco");
        foreach (int i in classCounts) WriteToCanvas(i);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WriteToCanvas(object text)
    {
        consoleText.text += "\n";
        consoleText.text += text;
    }
}
