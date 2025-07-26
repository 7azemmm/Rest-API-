namespace RestApi;

public class ApiEndpoints
{
    private const string ApiBase = "api";

    public class v1
    {
        private const string VersionBase = $"{ApiBase}/v1";
        public class Movies
        {
            private const string MoviesBase = $"{VersionBase}/movies";
            public const string create = MoviesBase;
            public const string get = $"{MoviesBase}/{{idOrslug}}";
            public const string getall = MoviesBase;
            public const string update = $"{MoviesBase}/{{id:guid}}";
            public const string delete = $"{MoviesBase}/{{id:guid}}";
            public const string rate = $"{MoviesBase}/{{MovieId:guid}}/ratings";
            public const string deleteRating = $"{MoviesBase}/{{MovieId:guid}}/ratings";
        }

        public static class Rating
        {
            public const string Base = $"{VersionBase}/ratings";
            public const string getUserRatings = $"{Base}/me";
        
        }
        
    }
    
    public class v2
    {
        private const string VersionBase = $"{ApiBase}/v2";
        public class Movies
        {
            private const string MoviesBase = $"{VersionBase}/movies";
            public const string get = $"{MoviesBase}/{{idOrslug}}";
           
        }

     
        
    }
    
    
    
}