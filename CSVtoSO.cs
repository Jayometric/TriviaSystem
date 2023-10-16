/* This script is for importing CSV files and creating scriptable objects
 / The format will depend on the columns from the CSV file, the format I used is on line 27
 / THIS FILE MUST BE IN THE "Editor" folder you created in the assets folder. 
 / This will output data to Resources/Questions folder
*/

using UnityEngine;
using UnityEditor;
using System.IO;

public class CVStoSO
{
    private static string questionsCSVPath = "/Editor/CSVs/Questions.csv";
    private static string questionsPath = "Assets/Resources/Questions/";
    private static int numberOfAnswers = 4;

    [MenuItem("Utilities/Generate Questions")]
    public static void GeneratePhrases()
    {

        string[] allLines = File.ReadAllLines(Application.dataPath + questionsCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            // CSV (COMMA SEPARATED VALUE) DATA FORMAT
            // QUESTION, CATEGORY, CORRECT ANSWER, WRONG ANSWER 1, WRONG ANSWER 2, WRONG ANSWER 3

            QuestionData questionData = ScriptableObject.CreateInstance<QuestionData>();
            questionData.question = splitData[0]; // 1st column from csv file
            questionData.category = splitData[1];

            // Initialize the array of answers
            questionData.answers = new string[4];

            // Check if the folder for generating questions does not exist
            if (!Directory.Exists(questionsPath))
            {
                // Create the directory as one does not exist (creates a folder)
                Directory.CreateDirectory(questionsPath);
            }

            for (int i = 0; i < numberOfAnswers; i++)
            {
                questionData.answers[i] = splitData[2 + i];
            }

            // CREATE THE FILE NAME
            // Remove the "?", file name cannot have that character
            if (questionData.question.Contains("?"))
            {
                // Questions will be named the same as the question text in this example
                questionData.name = questionData.question.Remove(questionData.question.IndexOf("?"));
            }
            else // Does not contain an invalid character, no changes required
            {
                questionData.name = questionData.question;
            }
            // Save this in the questionsPathfolder to load them later by script
            AssetDatabase.CreateAsset(questionData, $"{questionsPath}/{questionData.name}.asset");
        }

        AssetDatabase.SaveAssets();

        Debug.Log($"Generated Questions");
    }
}
