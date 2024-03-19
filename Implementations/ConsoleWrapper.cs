using AnturaAssessment.Interfaces;

namespace AnturaAssessment.Classes;

public class ConsoleWrapper : IConsoleWrapper
{
    public string ReadLine() => Console.ReadLine();

    public void Write(string value) => Console.Write(value);

    public void WriteLine(string value) => Console.WriteLine(value);

    public void WriteLine() => Console.WriteLine();
}
