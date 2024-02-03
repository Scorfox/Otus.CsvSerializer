namespace CsvSerializer.Services;

public interface ICsvSerializer
{
    string Serialize<T>(T obj);
    T Deserialize<T>(string input);
}