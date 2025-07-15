using System.Security.Claims;

namespace RestApi;

public static class  IdentityExtensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var UserId = context.User.Claims.SingleOrDefault( x => x.Type == "userid" );
        
        if(Guid.TryParse(UserId?.Value , out var parseID))
        {
            return parseID;
        }
        return null;
    }
}