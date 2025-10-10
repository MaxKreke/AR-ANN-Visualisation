using UnityEngine;

public static class Consts
{
    //Mean of every respective type of measurements in the dataset as reported in the covtype.info file
    public static readonly double[] mean = { 2959.36, 269.43, 2350.15, 212.15, 223.32, 142.53, 1980.29 };

    //Standard deviation of every respective type of measurements in the dataset as reported in the covtype.info file
    public static readonly double[] stdDev = { 279.98, 212.55, 1559.25, 26.77, 19.77, 38.27, 1324.19 };

    public static readonly string[] names = {        
        "Erhebung",
        "Entfernung zu Gew‰ssern",
        "Entfernung zu Straﬂen",
        "Hillshade Morgens",
        "Hillshade Mittags",
        "Hillshade Nachmittags",
        "Entfernung zu Brandstelle" 
    };

    //Units for every attribute
    public static readonly string[] units = { "m", "m", "m", "/255", "/255", "/255", "m" };

    public static readonly int inputSize = 7;
    public static readonly int outputSize = 3;
}
