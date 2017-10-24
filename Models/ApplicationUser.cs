using Microsoft.AspNet.Identity.EntityFramework;
 
public class ApplicationUser : IdentityUser
{
    public string Role { get; set; }
    public ApplicationUser()
    {
    }
}