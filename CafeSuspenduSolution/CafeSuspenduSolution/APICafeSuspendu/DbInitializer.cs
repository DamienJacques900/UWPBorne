using System;
using System.Collections.Generic;
using System.Data.Entity;
using APICafeSuspendu.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace APICafeSuspendu
{
    public class DbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
        }
    }
}
