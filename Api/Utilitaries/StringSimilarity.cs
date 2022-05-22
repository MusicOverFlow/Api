using System.Text.RegularExpressions;

namespace Api.Utilitaries;

public class StringSimilarity
{
    public double CompareStrings(string str1, string str2)
    {
        List<string> pairs1 = WordLetterPairs(str1.ToUpper());
        List<string> pairs2 = WordLetterPairs(str2.ToUpper());

        int intersection = 0;
        int union = pairs1.Count + pairs2.Count;

        for (int i = 0; i < pairs1.Count; i++)
        {
            for (int j = 0; j < pairs2.Count; j++)
            {
                if (pairs1[i] == pairs2[j])
                {
                    intersection++;
                    
                    pairs2.RemoveAt(j);
                    
                    break;
                }
            }
        }

        return (2.0 * intersection * 100) / union;
    }

    private List<string> WordLetterPairs(string str)
    {
        List<string> AllPairs = new List<string>();

        string[] Words = Regex.Split(str, @"\s");

        for (int w = 0; w < Words.Length; w++)
        {
            if (!string.IsNullOrEmpty(Words[w]))
            {
                string[] PairsInWord = LetterPairs(Words[w]);

                for (int p = 0; p < PairsInWord.Length; p++)
                {
                    AllPairs.Add(PairsInWord[p]);
                }
            }
        }
        
        return AllPairs;
    }

    private string[] LetterPairs(string str)
    {
        int numPairs = str.Length - 1;
        string[] pairs = new string[numPairs];

        for (int i = 0; i < numPairs; i++)
        {
            pairs[i] = str.Substring(i, 2);
        }
        
        return pairs;
    }
}
