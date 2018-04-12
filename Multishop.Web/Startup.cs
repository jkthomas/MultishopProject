using Microsoft.Owin;
using Owin;
using Multishop.Data.DAL.Context;
using Multishop.Entities.Accounts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartupAttribute(typeof(Multishop.Web.Startup))]
namespace Multishop.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }

        //TODO: Verify new AppDbContext usage in createRoles() method
        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            { 
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }
  
            if (!roleManager.RoleExists("Guest"))
            {
                var role = new IdentityRole
                {
                    Name = "Guest"
                };
                roleManager.Create(role);
            }
        }
    }
}
