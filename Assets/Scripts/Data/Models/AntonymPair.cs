public class AntonymPair : BaseModel
{
    public int Word_id;
    public int Antonym_id;

    public AntonymPair()
    {
    }

    public override string ToString()
    {
        return $"Antonym pair: id = {Id}, word_id = {Word_id}, antonym_id = {Antonym_id}";
    }
}
