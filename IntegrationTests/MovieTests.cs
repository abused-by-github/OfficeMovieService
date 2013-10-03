using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svitla.MovieService.DataAccess;

namespace Svitla.MovieService.Tests.IntegrationTests
{
    [TestClass]
    public class MovieTests
    {
        [TestMethod]
        public void RecreateDBFromModel()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (var context =new DataContext(connectionString))
            {
                context.Database.Delete();
                context.Database.Initialize(true);
                context.Database.ExecuteSqlCommand(@"insert into [user] (Name)
values ('the.korwin@gmail.com')

insert into Movie(Name, Url, User_Id)
values('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1),
('1', '1', 1)");
            }
        }
    }
}
