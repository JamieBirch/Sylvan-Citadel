using System;
using Random = UnityEngine.Random;

public static class ThoughtGenerator
{
    private static string[] randomThough =
    {
        "I really love true crime town criers.", 
        "Plotting world domination, one slimy trail at a time.", 
        "If I had arms, I'd totally be lifting weights right now.", 
        "Wondering if I could become a snailfluencer. #SlimyAndSuccessful",
        "Could I start a snail army and take over the world? Only one way to find out."
    };
    
    public static String RandomThought()
    {
        int prefixIndex = new System.Random().Next(0, randomThough.Length);

        return randomThough[prefixIndex];
    }


}
