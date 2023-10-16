using Microsoft.AspNetCore.Mvc;
using Star_Wars_DataBase.Data.DaData.Service;
using Dadata.Model;

namespace Star_Wars_DataBase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DaDataController : ControllerBase
{
    private readonly DaDataService _daDataService; 
    
    public DaDataController(DaDataService service) : base()
    {
        _daDataService = service;
    }
    
    [HttpGet("suggest-address")]
    public async Task<ActionResult<IList<Suggestion<Address>>>> SuggestAddress([FromQuery] string query)
    {
        var result = await _daDataService.GetAddressSuggestions(query);
        return new ActionResult<IList<Suggestion<Address>>>(result);
    }

    [HttpGet("suggest-initials")]
    public async Task<ActionResult<IList<Suggestion<Fullname>>>> SuggestInitials([FromQuery] string query)
    {
        var result = await _daDataService.GetPersonInitialsSuggestions(query);
        return new ActionResult<IList<Suggestion<Fullname>>>(result);
    }

    [HttpGet("suggest-company")]
    public async Task<ActionResult<IList<Suggestion<Party>>>> SuggestCompany([FromQuery] string query)
    {
        var result = await _daDataService.GetCompanySuggestions(query);
        return new ActionResult<IList<Suggestion<Party>>>(result);
    }
}
