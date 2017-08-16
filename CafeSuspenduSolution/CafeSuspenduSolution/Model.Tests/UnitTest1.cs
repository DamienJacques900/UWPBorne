using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace APICafeSuspendu.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private static ApplicationDbContext GetContext()
        {
            return new ApplicationDbContext();
        }

        [TestInitialize]
        public void FillDataBase()
        {
            using (var context = GetContext())
            {
                Database.SetInitializer(new DbInitializer()); //DropCreateDatabaseAlways
                context.Database.Initialize(true); //force l'initialisation
                APICafeSuspendu.Migrations.Configuration config = new Migrations.Configuration();
                config.FillDataBase(context);
            }
        }

        [TestMethod]
        public void CanGetDamien()
        {
            using (var context = GetContext())
            {
                Assert.AreEqual("Damien", context.Users.Where(c => c.UserName == "Damien").Single().UserName);
            }
        }

        [TestMethod]
        public void RoleOfDamienIsDefined()
        {
            using (var context = GetContext())
            {
                Assert.AreEqual(1, context.Users.Where(c => c.UserName == "Damien").Single().Roles.Count());
            }
        }

        [TestMethod]
        public void CanGetTimeTables()
        {
            using (var context = GetContext())
            {
                Assert.IsTrue(context.TimeTables.Count() > 0);
            }
        }

        [TestMethod]
        public void GreenHasSeveralCharities()
        {
            using (var context = GetContext())
            {
               Assert.AreEqual(2,context.Charities.Where(c=>c.ApplicationUserCoffee.UserName=="GreenFairy").Count());
            }
        }
    }
}
