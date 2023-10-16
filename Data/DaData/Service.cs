namespace Star_Wars_DataBase.Data.DaData.Service;
using Dadata;
using Dadata.Model;

public class DaDataService
{
    private readonly SuggestClientAsync _suggestClient;
    
    public DaDataService(ConfigurationManager configurationManager)
    {
        var apiKey = configurationManager
            .GetRequiredSection("DaData")
            .GetValue<string>("ApiKey") ?? throw new ArgumentException();
        
        _suggestClient = new SuggestClientAsync(apiKey);        
    }

    public async Task<IList<Suggestion<Address>>> GetAddressSuggestions(string addressText)
    {
        return (await _suggestClient.SuggestAddress(addressText)).suggestions;
    }

    public async Task<IList<Suggestion<Fullname>>> GetPersonInitialsSuggestions(string initialsText)
    {
        return (await _suggestClient.SuggestName(initialsText)).suggestions;
    }

    public async Task<IList<Suggestion<Party>>> GetCompanySuggestions(string companyText)
    {
        return (await _suggestClient.SuggestParty(companyText)).suggestions;
    }
}
