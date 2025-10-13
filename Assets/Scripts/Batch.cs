using UnityEngine;

public class Batch
{
    private double[][] input;
    private double[][] output;
    private int batchSize;

    public Batch(double[][] _input, double[][] _output)
    {
        input = _input;
        output = _output;
        batchSize = input.Length;
    }

    //Getters
    public double[][] GetInput()
    {
        return input;
    }

    public double[][] GetOutput()
    {
        return output;
    }

    public int GetBatchSize()
    {
        return batchSize;
    }

    public double[] GetInputRow(int i)
    {
        return input[i];
    }

    public double[] GetOutputRow(int i)
    {
        return output[i];
    }

}
