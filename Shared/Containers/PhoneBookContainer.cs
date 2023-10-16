namespace Star_Wars_DataBase.Shared.Containers;

using Star_Wars_DataBase.Data.PhoneBook;

public class PhoneBookContainer
{
    public readonly BlazorState<List<Contact>> ContactList = new(new List<Contact>());
    private readonly PhoneBookService _phoneBookService;

    public PhoneBookContainer(PhoneBookService phoneBookService)
    {
        _phoneBookService = phoneBookService;
    }

    public async Task GetContacts()
    {
        ContactList.Change(await _phoneBookService.GetContacts());
    }

    public async Task DeleteContact(Contact contact)
    {
        await _phoneBookService.DeleteContact(contact);

        ContactList.Change(contacts =>
        {
            contacts.Remove(contact);
            return contacts;
        });
    }

    public async Task CreateOrUpdateContact(Contact contact, bool update)
    {
        if (update)
        {
            await _phoneBookService.UpdateContact(contact);
        }
        else
        {
            await _phoneBookService.CreateContact(contact);
        }

        ContactList.Change(contacts =>
        {
            if (update)
            {
                var updatedIndex = contacts.FindIndex(updatingContact => updatingContact.Id == contact.Id);
                contacts[updatedIndex] = contact;
            }
            else
            {
                contacts.Insert(0, contact);
            }

            return contacts;
        });
    }
}