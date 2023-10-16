using System.Runtime.Serialization;

namespace Star_Wars_DataBase.Data.SWAPI;

[DataContract]
public class CategoriesDto
{
    [DataMember(Name = "people")] public string People = "";

    [DataMember(Name = "planets")] public string Planets = "";

    [DataMember(Name = "films")] public string Films = "";

    [DataMember(Name = "species")] public string Species = "";

    [DataMember(Name = "vehicles")] public string Vehicles = "";

    [DataMember(Name = "starships")] public string Starships = "";
}

[DataContract]
public class Urlable
{
    [DataMember(Name = "url")] public string Url { get; set; } = "";

    public string Id => SwapiUtils.ExtractIdFromLastPartOfUrl(Url);

    public string? Uri => SwapiUtils.MakeInternalLink(Url);
}

[DataContract]
public class BaseDto : Urlable
{
    [DataMember(Name = "created")] public string Created = "";

    public DateTime CreatedTime => Created.Length > 0 ? DateTime.Parse(Created) : DateTime.Now;

    [DataMember(Name = "edited")] public string Edited = "";

    public DateTime EditedTime => Edited.Length > 0 ? DateTime.Parse(Edited) : DateTime.Now;
}

[DataContract]
public class VehicleDto : BaseDto
{
    [DataMember(Name = "name")] public string Name = "";

    [DataMember(Name = "model")] public string Model = "";

    [DataMember(Name = "manufacturer")] public string Manufacturer = "";

    [DataMember(Name = "cost_in_credits")] public string CostInCredits = "";

    public string CostInCreditsFormatted
    {
        get
        {
            var success = long.TryParse(CostInCredits, out var number);
            return success ? number.ToString("N0") : CostInCredits;
        }
    }

    [DataMember(Name = "length")] public string Length = "";

    [DataMember(Name = "max_atmosphering_speed")]
    public string MaxAtmospheringSpeed = "";

    [DataMember(Name = "crew")] public string Crew = "";

    [DataMember(Name = "passengers")] public string Passengers = "";

    [DataMember(Name = "cargo_capacity")] public string CargoCapacity = "";

    [DataMember(Name = "consumables")] public string Consumables = "";

    [DataMember(Name = "vehicle_class")] public string VehicleClass = "";

    [DataMember(Name = "pilots")] public string[]? Pilots;
    public PersonDto[]? PilotsLoaded;

    [DataMember(Name = "films")] public string[]? Films;
    public FilmDto[]? FilmsLoaded;
}

[DataContract]
public class FilmDto : BaseDto
{
    [DataMember(Name = "title")] public string Title { get; set; } = "";

    [DataMember(Name = "episode_id")] public int EpisodeId { get; set; }

    [DataMember(Name = "opening_crawl")] public string OpeningCrawl { get; set; } = "";

    [DataMember(Name = "director")] public string Director { get; set; } = "";

    [DataMember(Name = "producer")] public string Producer { get; set; } = "";

    [DataMember(Name = "release_date")] public string ReleaseDate { get; set; } = "";

    public DateTime ReleaseDateTime => ReleaseDate.Length > 0 ? DateTime.Parse(ReleaseDate) : DateTime.Now;

    [DataMember(Name = "characters")] public string[]? Characters { get; set; }
    public PersonDto[]? CharactersLoaded;

    [DataMember(Name = "planets")] public string[]? Planets { get; set; }
    public PlanetDto[]? PlanetsLoaded;

    [DataMember(Name = "vehicles")] public string[]? Vehicles { get; set; }
    public VehicleDto[]? VehiclesLoaded;

    [DataMember(Name = "species")] public string[]? Species { get; set; }
    public SpecDto[]? SpeciesLoaded;

    [DataMember(Name = "starships")] public string[]? Starships { get; set; }
    public StarshipDto[]? StarshipsLoaded;
}

[DataContract]
public class PlanetDto : BaseDto
{
    [DataMember(Name = "name")] public string Name { get; set; } = "";

    [DataMember(Name = "rotation_period")] public string RotationPeriod { get; set; } = "";

    [DataMember(Name = "orbital_period")] public string OrbitalPeriod { get; set; } = "";

    [DataMember(Name = "diameter")] public string Diameter { get; set; } = "";

    [DataMember(Name = "climate")] public string Climate { get; set; } = "";

    [DataMember(Name = "gravity")] public string Gravity { get; set; } = "";

    [DataMember(Name = "terrain")] public string Terrain { get; set; } = "";

    [DataMember(Name = "surface_water")] public string SurfaceWater { get; set; } = "";

    [DataMember(Name = "population")] public string Population { get; set; } = "";

    [DataMember(Name = "residents")] public string[]? Residents { get; set; }
    public PersonDto[]? ResidentsLoaded;

    [DataMember(Name = "films")] public string[]? Films { get; set; }
    public FilmDto[]? FilmsLoaded;
}

[DataContract]
public class PersonDto
    : BaseDto
{
    [DataMember(Name = "name")] public string Name { get; set; } = "";

    [DataMember(Name = "height")] public string Height { get; set; } = "";

    [DataMember(Name = "mass")] public string Mass { get; set; } = "";

    [DataMember(Name = "hair_color")] public string HairColor { get; set; } = "";

    [DataMember(Name = "skin_color")] public string SkinColor { get; set; } = "";

    [DataMember(Name = "eye_color")] public string EyeColor { get; set; } = "";

    [DataMember(Name = "birth_year")] public string BirthYear { get; set; } = "";

    [DataMember(Name = "gender")] public string Gender { get; set; } = "";

    [DataMember(Name = "homeworld")] public string Homeworld { get; set; } = "";
    public PlanetDto? HomeworldLoaded;

    [DataMember(Name = "films")] public string[]? Films { get; set; }
    public FilmDto[]? FilmsLoaded;

    [DataMember(Name = "species")] public string[]? Species { get; set; }
    public SpecDto[]? SpeciesLoaded;

    [DataMember(Name = "vehicles")] public string[]? Vehicles { get; set; }
    public VehicleDto[]? VehiclesLoaded;

    [DataMember(Name = "starships")] public string[]? Starships { get; set; }
    public StarshipDto[]? StarshipsLoaded;
}

[DataContract]
public class SpecDto : BaseDto
{
    [DataMember(Name = "name")] public string Name { get; set; } = "";

    [DataMember(Name = "classification")] public string Classification { get; set; } = "";

    [DataMember(Name = "designation")] public string Designation { get; set; } = "";

    [DataMember(Name = "average_height")] public string AverageHeight { get; set; } = "";

    [DataMember(Name = "skin_colors")] public string SkinColors { get; set; } = "";

    [DataMember(Name = "hair_colors")] public string HairColors { get; set; } = "";

    [DataMember(Name = "eye_colors")] public string EyeColors { get; set; } = "";

    [DataMember(Name = "average_lifespan")]
    public string AverageLifespan { get; set; } = "";

    [DataMember(Name = "homeworld")] public string? Homeworld { get; set; }
    public string? HomeworldUri => SwapiUtils.MakeInternalLink(Homeworld);
    public PlanetDto? HomeworldLoaded;

    [DataMember(Name = "language")] public string Language { get; set; } = "";

    [DataMember(Name = "people")] public string[]? People { get; set; }
    public PersonDto[]? PeopleLoaded;

    [DataMember(Name = "films")] public string[]? Films { get; set; }
    public FilmDto[]? FilmsLoaded;
}

[DataContract]
public class StarshipDto : BaseDto
{
    [DataMember(Name = "name")] public string Name { get; set; } = "";

    [DataMember(Name = "model")] public string Model { get; set; } = "";

    [DataMember(Name = "manufacturer")] public string Manufacturer { get; set; } = "";

    [DataMember(Name = "cost_in_credits")] public string CostInCredits { get; set; } = "";

    [DataMember(Name = "length")] public string Length { get; set; } = "";

    [DataMember(Name = "max_atmosphering_speed")]
    public string MaxAtmospheringSpeed { get; set; } = "";

    [DataMember(Name = "crew")] public string Crew { get; set; } = "";

    [DataMember(Name = "passengers")] public string Passengers { get; set; } = "";

    [DataMember(Name = "cargo_capacity")] public string CargoCapacity { get; set; } = "";

    [DataMember(Name = "consumables")] public string Consumables { get; set; } = "";

    [DataMember(Name = "hyperdrive_rating")]
    public string HyperdriveRating { get; set; } = "";

    [DataMember(Name = "MGLT")] public string Mglt { get; set; } = "";

    [DataMember(Name = "starship_class")] public string StarshipClass { get; set; } = "";

    [DataMember(Name = "pilots")] public string[]? Pilots { get; set; }
    public PersonDto[]? PilotsLoaded { get; set; }

    [DataMember(Name = "films")] public string[]? Films { get; set; }
    public FilmDto[]? FilmsLoaded { get; set; }
}

[DataContract]
public class PageResult<T> where T : class
{
    [DataMember(Name = "count")] public int Count;

    [DataMember(Name = "next")] public string? Next;

    [DataMember(Name = "previous")] public string? Previous;

    [DataMember(Name = "results")] public T[]? Results;
}