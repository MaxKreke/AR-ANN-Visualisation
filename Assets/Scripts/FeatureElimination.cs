using UnityEngine;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.Neuro.Layers;
using Accord.Neuro.ActivationFunctions;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class FeatureElimination : MonoBehaviour
{
    private ActivationNetwork network;
    private double trainingError = 1.0;
    private float validationAccuracy = 0.0f;
    private int epoch = 0;
    private float threshold = .95f;
    private BackPropagationLearning teacher;

    private double[][] input;
    private double[][] output;
    private double[][] valInput;
    private double[][] valOutput;

    private bool finished = true;

    private double dataAmount;

    private float peakValidationAccuracy = 0.0f;
    private int peakValidationAccuracyEpoch = 0;

    private bool[] eliminatedFeatures = { false, true, true, true, true, true, true, true, true, true };
    private int remainingFeatures = 0;

    // Update is called once per frame
    void Update()
    {
        if (finished)return;
        if (validationAccuracy < threshold && epoch <= 70)
        {
            //Dividing error by trainingdataamount to get the average error per data point
            trainingError = teacher.RunEpoch(input, output)/dataAmount;

            //Updating first and incrementing second, so that the first iteration gets printed
            if (epoch % 1 == 0)
            {
                UpdateText();
                ComputeValidationAccuracy();
            }
            epoch++;

        }
        else
        {
            finished = true;
            UpdateText();
            ComputeValidationAccuracy();
            Debug.Log("Peak Validation Accuracy achieved in Epoch: " + peakValidationAccuracyEpoch);
            Debug.Log("Peak Validation Accuracy: " + peakValidationAccuracy);
            ComputePFIs();
        }
    }

    void UpdateText()
    {
        Debug.Log("Epoch: " + epoch + "\nError: " + Math.Sqrt(trainingError));
    }

    public void InitializeAndStart()
    {
        //Count remaining Features
        foreach (bool elim in eliminatedFeatures)if(!elim)remainingFeatures++;

        //Define Hidden Layers
        int[] layers = { 15, 9, 3 };

        //10 Input Nodes
        network = new ActivationNetwork(new SigmoidFunction(), remainingFeatures, layers);

        //Initialize random Weights using a Gaussian Distribution
        new GaussianWeights(network, 1.0).Randomize();

        //Reset Epochs to 0 and finished to true after network has been initialized to abort training
        epoch = 0;
        finished = true;

        //Set input and output data
        DataLoader dl = GetComponent<DataLoader>();
        input = dl.GetInputs();
        output = dl.GetOutputs();
        valInput = dl.GetValInputs();
        valOutput = dl.GetValOutputs();

        dataAmount = (double)dl.GetDataAmount();
        
        //Remove eliminated Values from datasets
        for(int i = 0; i < input.Length; i++)input[i] = removeValueFromPoint(input[i]);
        for (int i = 0; i < valInput.Length; i++)valInput[i] = removeValueFromPoint(valInput[i]);

        //Create new Backpropagation Object
        teacher = new BackPropagationLearning(network);
        finished = false;
    }


    private float ComputeAccuracy(double[][] validationSet)
    {
        int correctPredictions = 0;
        //Iterate over validation Data and count correct classification.
        for (int i = 0; i < validationSet.Length; i++)
        {
            double[] prediction = network.Compute(validationSet[i]);
            double[] truth = valOutput[i];
            if (ComparePrediction(prediction, truth)) correctPredictions++;
        }
        return ((float)correctPredictions / (float)valInput.Length);
    }

    private void ComputeValidationAccuracy()
    {
        validationAccuracy = ComputeAccuracy(valInput);
        Debug.Log("Validation Accuracy: " + validationAccuracy);
        if (validationAccuracy > peakValidationAccuracy)
        {
            peakValidationAccuracy = validationAccuracy;
            peakValidationAccuracyEpoch = epoch;
        }
    }


    private int PredictionMax(double[] prediction)
    {
        double max = 0.0;
        int maxIdx = 0;
        for(int i = 0; i < prediction.Length; i++)
        {
            if(prediction[i] > max)
            {
                max = prediction[i];
                maxIdx = i;
            }
        }
        return maxIdx;
    }

    private bool ComparePrediction(double[] prediction, double[] truth)
    {
        return (PredictionMax(prediction) == PredictionMax(truth));
    }

    private void ComputePFIs()
    {
        float[] PFIs = new float[remainingFeatures];
        double[][][] permutatedValidationSets = new double[remainingFeatures][][];
        for (int i = 0; i < remainingFeatures; i++)
        {
            //Create a Copy of the validation set for each input parameter using static DeepCopy function
            permutatedValidationSets[i] = Utils.DeepCopy<double>(valInput);
            //Shuffle respective column
            Utils.ShuffleSingleColumn<double>(permutatedValidationSets[i], i);
            //Compute Accuracy on permutated Data
            PFIs[i] = validationAccuracy - ComputeAccuracy(permutatedValidationSets[i]);
            Debug.Log("PFI on shuffling parameter " + i + " = " + PFIs[i]);
        }
    }

    private double[] removeValueFromPoint(double[] point)
    {
        double[] newPoint = new double[remainingFeatures];
        for(int i = 0, j = 0; i < remainingFeatures; i++, j++)
        {
            while (eliminatedFeatures[j]) j++;
            newPoint[i] = point[j];
        }
        return newPoint;
    }


}
