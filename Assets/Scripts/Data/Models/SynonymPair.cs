public class SynonymPair : BaseModel
{
    public int Word_id;
    public int Synonym_id;

    public SynonymPair()
    {
    }

   public override string ToString()
    {
        return $"Synonym pair: id = {Id}, word_id = {Word_id}, synonym_id = {Synonym_id}";
    }
}
