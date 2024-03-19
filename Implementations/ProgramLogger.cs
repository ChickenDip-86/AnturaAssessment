using Serilog;
using ILogger = Serilog.ILogger;

namespace AnturaAssessment.Classes;

public class ProgramLogger
{
    public static ProgramLogger Instance => instance;
    public ILogger Logger => logger;

    private static readonly ProgramLogger instance = new ProgramLogger();
    private readonly ILogger logger;

    private ProgramLogger()
    {
        logger = new LoggerConfiguration()
            .WriteTo.RollingFile("log-{Date}.txtRRRR")
            .CreateLogger();
        
    }

    public void LogInformation(string message)
    {
        Logger.Information(message);
    }

    public void LogInformation(string message, params object[] properties)
    {
        Logger.Information(message, properties);
    }


    public void OpenAppLogMessage()
    {
        ProgramLogger.Instance.LogInformation("                    ");
        ProgramLogger.Instance.LogInformation("--------------------");
        ProgramLogger.Instance.LogInformation("--------------------");
        ProgramLogger.Instance.LogInformation("--------------------");
        ProgramLogger.Instance.LogInformation("Starting Application");

    }

    public void CloseAppLogMessage()
    {
        ProgramLogger.Instance.LogInformation("Closing Application");
        ProgramLogger.Instance.LogInformation("--------------------");
        ProgramLogger.Instance.LogInformation("--------------------");
        ProgramLogger.Instance.LogInformation("--------------------");
        ProgramLogger.Instance.LogInformation("                    ");
    }










}





