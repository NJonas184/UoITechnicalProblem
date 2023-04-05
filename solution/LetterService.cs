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

    static string PARENT = "solution";
    static string PARENTDIRECTORY = $"{Directory.GetParent(PARENT)}";
    static string PROJECTDIRECTORY = $"{Directory.GetParent(PARENTDIRECTORY)}";
    static string ARCHIVE_PATH = $"{PROJECTDIRECTORY}/CombinedLetters/Archive";
    static string INPUT_PATH = $"{PROJECTDIRECTORY}/CombinedLetters/Input";
    static string ADMISSION_PATH = $"{INPUT_PATH}/Admission";
    static string SCHOLARSHIP_PATH = $"{INPUT_PATH}/Scholarship";
    static string OUTPUT_PATH = $"{PROJECTDIRECTORY}/CombinedLetters/Output";

    public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile) {
        // 1. Read both files
        // 2. take data from both files and combine
        // 3. check for appropriate dated folder using inputfile paths
        // 4. Write in resultFile

        string fileOne = File.ReadAllText(inputFile1);
        System.Console.WriteLine(fileOne);
        string fileTwo = File.ReadAllText(inputFile2);
        System.Console.WriteLine(fileTwo);
        string combinedFile = $"{fileOne}\n\n{fileTwo}";
        System.Console.WriteLine(combinedFile);

        File.WriteAllText(resultFile, combinedFile);
    }

    public List<string> CheckScholarships(string admissionFolder, string[] filesArray) {
        //returns input files that match
        
        
        var scholarshipFolder = $"{SCHOLARSHIP_PATH}/{admissionFolder}";
        List<string> idList = new List<string>();
        foreach(String file in filesArray){
            System.Console.WriteLine(file);
            string temp = file.Split('-')[1];
            string id = temp.Split('.')[0];
            System.Console.WriteLine(id); //Delete check
            string scholarshipFile = $"scholarship-{id}.txt";
            System.Console.WriteLine($"{scholarshipFolder}/{scholarshipFile}");
            if(File.Exists($"{scholarshipFolder}/{scholarshipFile}")){
                System.Console.WriteLine("True");
                string resultPath = $"{OUTPUT_PATH}/{admissionFolder}";
                if(Directory.Exists(resultPath)!= true){
                    Directory.CreateDirectory(resultPath);
                }
                string resultFile = $"{resultPath}/CombinedLetters-{id}.txt";
                string admissionFilePath = $"{file}";
                string scholarshipFilePath = $"{scholarshipFolder}/{scholarshipFile}";

                //Needs to be called by an object, change functions to be non-static and run using an object.
                CombineTwoLetters(admissionFilePath, scholarshipFilePath, resultFile);
                idList.Add(id);
            }
        }
        return idList;
        
    }



    public List<string> CombineAllLetters() {
        //Function that runs through all admission folders and checks corresponding scholarship folders
        var admissionDirectories = Directory.GetDirectories(ADMISSION_PATH);
        List<string> idList = new List<string>();
        //Add solution string
        foreach(string folder in admissionDirectories){
            string[] folderArray = folder.Split(@"\");
            string folderName = folderArray[folderArray.Length - 1];
            int files = Directory.GetFiles(folder).Count();
            if(files > 0)
            {
                List<string> temp = CheckScholarships(folderName, Directory.GetFiles(folder));
                foreach(string id in temp){
                    idList.Add(id);
                }
                
            }
            Directory.Move(folder, $"{ARCHIVE_PATH}/Admission/{folderName}");
            var scholarshipFolder = $"{SCHOLARSHIP_PATH}/{folderName}";
            if(Directory.Exists(scholarshipFolder)){
                //Check to see if scholarship exists
                Directory.Move(scholarshipFolder,$"{ARCHIVE_PATH}/Scholarship/{folderName}");
            }
            
        }
        return idList;
    }

    public static void Main(string[] args) {
        
        LetterService letterService = new LetterService();
        List<string> idList = letterService.CombineAllLetters();
        DateTime today = DateTime.Today;

        string report = $"{today.ToString("MM/dd/yyyy")} Report\n-----------------------------\n\n";
        report = report + $"Number of Combined Letters: {idList.Count}";
        //report = report + $"\n\t{idList}";
        foreach(string id in idList){
            //System.Console.WriteLine(id);
            report = report + $"\n\t{id}";
        }

        System.Console.WriteLine(report);

        File.WriteAllText($"{OUTPUT_PATH}/{today.ToString("MM-dd-yyyy")}-Report.txt", report);
    
    }

}