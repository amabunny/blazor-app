namespace Star_Wars_DataBase.Data.SWAPI

{
    public abstract class SwapiUtils
    {
        public static string ExtractIdFromLastPartOfUrl(string url)
        {
            var splitUrl = url.Split('/');
            return splitUrl.ElementAtOrDefault(splitUrl.Length - 2) ?? "";
        }

        public static string? MakeInternalLink(string? url)
        {
            if (url == null)
            {
                return null;
            }

            var splitUrl = url
                .Split('/')
                .Skip(4);

            return "swapi/" + string.Join('/', splitUrl);
        }

        public static string[]? MakeInternalLinks(string[]? links)
        {
            return links?
                .ToList()
                .ConvertAll(l => MakeInternalLink(l) ?? "")
                .ToArray();
        }

        public static string FormatDate(string? stringDate)
        {
            return DateTime.Parse(stringDate ?? "").ToString("y");
        }

        public static string FormatNumber(string? inputString)
        {
            var success = long.TryParse(inputString, out long number);

            return success
                ? number.ToString("N0")
                : inputString ?? "";
        }

        public static async Task<T[]> AsyncGetter<T>(Func<string, bool, Task<T>> loader, string[]? urls)
        {
            var tasks = (urls ?? Array.Empty<string>())
                .ToList()
                .ConvertAll(ExtractIdFromLastPartOfUrl)
                .ConvertAll(url => loader(url, true))
                .ToArray();

            return await Task.WhenAll(tasks);
        }
    }
}