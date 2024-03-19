using AnturaAssessment.Interfaces;

namespace AnturaAssessment.Classes;

public class FileHandler : IFileHandler
{
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public FileStream Open(string path, FileMode mode)
    {
        return File.Open(path, mode);
    }

    public void Close(FileStream stream)
    {
        stream.Close();
    }

}
