namespace RestApi;

public class ApiEndpoints
{
    private const string ApiBase = "api";

    public class Movies
    {
        private const string MoviesBase = $"{ApiBase}/movies";
        public const string create = MoviesBase;
        public const string get = $"{MoviesBase}/{{idOrslug}}";
        public const string getall = MoviesBase;
        public const string update = $"{MoviesBase}/{{id:guid}}";
        public const string delete = $"{MoviesBase}/{{id:guid}}";
        public const string rate = $"{MoviesBase}/{{id:guid}}/ratings";
        public const string deleteRating = $"{MoviesBase}/{{id:guid}}/ratings";
    }

    public static class Rating
    {
        public const string Base = $"{ApiBase}/ratings";
        public const string getUserRatings = $"{Base}/me";
        
    }
    
    
}