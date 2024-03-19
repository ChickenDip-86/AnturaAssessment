using System.Reflection;
using System.Text.RegularExpressions;
using AnturaAssessment.Interfaces;
using Serilog;
using ILogger = Serilog.ILogger;


namespace AnturaAssessment.Classes;

public class DocumentReader : IDocumentReader
{
    private readonly IConsoleWrapper console;
    private readonly IFileHandler fileHandler;

    private FileStream? fileStream;
    private StreamReader? streamReader;

    private int maxAttempts = 20;
    private int attempts = 0;

    #region Initialize
    public DocumentReader(IConsoleWrapper console, IFileHandler fileHandler) 
    {
        this.console = console;
        this.fileHandler = fileHandler;
    }
    #endregion

    #region Public Methods
    public void FileNameInDocumentCount()
    {
        attempts = 0;
        string userInput = CaptureInputToPath();

        if (attempts >= maxAttempts)
        {
            Log.Logger.Information("Method {MethodName}: Exceeded maximum attempts limit ({MaxAttempts}), user tried entering paths {Attempts} amount of times",
                nameof(CaptureInputToPath), maxAttempts, attempts);

            console.WriteLine("Maximum attempts reached. Please try again later or contact support for assistance.");
            console.WriteLine("Relaunch program to renew your attempts.");
            Environment.Exit(0);
        }

        OpenFile(userInput);

        if (fileStream == null || streamReader == null)
        {
            console.WriteLine("Failed to open file.");
            return;
        }

        try
        {
            string fileNameWithOutExtension = ExtractFileNameWithoutExtension(userInput);
            int fileNameFrequency = CountHowManyTimesFileNameAppearsInsideFile(fileNameWithOutExtension);

            if (fileNameFrequency <= 0)
            {
                console.WriteLine($"The name of the file was not present in the document");
                return;
            }

            string frequencyMessage = (fileNameFrequency > 1) ? "times" : "time";
            console.WriteLine($"The name of the file has appeared {fileNameFrequency} {frequencyMessage} in the document");
        }
        finally
        {
            CloseFile();
        }
    }

    public void CloseFile()
    {
        if (fileStream != null)
        {
            fileHandler.Close(fileStream);
        }
        if (streamReader != null)
        {
            streamReader.Close();
        }
        streamReader = null;
        fileStream = null;
    }
    #endregion

    #region Private Methods
    private string CaptureInputToPath()
    {
        string input = null;
        bool validInput = false;

        while (attempts < maxAttempts && !validInput)
        {
            console.WriteLine("Input the path to a file, don't forget the file extension (e.g., .txt)");
            console.Write("Enter Path: ");
            input = console.ReadLine()?.Trim('"');

            if (string.IsNullOrWhiteSpace(input))
            {
                attempts++;
                console.WriteLine();
                console.WriteLine("Please input a valid path to a file");
            }
            else if (input.Length > 260) 
            {
                attempts++;
                console.WriteLine();
                console.WriteLine("Input path exceeds maximum length (260 characters)");
            }
            else if (!input.Contains('.'))
            {
                attempts++;
                console.WriteLine();
                console.WriteLine("The input must contain a file extension");
            }
            else
            {
                validInput = true;
            }

            Log.Logger.Information("Method {MethodName}: User trying: {Input} as input string. Login attempt nr {Attempts}",
               nameof(CaptureInputToPath), input, attempts);
        }

        Log.Logger.Information("Method {MethodName}: Input string accepted on attempt {Attempts} and exiting loop. String is: {Input} ",
            nameof(CaptureInputToPath), input, attempts);

        return input;
    }

    private void OpenFile(string filePath)
    {
        Log.Logger.Information("Method {MethodName}: Entering method with string '{FilePath}' as path.", nameof(OpenFile), filePath);

        try
        {
            if (!fileHandler.Exists(filePath))
            {
                attempts++;
                console.WriteLine($"No file found in path: '{filePath}', please try again!");
                Log.Logger.Information("Method {MethodName}: No file found in path: '{FilePath}'", nameof(OpenFile), filePath);
                return;
            }

            fileStream = fileHandler.Open(filePath, FileMode.Open);
            streamReader = new StreamReader(fileStream);
            Log.Logger.Information("Method {MethodName}: Successfully opened file in path: '{FilePath}'", nameof(OpenFile), filePath);
        }
        
        catch (UnauthorizedAccessException ex)
        {
            console.WriteLine($"Access to the file is unauthorized: {ex.Message}");
            Log.Logger.Information("Method {MethodName}: Error opening file at path: '{FilePath}': Unauthorized access", nameof(OpenFile), filePath);
        }
        catch (Exception ex)
        {
            console.WriteLine($"Error opening file: {ex.Message}");
            Log.Logger.Information("Method {MethodName}:Error opening file in path: '{FilePath}': {Ex.Message}", nameof(OpenFile), filePath, ex.Message);
        }
    }


    private string ExtractFileNameWithoutExtension(string fullFileName)
    {
        if (string.IsNullOrEmpty(fullFileName))
        {
            Log.Logger.Information("Method {MethodName}: Input string is NullOrEmpty", nameof(ExtractFileNameWithoutExtension));
            return string.Empty;
        }

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullFileName);
        Log.Logger.Information("Method {MethodName}: Returning filename without extension: '{FileNameWithoutExtension}'", nameof(ExtractFileNameWithoutExtension), fileNameWithoutExtension);
        return fileNameWithoutExtension;
    }

    private int CountHowManyTimesFileNameAppearsInsideFile(string fileNameWithoutExtension)
    {
        try
        {
            if (streamReader == null)
            {
                Log.Logger.Information("Method {MethodName}: file is null, method needs a file to operate on.", nameof(CountHowManyTimesFileNameAppearsInsideFile));
                throw new InvalidOperationException("File reader is not initialized. Ensure that the file is opened before calling this method.");
            }

            string fileContent = streamReader.ReadToEnd();
            int fileNameCounter = Regex.Matches(fileContent, $@"\b{Regex.Escape(fileNameWithoutExtension)}\b", RegexOptions.IgnoreCase).Count;
            Log.Logger.Information("Method {MethodName}: fileContent is {FileContent}, amount of regex matches for filename in document is: {FileNameCounter}", nameof(CountHowManyTimesFileNameAppearsInsideFile), fileContent, fileNameCounter);
            return fileNameCounter;
        }
        catch (Exception ex)
        {
            console.WriteLine($"Error reading file: {ex.Message}");
            Log.Logger.Information("Method {MethodName}: Error reading file: {Ex.Message}", nameof(CountHowManyTimesFileNameAppearsInsideFile), ex.Message);
            throw new IOException($"Error reading file in method {nameof(CountHowManyTimesFileNameAppearsInsideFile)}: {ex.Message}", ex);
        }
    }
    #endregion

}












