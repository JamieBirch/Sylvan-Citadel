using Random = System.Random;

public static class Utils
{
    public static float GenerateRandom(float min, float max)
    {
        Random random = new Random();
        return (float)((random.NextDouble() * (max - min)) + min);
    }
}
