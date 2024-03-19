
namespace AnturaAssessment.Interfaces;

public interface IFileHandler
{
    public bool Exists(string path);
    public FileStream Open(string path, FileMode mode);
    public void Close(FileStream stream);
}
