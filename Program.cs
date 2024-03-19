using AnturaAssessment.Classes;
using AnturaAssessment.Interfaces;
using Serilog;
using Serilog.Enrichers;
using Serilog.Formatting.Json;

public class Program
{
    private readonly IDocumentReader documentReader;
    private readonly IMessages messages;

    public Program(IDocumentReader documentReader, IMessages messages)
    {
        this.documentReader = documentReader;
        this.messages = messages;
    }

    public static void Main()
    {
        Log.Logger = new LoggerConfiguration()
           .WriteTo.File(new JsonFormatter(), "log.json")
           .Enrich.With<EnvironmentUserNameEnricher>()
           .CreateLogger();

        IConsoleWrapper console = new ConsoleWrapper();
        IFileHandler fileHandler = new FileHandler();
        IDocumentReader documentReader = new DocumentReader(console, fileHandler);
        IMessages messages = new Messages();

        Program program = new Program(documentReader, messages);
        program.Run();
    }

    public void Run()
    {
        Log.Logger.Information("Method {MethodName}: is starting the application!", nameof(Run));

        messages.Greeting();
        documentReader.FileNameInDocumentCount();
        bool runAgain = messages.PromptAgain();

        while (runAgain)
        {
            documentReader.FileNameInDocumentCount();
            runAgain = messages.PromptAgain();
        }

        Log.Logger.Information("Method {MethodName}: is closing the application!", nameof(Run));
    }
}





