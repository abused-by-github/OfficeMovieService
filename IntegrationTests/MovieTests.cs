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
values ('the.korwin@gmail.com'), ('the.korwin1@gmail.com'), ('the.korwin2@gmail.com')

insert into Movie(Name, Url, ImageUrl, User_Id)
values('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/959/17959_thumb.jpg', 1),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/959/17959_thumb.jpg', 1),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/959/17959_thumb.jpg', 2),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/959/17959_thumb.jpg', 3),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 1),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 2),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 3),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 1),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 2),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 3),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 1),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 2),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 3),
('Темный рыцарь: Возрождение легенды', 'http://www.ya.ru', 'http://static.kinokopilka.tv/system/images/movies/covers/000/017/592/17592_thumb.jpg', 1)");
            }
        }
    }
}
