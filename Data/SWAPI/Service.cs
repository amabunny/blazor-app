using Star_Wars_DataBase.Data.Utility;

namespace Star_Wars_DataBase.Data.SWAPI;

public class SwapiService
{
    private HttpClientCache _client;
    
    public SwapiService(IHttpClientFactory clientFactory)
    {
        var client = clientFactory.CreateClient("SWAPI");
        _client = new HttpClientCache(client, "swapi");
    }

    #region Categories

    public async Task<CategoriesDto> GetCategoriesAsync()
    {
        return await _client.GetFromCacheOrRequest<CategoriesDto>("/api");
    }

    #endregion

    #region Vehicles

    public async Task<PageResult<VehicleDto>> GetVehiclesAsync(int page = 1)
    {
        var query = page > 1 ? $"?page={page}" : "";
        return await _client.GetFromCacheOrRequest<PageResult<VehicleDto>>($"/api/vehicles/{query}");
    }

    public async Task<VehicleDto> GetVehicleAsync(string id, bool lazy = false)
    {
        var vehicle = await _client.GetFromCacheOrRequest<VehicleDto>($"/api/vehicles/{id}/");

        if (lazy) return vehicle;

        vehicle.FilmsLoaded = await SwapiUtils.AsyncGetter(GetFilmAsync, vehicle.Films);
        vehicle.PilotsLoaded = await SwapiUtils.AsyncGetter(GetPersonAsync, vehicle.Pilots);

        return vehicle;
    }

    #endregion

    #region Films

    public async Task<FilmDto> GetFilmAsync(string id, bool lazy = true)
    {
        var film = await _client.GetFromCacheOrRequest<FilmDto>($"/api/films/{id}");

        if (lazy) return film;

        film.PlanetsLoaded = await SwapiUtils.AsyncGetter(GetPlanetAsync, film.Planets);
        film.VehiclesLoaded = await SwapiUtils.AsyncGetter(GetVehicleAsync, film.Vehicles);
        film.CharactersLoaded = await SwapiUtils.AsyncGetter(GetPersonAsync, film.Characters);
        film.SpeciesLoaded = await SwapiUtils.AsyncGetter(GetSpecAsync, film.Species);
        film.StarshipsLoaded = await SwapiUtils.AsyncGetter(GetStarshipAsync, film.Starships);

        return film;
    }

    public async Task<PageResult<FilmDto>> GetFilmsAsync(int page = 1)
    {
        var query = page > 1 ? $"?page={page}" : "";
        return await _client.GetFromCacheOrRequest<PageResult<FilmDto>>($"/api/films/{query}");
    }

    #endregion

    #region Planets

    public async Task<PlanetDto> GetPlanetAsync(string id, bool lazy = true)
    {
        var planet = await _client.GetFromCacheOrRequest<PlanetDto>($"/api/planets/{id}");

        if (lazy) return planet;

        planet.FilmsLoaded = await SwapiUtils.AsyncGetter(GetFilmAsync, planet.Films);
        planet.ResidentsLoaded = await SwapiUtils.AsyncGetter(GetPersonAsync, planet.Residents);

        return planet;
    }

    public async Task<PageResult<PlanetDto>> GetPlanetsAsync(int page = 1)
    {
        var query = page > 1 ? $"?page={page}" : "";
        return await _client.GetFromCacheOrRequest<PageResult<PlanetDto>>($"/api/planets/{query}");
    }

    #endregion

    #region People

    public async Task<PageResult<PersonDto>> GetPeopleAsync(int page = 1)
    {
        var query = page > 1 ? $"?page={page}" : "";
        return await _client.GetFromCacheOrRequest<PageResult<PersonDto>>($"/api/people/{query}");
    }

    public async Task<PersonDto> GetPersonAsync(string id, bool lazy = true)
    {
        var person = await _client.GetFromCacheOrRequest<PersonDto>($"/api/people/{id}");

        if (lazy) return person;

        person.FilmsLoaded = await SwapiUtils.AsyncGetter(GetFilmAsync, person.Films);
        person.SpeciesLoaded = await SwapiUtils.AsyncGetter(GetSpecAsync, person.Species);
        person.StarshipsLoaded = await SwapiUtils.AsyncGetter(GetStarshipAsync, person.Starships);
        person.VehiclesLoaded = await SwapiUtils.AsyncGetter(GetVehicleAsync, person.Vehicles);
        person.HomeworldLoaded = await GetPlanetAsync(SwapiUtils.ExtractIdFromLastPartOfUrl(person.Homeworld));

        return person;
    }

    #endregion

    #region Species

    public async Task<PageResult<SpecDto>> GetSpeciesAsync(int page = 1)
    {
        var query = page > 1 ? $"?page={page}" : "";
        return await _client.GetFromCacheOrRequest<PageResult<SpecDto>>($"/api/species/{query}");
    }

    public async Task<SpecDto> GetSpecAsync(string id, bool lazy = true)
    {
        var spec = await _client.GetFromCacheOrRequest<SpecDto>($"/api/species/{id}");

        if (lazy) return spec;

        spec.FilmsLoaded = await SwapiUtils.AsyncGetter(GetFilmAsync, spec.Films);
        spec.PeopleLoaded = await SwapiUtils.AsyncGetter(GetPersonAsync, spec.People);

        if (spec.Homeworld != null)
        {
            spec.HomeworldLoaded = await GetPlanetAsync(SwapiUtils.ExtractIdFromLastPartOfUrl(spec.Homeworld));
        }

        return spec;
    }

    #endregion

    #region Starships

    public async Task<StarshipDto> GetStarshipAsync(string id, bool lazy = true)
    {
        var starship = await _client.GetFromCacheOrRequest<StarshipDto>($"/api/starships/{id}");

        if (lazy) return starship;

        starship.PilotsLoaded = await SwapiUtils.AsyncGetter(GetPersonAsync, starship.Pilots);
        starship.FilmsLoaded = await SwapiUtils.AsyncGetter(GetFilmAsync, starship.Films);

        return starship;
    }

    public async Task<PageResult<StarshipDto>> GetStarshipsAsync(int page = 1)
    {
        var query = page > 1 ? $"?page={page}" : "";
        return await _client.GetFromCacheOrRequest<PageResult<StarshipDto>>($"/api/starships/{query}");
    }

    #endregion
}