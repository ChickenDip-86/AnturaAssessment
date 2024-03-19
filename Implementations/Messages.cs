namespace AnturaAssessment.Classes;

public class Messages : IMessages
{
    public void Greeting()
    {
        Console.WriteLine("Welcome!");
        Console.WriteLine("Here you can count the number of times the filename appears in the file!");
    }

    public bool PromptAgain()
    {
        Console.WriteLine();
        Console.WriteLine("Do you want to check another file?");
        Console.WriteLine("Press Y/N");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Y)
            {
                Console.WriteLine();
                return true;
            }
            else if (key.Key == ConsoleKey.N)
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
                return false;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            }
        }
    }



}
