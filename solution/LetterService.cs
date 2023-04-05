using System;
using System.IO;


public interface ILetterService {

    /// <summary>
    /// Combine two letter files into one file.
    /// <param name="inputFile1">File path for the first letter.</param>
    /// <param name="inputFile2">File path for the second letter.</param>
    /// <param name="resultFile">File path for the combined letter.</param>
    void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile);
} //ILetterSErvice

public class LetterService : ILetterService {
    //Class for LetterService solution
    static string PARENT = "solution";
    static string PARENTDIRECTORY = $"{Directory.GetParent(PARENT)}";
    static string PROJECTDIRECTORY = $"{Directory.GetParent(PARENTDIRECTORY)}";
    static string ARCHIVE_PATH = $"{PROJECTDIRECTORY}/CombinedLetters/Archive";
    static string INPUT_PATH = $"{PROJECTDIRECTORY}/CombinedLetters/Input";
    static string ADMISSION_PATH = $"{INPUT_PATH}/Admission";
    static string SCHOLARSHIP_PATH = $"{INPUT_PATH}/Scholarship";
    static string OUTPUT_PATH = $"{PROJECTDIRECTORY}/CombinedLetters/Output";

    public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile) {
        // Combines two letters and writes the result in resultFile

        string fileOne = File.ReadAllText(inputFile1);
        System.Console.WriteLine(fileOne);
        string fileTwo = File.ReadAllText(inputFile2);
        System.Console.WriteLine(fileTwo);
        string combinedFile = $"{fileOne}\n\n{fileTwo}";
        System.Console.WriteLine(combinedFile);

        File.WriteAllText(resultFile, combinedFile);
    } //CombineTwoLetters

    public List<string> CheckScholarships(string admissionFolder, string[] filesArray) {
        //Checks to see if id is present in both relevant folders, then calls CombineTwoLetters function.
        //Returns a list of ids
        
        var scholarshipFolder = $"{SCHOLARSHIP_PATH}/{admissionFolder}";
        List<string> idList = new List<string>();
        foreach(String file in filesArray){
            
            string temp = file.Split('-')[1];
            string id = temp.Split('.')[0];
            
            string scholarshipFile = $"scholarship-{id}.txt";
            
            if(File.Exists($"{scholarshipFolder}/{scholarshipFile}")){
                System.Console.WriteLine("True");
                string resultPath = $"{OUTPUT_PATH}/{admissionFolder}";
                if(Directory.Exists(resultPath)!= true){
                    Directory.CreateDirectory(resultPath);
                } //if
                string resultFile = $"{resultPath}/CombinedLetters-{id}.txt";
                string admissionFilePath = $"{file}";
                string scholarshipFilePath = $"{scholarshipFolder}/{scholarshipFile}";

                CombineTwoLetters(admissionFilePath, scholarshipFilePath, resultFile);
                idList.Add(id);
            } //if
        } //foreach
        return idList;
        
    } //CheckScholarship



    public List<string> CombineAllLetters() {
        //Function that runs through all admission folders and checks corresponding scholarship folders
        var admissionDirectories = Directory.GetDirectories(ADMISSION_PATH);
        List<string> idList = new List<string>();
        //List of ids for later
        foreach(string folder in admissionDirectories){
            string[] folderArray = folder.Split(@"\");
            string folderName = folderArray[folderArray.Length - 1];
            int files = Directory.GetFiles(folder).Count();
            if(files > 0)
            {
                List<string> temp = CheckScholarships(folderName, Directory.GetFiles(folder));
                foreach(string id in temp){
                    idList.Add(id);
                } //foreach
                
            } //if
            Directory.Move(folder, $"{ARCHIVE_PATH}/Admission/{folderName}");
            var scholarshipFolder = $"{SCHOLARSHIP_PATH}/{folderName}";
            if(Directory.Exists(scholarshipFolder)){
                //Check to see if scholarship exists
                Directory.Move(scholarshipFolder,$"{ARCHIVE_PATH}/Scholarship/{folderName}");
            } //if
            
        } //foreach
        return idList;
    } //CombineAllLetters

    public static void Main(string[] args) {
        //Main function
        LetterService letterService = new LetterService();
        List<string> idList = letterService.CombineAllLetters();
        DateTime today = DateTime.Today;

        string report = $"{today.ToString("MM/dd/yyyy")} Report\n-----------------------------\n\n";
        report = report + $"Number of Combined Letters: {idList.Count}";
        //report = report + $"\n\t{idList}";
        foreach(string id in idList){
            //System.Console.WriteLine(id);
            report = report + $"\n\t{id}";
        } //foreach

        System.Console.WriteLine(report);

        File.WriteAllText($"{OUTPUT_PATH}/{today.ToString("MM-dd-yyyy")}-Report.txt", report);
    
    } //Main

} //LetterService