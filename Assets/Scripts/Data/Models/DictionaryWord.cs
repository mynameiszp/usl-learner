public class DictionaryWord : BaseModel
{
    public int DictionaryId { get; set; }
    public string Name { get; set; }
    public string TranslationSign { get; set; }

    public DictionaryWord()
    {
    }

    public override string ToString()
    {
        return $"Dictionary Word: id = {Id}, name = {Name}, dictionaryId = {DictionaryId}, sign = {TranslationSign}";
    }
}
