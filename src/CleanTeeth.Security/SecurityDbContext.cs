using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Models;

namespace Security;

public class SecurityDbContext : IdentityDbContext<User>
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
    {
    }

    protected SecurityDbContext()
    {
        
    }
}