using SQLite4Unity3d;

public class DictionaryWord : BaseModel
{
    public int DictionaryId { get; set; }
    public string Name { get; set; }
    public string TranslationSign { get; set; }
}
