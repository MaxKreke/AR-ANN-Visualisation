using UnityEngine;
using UnityEngine.UI;
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

public class ANNContainer: MonoBehaviour
{
    private ActivationNetwork network;
    private double error = 1.0;
    private int epoch = 0;
    private double threshold = 0.01;
    private BackPropagationLearning teacher;
    private int inputCount;

    private double[][] input;
    private double[][] output;

    public LayerList ll;
    public GameObject nodePrefab;
    public GameObject inputNodePrefab;
    public GameObject outputNodePrefab;
    public GameObject weightPrefab;

    public Button startButton;
    private CanvasController cc;

    private float scalingFactor = .5f;

    private bool finished = true;

    void Start()
    {
        inputCount = GetComponent<DataLoader>().GetFeatureCount();
        cc = GameObject.Find("MainCanvas").GetComponent<CanvasController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)return;
        if (error > threshold && epoch <= 1000)
        {
            //Dividing error by trainingdataamount to get the average error per data point
            error = teacher.RunEpoch(input, output) / (double)input.Length;

            //Updating first and incrementing second, so that the first iteration gets printed
            if (epoch % 10 == 0)
            {
                UpdateNetwork();
                UpdateText();
            }
            epoch++;

        }
        else
        {
            finished = true;
            startButton.interactable = false;
            UpdateNetwork();
            UpdateText();
        }
    }

    void UpdateText()
    {
        WriteToCanvas("Epoch: " + epoch + "\nError: " + Math.Sqrt(error));
    }

    private void WriteToCanvas(object text)
    {
        cc.StatusPrint(0, text);
    }

    private IEnumerator InitializeNetwork()
    {
        //Reset Epochs to 0 and finished to true after network has been initialized to abort training
        epoch = 0;
        finished = true;

        //Delete previous network if there is any
        foreach(Transform layer in transform.GetChild(0))
        {
            Destroy(layer.gameObject);
        }

        //Necessary because Destroying objects is only done at the end of the frame
        yield return null;

        List<int> allNodes = ll.layers;
        int layerCount = allNodes.Count;

        //Initialize Network

        //Check layer count
        if (layerCount == 0) yield break;
        if (layerCount > 5) yield break;

        int[] layers = new int[layerCount + 1];

        for (int i = 0; i < layerCount; i++)
        {
            layers[i] = allNodes[i];
        }

        //3 Output Nodes
        layers[layerCount] = 3;

        //10 Input Nodes
        network = new ActivationNetwork(new SigmoidFunction(), inputCount, layers);

        //Initialize random Weights using a Gaussian Distribution
        new GaussianWeights(network, 1.0).Randomize();

        CreateLayer(-network.Layers.Length, 0, true);
        for (int i = 0; i < network.Layers.Length; i++)CreateLayer(2 + 2 * i - network.Layers.Length, i, false);

        //Message ANNManager that layers have been created
        GetComponent<ANNManager>().CollectLayers();
    }

    private void CreateLayer(float offset, int idx, bool input)
    {
        ActivationLayer layer = network.Layers[idx] as ActivationLayer;
        Transform container = transform.GetChild(0);
        String name = "Layer " + idx;
        if (input) name = "Input";
        GameObject layerObj = new GameObject(name);
        layerObj.transform.SetParent(container);
        layerObj.transform.localScale = Vector3.one;
        layerObj.transform.localRotation = Quaternion.identity;
        layerObj.transform.localPosition = (Vector3.right *offset + Vector3.up/4)* scalingFactor;

        int nodeCount = layer.Neurons.Length;
        if (input) nodeCount = inputCount;

        for (int i = 0; i < nodeCount; i++)
        {

            //Instantiate node GameObject depending on whether its input, output or hidden layer
            GameObject nodeObj;
            if (input) nodeObj = GameObject.Instantiate(inputNodePrefab, layerObj.transform);
            else if (idx == network.Layers.Length - 1)
            {
                nodeObj = GameObject.Instantiate(outputNodePrefab, layerObj.transform);
                if(i > 2)
                {
                    Debug.LogError("Index exceeds class number");
                    return;
                }
                nodeObj.GetComponent<OutputNodeRef>().SetClass(i);
            }
            else nodeObj = GameObject.Instantiate(nodePrefab, layerObj.transform);

            nodeObj.name = "Node " + idx + "," + i;
            //Calculate Position
            Vector3 nodePosition = (2 * i - nodeCount) * Vector3.forward * .165f;
            nodeObj.transform.localPosition = nodePosition;

            if(input)
            {
                nodeObj.GetComponent<InputDescription>().SetAttributeName(i);
                nodeObj.transform.localScale = new Vector3(.2f, .06f, .2f);
                continue;
            }
            
            nodeObj.transform.localScale = new Vector3(.2f, .2f, .2f);

            //Pass Neuron Object to node Object's noderef script
            NodeRef nr = nodeObj.GetComponent<NodeRef>();
            nr.AssignNode(layer.Neurons[i] as ActivationNeuron);

            //Define number of iterations to equal the number of inputs, any node in the given layer has
            int otherNodeCount = network.Layers[idx].Neurons[0].InputsCount;
            //Create Weights leading into Node
            for (int j = 0; j < otherNodeCount; j++)
            {
                //Find other connecting node of weight in hierarchy
                Transform otherNode = container.GetChild(idx).GetChild(j);
                CreateConnectingWeightBetweenObjects(nodeObj.transform, otherNode, j);
            }
            nr.collectWeightRefs();
        }

        if (input) return;

        //Attach LayerManager to script and have it find its child nodes
        LayerManager lm = layerObj.AddComponent<LayerManager>();
        lm.CollectNodeRefs();
    }


    private void CreateConnectingWeightBetweenObjects(Transform obj1, Transform obj2, int idx)
    {
        GameObject weight = GameObject.Instantiate(weightPrefab, obj1);
        WeightRef wr = weight.GetComponent<WeightRef>();

        //Assign connecting Nodes
        wr.AssignTransforms(obj2,obj1);

        //Get Node
        Neuron neuron = obj1.gameObject.GetComponent<NodeRef>().GetNeuron();
        //Assign Weight Reference
        wr.AssignWeight(neuron.Weights, idx);

        //Using world space positions to not get results skewed by scaling
        Vector3 nodePosition = obj1.position;
        Vector3 prevNodePosition = obj2.position;
        Vector3 weightPosition = (nodePosition + prevNodePosition) / 2;
        weight.transform.position = weightPosition;

        //Stretching cylinder to be length of the distance between the nodes it's connecting
        float absoluteDistance = (nodePosition - prevNodePosition).magnitude;
        //Define thickness
        float thickness = .16f;
        weight.transform.localScale = new Vector3(thickness, absoluteDistance*12.5f/scalingFactor, thickness);
        wr.AssignThickness(thickness);
        wr.UpdateColorAndShape();

        // ChatGPT generated code: rotate cylinder so its Y axis aligns with direction
        weight.transform.rotation = Quaternion.FromToRotation(Vector3.up, weightPosition - prevNodePosition);

    }

    private void CheckStartButton()
    {
        if(ll.layers.Count == 0)startButton.interactable = false;
        else startButton.interactable = true;
    }

    //Add Layer and then re-create network
    public void AddLayer()
    {
        if (!ll.AddLayer())
        {
            CheckStartButton();
            return;
        }
        IEnumerator render = InitializeNetwork();
        StartCoroutine(render);
        CheckStartButton();
    }

    //Remove Layer and then re-create network
    public void RemoveLayer()
    {

        if (!ll.RemoveLayer())
        {
            CheckStartButton();
            return;
        }
        IEnumerator render = InitializeNetwork();
        StartCoroutine(render);
        CheckStartButton();
    }

    public void SetFinished(bool _finished)
    {
        finished = _finished;
    }


    //Start Training
    public void StartProcess()
    {
        //Set input and output data
        DataLoader dl = GetComponent<DataLoader>();
        input = dl.GetInputs();
        output = dl.GetOutputs();

        //Create new Backpropagation Object
        teacher = new BackPropagationLearning(network);
    }

    public void ResetNetwork()
    {
        IEnumerator render = InitializeNetwork();
        StartCoroutine(render);
        CheckStartButton();
    }

    private void UpdateNetwork()
    {
        GetComponent<ANNManager>().UpdateLayers();
    }

}
