using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svitla.MovieService.Container;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebApi.Controllers;

namespace Svitla.MovieService.Tests.IntegrationTests
{
    [TestClass]
    public class MovieTests
    {
        [TestMethod]
        public void ListMovies()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var container = new MovieServiceApplicationContainer();
            var movieController = (MovieController) container.DependencyResolver.GetService(typeof(MovieController));

            var movies = movieController.List(new Paging(3, 1));
            Assert.AreEqual(10, movies.Data.Total);
            Assert.AreEqual(3, movies.Data.Items.Count());
        }

        [TestMethod]
        public void RecreateDBFromModel()
        {
            var contextType = Type.GetType("Svitla.MovieService.DataAccess.DataContext, DataAccess");

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var context = Activator.CreateInstance(contextType,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { connectionString },
                null) as DbContext;
            context.Database.Delete();
            context.Database.Initialize(true);
        }
    }
}
