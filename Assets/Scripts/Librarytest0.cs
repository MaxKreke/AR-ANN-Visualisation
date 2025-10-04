using UnityEngine;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.ActivationFunctions;
using TMPro;
using System;

public class Librarytest0 : MonoBehaviour
{
    // XOR Training Data
    private double[][] inputs =
    {
        new double[] { 0, 0 },
        new double[] { 0, 1 },
        new double[] { 1, 0 },
        new double[] { 1, 1 }
    };

    private double[][] outputs =
    {
        new double[] { 0 },
        new double[] { 1 },
        new double[] { 1 },
        new double[] { 0 }
    };
    private ActivationNetwork network;
    private double error = 1.0;
    private int epoch = 0;
    private double threshold = 0.01;
    private BackPropagationLearning teacher;

    public TMP_Text consoleText;

    private bool finished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        network = new ActivationNetwork(
            new SigmoidFunction(2), // Activation function
            inputsCount: 2,              // Number of input neurons
            neuronsCount: new int[] { 8, 1 } // Hidden layer: 2 neurons, Output layer: 1 neuron
        );

        // Step 2: Initialize Weights
        new NguyenWidrow(network).Randomize();

        // Step 3: Train the Network using Backpropagation
        teacher = new BackPropagationLearning(network);

        consoleText.text = "Training!";
        //Debug.Log("Training!");

    }

    // Update is called once per frame
    void Update()
    {
        if (finished) return;
        if (error > threshold)
        {
            error = teacher.RunEpoch(inputs, outputs);
            epoch++;
            if (epoch % 10 == 0) UpdateText();

        }
        else
        {   
            finished = true;
            UpdateText();
        }
    }

    void UpdateText()
    {
        consoleText.text = "Epoch: " + epoch + "\nError: " + error;
    }
}
