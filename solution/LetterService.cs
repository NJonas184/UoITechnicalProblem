using System;
using System.IO;


public interface ILetterService {

    /// <summary>
    /// Combine two letter files into one file.
    /// <param name="inputFile1">File path for the first letter.</param>
    /// <param name="inputFile2">File path for the second letter.</param>
    /// <param name="resultFile">File path for the combined letter.</param>
    void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile);
}

public class LetterService : ILetterService {


    public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile) {
        // 1. Read both files
        // 2. take data from both files and combine
        // 3. check for appropriate dated folder using inputfile paths
        // 4. Write in resultFile

        string fileOne = File.ReadAllText(inputFile1);
        string fileTwo = File.ReadAllText(inputFile2);
        string combinedFile = $"{fileOne}\n\n{fileTwo}";

        File.WriteAllText(resultFile, combinedFile);
    }

    public static void Main(string[] args){

        System.Console.WriteLine($"Hello World!");
    }

}