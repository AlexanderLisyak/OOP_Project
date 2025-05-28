namespace FinanceApp
{
    public interface IJsonSerializable
    {
        void SaveToJson(string path);
        void LoadFromJson(string path);
    }
}