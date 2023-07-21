using System;

public static class NameGenerator
{
    private static string[] prefixes = {"Ela", "Rob", "Gwen", "Wulfric", "Bran", "Maud", "Hugh", "Agnes", "Will", "Elin", "Ber", "Alys", "Edric", "Sybil", "Rolf", "Ada", "Hal", "Isolde", "Egon", "Meg"};
    private static string[] postfixes = {"win", "mund", "fride", "ric", "gar", "trude", "bert", "wina", "frid", "gith", "mond", "eburh", "trud", "burg", "wulf", "frith", "ward", "swith", "gund", "dric"};
        
    public static String CreateName()
    {
        int prefixIndex = new Random().Next(0, prefixes.Length);
        int postfixIndex = new Random().Next(0, postfixes.Length);

        return prefixes[prefixIndex] + postfixes[postfixIndex];
    }

}
