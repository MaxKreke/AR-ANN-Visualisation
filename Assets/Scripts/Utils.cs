using UnityEngine;
using System;

public static class Utils
{
    //Shuffle function by courtesy of ChatGPT, edited and tested by me
    public static void Shuffle<T>(T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            // Swap
            T temp = array[k];
            array[k] = array[n];
            array[n] = temp;
        }
    }

    public static T[][] DeepCopy<T>(T[][] matrix)
    {
        int x = matrix.Length;
        T[][] copy = new T[x][];
        for(int i = 0; i < x; i++) 
        {
            int y = matrix[i].Length;
            T[] row = new T[y];
            Array.Copy(matrix[i], row, y);
            copy[i] = row;
        }
        return copy;
    }

    public static void ShuffleSingleColumn<T>(T[][] matrix, int columnIdx)
    {
        //Copy Values into Array
        T[] column = new T[matrix.Length];
        for (int i = 0; i < matrix.Length; i++) { column[i] = matrix[i][columnIdx]; }

        //Shuffle Array
        Shuffle<T>(column);

        //Write Values back into matrix
        for (int i = 0; i < matrix.Length; i++) { matrix[i][columnIdx]= column[i]; }
    }

}
