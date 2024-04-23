using Random = System.Random;

public static class Utils
{
    public static float GenerateRandom(float min, float max)
    {
        Random random = new Random();
        return (float)((random.NextDouble() * (max - min)) + min);
    }
    
    public static float GenerateRandomChance()
    {
        Random random = new Random();
        return (float)random.NextDouble() * 100;
    }
    
    public static int GenerateRandomIntNumberWhereMaxIs(int max)
    {
        Random random = new Random();
        return (int)(random.NextDouble() * (max) + 1);
    }
    
    public static int GenerateRandomIntBetween(int min, int max)
    {
        Random random = new Random();
        return (int)(random.NextDouble() * (max-min) + min);
    }
    
    public static bool TossCoin()
    {
        Random random = new Random();
        float nextDouble = (float)random.NextDouble() * 100;
        return nextDouble > 50;
    }
}
