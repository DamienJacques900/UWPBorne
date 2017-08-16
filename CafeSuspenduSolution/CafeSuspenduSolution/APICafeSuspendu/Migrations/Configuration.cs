namespace APICafeSuspendu.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //Utilisation des tests unitaires pour remplir la database, d'où la méthode FillDataBase ci-dessous
        }

        public void FillDataBase(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var store = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(store);
            var adminRole = new IdentityRole { Name = "admin" };
            var userPersonRole = new IdentityRole { Name = "userperson" };
            var userCafeRole = new IdentityRole { Name = "usercafe" };

            roleManager.Create(adminRole);
            roleManager.Create(userPersonRole);
            roleManager.Create(userCafeRole);

            ApplicationUser damien = new ApplicationUser()
            {
                UserName = "Damien",
                FirstName = "Damien",
                LastName = "Jacques",
                Email = "damien@gmail.com",
                PhoneNumber = "071717171",

                // Charities = new List<Charity>()
            };

            ApplicationUser antoni = new ApplicationUser()
            {
                UserName = "Antoni",
                FirstName = "Antoni",
                LastName = "ManiscalcoA",
                Email = "antoni@gmail.com",
                PhoneNumber = "071717171",

                // Charities = new List<Charity>(),
            };

            ApplicationUser green = new ApplicationUser()
            {
                UserName = "GreenFairy",
                CafeName = "Green Fairy",
                Street = "Rue godefroid 5000 Namur",
                Number = "2",
                NbCoffeeRequiredForPromotion = 20,
                PromotionValue = 2.5,
                Email = "green@namur.be",

                Bookings = new List<Booking>(),
                TimeTables = new List<TimeTable>()
            };

            ApplicationUser caves = new ApplicationUser()
            {
                UserName = "Caves",
                CafeName = "Les caves",
                Street = "Rue godefroid 5000 Namur",
                Number = "5",
                NbCoffeeRequiredForPromotion = 10,
                PromotionValue = 2.5,
                Email = "caves@namur.be",

                Bookings = new List<Booking>(),
                TimeTables = new List<TimeTable>()
            };

            ApplicationUser starbucks = new ApplicationUser()
            {
                UserName = "Starbucks",
                CafeName = "Starbucks",
                Street = "Rue de fer 5000 Namur",
                Number = "50",
                NbCoffeeRequiredForPromotion = 10,
                PromotionValue = 2.5,
                Email = "starbucks@namur.be",

                Bookings = new List<Booking>(),
                TimeTables = new List<TimeTable>()
            };

            var userCreationResult = userManager.Create(damien, "DamienMDP1");

            Debug.Assert(userCreationResult.Succeeded);
            var addToRoleResult = userManager.AddToRole(
                 damien.Id, "userperson");
            Debug.Assert(addToRoleResult.Succeeded);
            userManager.Create(antoni, "AntoniMDP1");
            userManager.AddToRole(antoni.Id, "userperson");

            userManager.Create(green, "GreenMDP1");
            userManager.AddToRole(green.Id, "usercafe");
            userManager.Create(caves, "CavesMDP1");
            userManager.AddToRole(caves.Id, "usercafe");
            userManager.Create(starbucks, "StarbucksMDP1");
            userManager.AddToRole(starbucks.Id, "usercafe");

            TimeTable lundiGreen = new TimeTable()
            {
                OpeningHour = new TimeSpan(1, 0, 0),
                ClosingHour = new TimeSpan(11, 0, 0),
                DayOfWeek = 1,
                ApplicationUser = green
            };

            TimeTable lundiCaves = new TimeTable()
            {
                OpeningHour = new TimeSpan(1, 0, 0),
                ClosingHour = new TimeSpan(11, 0, 0),
                DayOfWeek = 1,
                ApplicationUser = caves
            };

            TimeTable lundiStarbucks = new TimeTable()
            {
                OpeningHour = new TimeSpan(1, 0, 0),
                ClosingHour = new TimeSpan(11, 0, 0),
                DayOfWeek = 1,
                ApplicationUser = starbucks
            };

            TimeTable mardiGreen = new TimeTable()
            {
                OpeningHour = new TimeSpan(2, 0, 0),
                ClosingHour = new TimeSpan(12, 0, 0),
                DayOfWeek = 2,
                ApplicationUser = green
            };

            TimeTable mardiCaves = new TimeTable()
            {
                OpeningHour = new TimeSpan(2, 0, 0),
                ClosingHour = new TimeSpan(12, 0, 0),
                DayOfWeek = 2,
                ApplicationUser = caves
            };

            TimeTable mardiStarbucks = new TimeTable()
            {
                OpeningHour = new TimeSpan(2, 0, 0),
                ClosingHour = new TimeSpan(12, 0, 0),
                DayOfWeek = 2,
                ApplicationUser = starbucks
            };

            TimeTable mercrediGreen = new TimeTable()
            {
                OpeningHour = new TimeSpan(3, 0, 0),
                ClosingHour = new TimeSpan(13, 0, 0),
                DayOfWeek = 3,
                ApplicationUser = green
            };

            TimeTable mercrediCaves = new TimeTable()
            {
                OpeningHour = new TimeSpan(3, 0, 0),
                ClosingHour = new TimeSpan(13, 0, 0),
                DayOfWeek = 3,
                ApplicationUser = caves
            };

            TimeTable mercrediStarbucks = new TimeTable()
            {
                OpeningHour = new TimeSpan(3, 0, 0),
                ClosingHour = new TimeSpan(13, 0, 0),
                DayOfWeek = 3,
                ApplicationUser = starbucks
            };

            TimeTable jeudiGreen = new TimeTable()
            {
                OpeningHour = new TimeSpan(4, 0, 0),
                ClosingHour = new TimeSpan(14, 0, 0),
                DayOfWeek = 4,
                ApplicationUser = green
            };

            TimeTable jeudiCaves = new TimeTable()
            {
                OpeningHour = new TimeSpan(4, 0, 0),
                ClosingHour = new TimeSpan(14, 0, 0),
                DayOfWeek = 4,
                ApplicationUser = caves
            };

            TimeTable jeudiStarbucks = new TimeTable()
            {
                OpeningHour = new TimeSpan(4, 0, 0),
                ClosingHour = new TimeSpan(14, 0, 0),
                DayOfWeek = 4,
                ApplicationUser = starbucks
            };

            TimeTable vendrediGreen = new TimeTable()
            {
                OpeningHour = new TimeSpan(5, 0, 0),
                ClosingHour = new TimeSpan(15, 0, 0),
                DayOfWeek = 5,
                ApplicationUser = green
            };

            TimeTable vendrediCaves = new TimeTable()
            {
                OpeningHour = new TimeSpan(5, 0, 0),
                ClosingHour = new TimeSpan(15, 0, 0),
                DayOfWeek = 5,
                ApplicationUser = caves
            };

            TimeTable vendrediStarbucks = new TimeTable()
            {
                OpeningHour = new TimeSpan(5, 0, 0),
                ClosingHour = new TimeSpan(15, 0, 0),
                DayOfWeek = 5,
                ApplicationUser = starbucks
            };

            TimeTable samediGreen = new TimeTable()
            {
                OpeningHour = new TimeSpan(6, 0, 0),
                ClosingHour = new TimeSpan(16, 0, 0),
                DayOfWeek = 6,
                ApplicationUser = green
            };

            TimeTable samediCaves = new TimeTable()
            {
                OpeningHour = new TimeSpan(6, 0, 0),
                ClosingHour = new TimeSpan(16, 0, 0),
                DayOfWeek = 6,
                ApplicationUser = caves
            };

            TimeTable samediStarbucks = new TimeTable()
            {
                OpeningHour = new TimeSpan(6, 0, 0),
                ClosingHour = new TimeSpan(16, 0, 0),
                DayOfWeek = 6,
                ApplicationUser = starbucks
            };

            TimeTable dimancheGreen = new TimeTable()
            {
                OpeningHour = new TimeSpan(7, 0, 0),
                ClosingHour = new TimeSpan(17, 0, 0),
                DayOfWeek = 7,
                ApplicationUser = green
            };

            TimeTable dimancheCaves = new TimeTable()
            {
                OpeningHour = new TimeSpan(7, 0, 0),
                ClosingHour = new TimeSpan(17, 0, 0),
                DayOfWeek = 7,
                ApplicationUser = caves
            };

            TimeTable dimancheStarbucks = new TimeTable()
            {
                OpeningHour = new TimeSpan(7, 0, 0),
                ClosingHour = new TimeSpan(17, 0, 0),
                DayOfWeek = 7,
                ApplicationUser = starbucks
            };



            Charity charity1 = new Charity()
            {
                NbCoffeeOffered = 25,
                NbCoffeeConsumed = 0,
                OfferingTime = new DateTime(2016,12,13), //Date à modifier pour test Damien -> Test -> Windows -> Test explorer et tu en lances 1
                ApplicationUserPerson = antoni,
                ApplicationUserCoffee = green
            };
            Charity charity2 = new Charity()
            {
                NbCoffeeOffered = 2,
                NbCoffeeConsumed = 1,
                OfferingTime = new DateTime(),
                ApplicationUserPerson = damien,
                ApplicationUserCoffee = green
            };

            Charity charity3 = new Charity()
            {
                NbCoffeeOffered = 2,
                NbCoffeeConsumed = 2,
                OfferingTime = new DateTime(),
                ApplicationUserPerson = damien,
                ApplicationUserCoffee = caves
            };

            Charity charity4 = new Charity()
            {
                NbCoffeeOffered = 3,
                NbCoffeeConsumed = 1,
                OfferingTime = new DateTime(),
                ApplicationUserPerson = antoni,
                ApplicationUserCoffee = starbucks
            };

            Terminal terminal = new Terminal()
            {
                NbBookedCoffees = 1,
                Latitude = 85.2,
                Longitude = 42,
                Bookings = new List<Booking>()
            };
            Booking booking = new Booking()
            {
                DateBooking = new DateTime(2016,12,14),
                Name = "Sans abris 1",
                ApplicationUser = green,
                Terminal = terminal
            };

            Booking booking2 = new Booking()
            {
                DateBooking = new DateTime(2016, 12, 13),
                Name = "Sans abris 18",
                ApplicationUser = green,
                Terminal = terminal
            };

            green.TimeTables.Add(lundiGreen);
            caves.TimeTables.Add(lundiCaves);
            starbucks.TimeTables.Add(lundiStarbucks);
            green.TimeTables.Add(mardiGreen);
            caves.TimeTables.Add(mardiCaves);
            starbucks.TimeTables.Add(mardiStarbucks);
            green.TimeTables.Add(mercrediGreen);
            caves.TimeTables.Add(mercrediCaves);
            starbucks.TimeTables.Add(mercrediStarbucks);
            green.TimeTables.Add(jeudiGreen);
            caves.TimeTables.Add(jeudiCaves);
            starbucks.TimeTables.Add(jeudiStarbucks);
            green.TimeTables.Add(vendrediGreen);
            caves.TimeTables.Add(vendrediCaves);
            starbucks.TimeTables.Add(vendrediStarbucks);
            green.TimeTables.Add(samediGreen);
            caves.TimeTables.Add(samediCaves);
            starbucks.TimeTables.Add(samediStarbucks);
            green.TimeTables.Add(dimancheGreen);
            caves.TimeTables.Add(dimancheCaves);
            starbucks.TimeTables.Add(dimancheStarbucks);

            green.Bookings.Add(booking);
            green.Bookings.Add(booking2);
            terminal.Bookings.Add(booking);
            terminal.Bookings.Add(booking2);

            context.Charities.Add(charity1);
            context.Charities.Add(charity2);
            context.Charities.Add(charity3);
            context.Charities.Add(charity4);



            context.SaveChanges();
        }
    }
}
