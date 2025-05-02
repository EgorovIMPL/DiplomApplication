namespace Contract;

public class CreateUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public long SteamId { get; set; }
}