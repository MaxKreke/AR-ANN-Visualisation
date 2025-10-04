using UnityEngine;

public class Batch
{
    public double[][] input;
    public double[][] output;
    public int batchSize;

    public Batch(double[][] _input, double[][] _output)
    {
        input = _input;
        output = _output;
        batchSize = input.Length;
    }
}
