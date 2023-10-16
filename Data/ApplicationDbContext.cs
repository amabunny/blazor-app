using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Star_Wars_DataBase.Data.PhoneBook;

namespace Star_Wars_DataBase.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public required DbSet<Contact> Contacts { get; set; }
}