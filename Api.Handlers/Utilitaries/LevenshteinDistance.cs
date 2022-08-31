namespace Api.Handlers.Utilitaries;

public abstract class LevenshteinDistance
{
    public static double Compare(string source, string target)
    {
        if (string.IsNullOrWhiteSpace(source.Trim()) || string.IsNullOrWhiteSpace(target.Trim()))
        {
            return 0.0;
        }
        if (source.Equals(target))
        {
            return 1.0;
        }

        source = source.ToUpper();
        target = target.ToUpper();

        return 1.0 - ComputeLevenshteinDistance(source, target) / (double)Math.Max(source.Length, target.Length);
    }

    private static int ComputeLevenshteinDistance(string source, string target)
    {
        int sourceWordCount = source.Length;
        int targetWordCount = target.Length;

        if (sourceWordCount == 0)
        {
            return targetWordCount;
        }

        if (targetWordCount == 0)
        {
            return sourceWordCount;
        }

        int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

        for (int i = 0; i <= sourceWordCount; i += 1)
        {
            distance[i, 0] = i;
        }
        for (int j = 0; j <= targetWordCount; j += 1)
        {
            distance[0, j] = j;
        }

        for (int i = 1; i <= sourceWordCount; i += 1)
        {
            for (int j = 1; j <= targetWordCount; j += 1)
            {
                int cost = target[j - 1].Equals(source[i - 1]) ? 0 : 1;
                distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
            }
        }

        return distance[sourceWordCount, targetWordCount];
    }
}
