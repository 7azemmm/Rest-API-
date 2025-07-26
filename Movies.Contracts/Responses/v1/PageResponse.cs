namespace Movies.Contracts.Responses;

public class PageResponse<TResponse>
{
    public required IEnumerable<TResponse> Items { get; init; } = Enumerable.Empty<TResponse>();
    public required int Page { get; init; } 
       
    public required int pageSize { get; init; }  
    public required int total { get; init; }
    
    public bool HasNextPage => total > (Page * pageSize);
}