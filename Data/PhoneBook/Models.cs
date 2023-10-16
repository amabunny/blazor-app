namespace Star_Wars_DataBase.Data.PhoneBook;

public class Contact
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = "";

    public string FullName { get; set; } = "";

    public string Address { get; set; } = "";

    public string? Telegram { get; set; }

    public string? Company { get; set; }

    public DateTime CreatedTimestamp { get; set; }
}