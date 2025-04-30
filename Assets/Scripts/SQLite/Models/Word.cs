public class Word : BaseModel
{
    public string Name { get; set; }

    public Word()
    {
    }

    public override string ToString()
    {
        return $"Word: id = {Id}, name = {Name}";
    }
}
