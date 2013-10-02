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
            var movieFacade = container.GetComponent<IMovieFacade>();

            var movies = movieFacade.FindMovies(new Paging(3, 1));
            Assert.AreEqual(10, movies.Total);
            Assert.AreEqual(3, movies.Items.Count());
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
