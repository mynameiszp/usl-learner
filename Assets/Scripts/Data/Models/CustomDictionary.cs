public class CustomDictionary : BaseModel
{
    public string Name{ get; set; }

    public CustomDictionary(string name)
    {
        Name = name;
    }

    public CustomDictionary()
    {
    }

    public override string ToString()
    {
        return $"Custom Dictionary: id = {Id}, name = {Name}";
    }
}
