namespace AppServer.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Mail { get; set; }
    public string Country { get; set; }
}
