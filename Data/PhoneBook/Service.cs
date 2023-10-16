using Microsoft.EntityFrameworkCore;

namespace Star_Wars_DataBase.Data.PhoneBook;

public class PhoneBookService
{
    private readonly ApplicationDbContext _dbContext;

    public PhoneBookService(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbContext = dbFactory.CreateDbContext();
    }

    public Task<List<Contact>> GetContacts()
    {
        return _dbContext.Contacts.ToListAsync();
    }

    public async Task<Contact> CreateContact(Contact contact)
    {
        contact.CreatedTimestamp = DateTime.Now;
        var result = await _dbContext.Contacts.AddAsync(contact);

        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Contact> UpdateContact(Contact contact)
    {
        contact.CreatedTimestamp = DateTime.Now;
        var result = _dbContext.Contacts.Update(contact);

        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Contact> DeleteContact(Contact contact)
    {
        var result = _dbContext.Contacts.Remove(contact);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }
}