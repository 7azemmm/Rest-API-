namespace RestApi;

public class ApiEndpoints
{
    private const string ApiBase = "api";

    public class Movies
    {
        private const string MoviesBase = $"{ApiBase}/movies";
        public const string create = MoviesBase;
        public const string get = $"{MoviesBase}/{{id:guid}}";
        public const string getall = MoviesBase;
    }
    
    
}