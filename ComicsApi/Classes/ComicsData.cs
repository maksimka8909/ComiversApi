namespace ComicsApi.Classes;

public class ComicsData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int idEditor { get; set; }
    public int idAuthor { get; set; }
    public string dateOfIssue { get; set; }
}