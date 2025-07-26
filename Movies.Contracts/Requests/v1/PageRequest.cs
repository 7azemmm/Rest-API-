namespace Movies.Contracts.Responses;

public class PageRequest
{
   public required int Page { get; init; } = 1;
       
       public required int pageSize { get; init; } = 10; 


}