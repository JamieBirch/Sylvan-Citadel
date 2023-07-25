using System;

public static class NameGenerator
{
    private static string[] namePrefixes = {"Ela", "Rob", "Gwen", "Wulfric", "Bran", "Maud", "Hugh", "Agnes", "Will", "Elin", "Ber", "Alys", "Edric", "Sybil", "Rolf", "Ada", "Hal", "Isolde", "Egon", "Meg"};
    private static string[] namePostfixes = {"win", "mund", "fride", "ric", "gar", "trude", "bert", "wina", "frid", "gith", "mond", "eburh", "trud", "burg", "wulf", "frith", "ward", "swith", "gund", "dric"};
    
    private static string[] districtPrefixes = {"Grim", "Thorn", "Black", "Stone", "Wulf", "Ash", "Oak", "Iron", "Red", "Silver", "Cold", "Amber", "Raven", "Wolf", "Gold", "Green", "Swift", "Storm", "Mist", "Hollow"};
    private static string[] districtPostfixes = {"wood", "ford", "shire", "brook", "dale", "burg", "mead", "haven", "cliff", "field", "vale", "gate", "wick", "stoke", "thorp", "wald", "stead", "wick", "moor", "ton"};
    
    public static String CreateHumanName()
    {
        int prefixIndex = new Random().Next(0, namePrefixes.Length);
        int postfixIndex = new Random().Next(0, namePostfixes.Length);

        return namePrefixes[prefixIndex] + namePostfixes[postfixIndex];
    }
    
    public static String CreateDistrictName()
    {
        int prefixIndex = new Random().Next(0, districtPrefixes.Length);
        int postfixIndex = new Random().Next(0, districtPostfixes.Length);

        return districtPrefixes[prefixIndex] + districtPostfixes[postfixIndex];
    }

}
