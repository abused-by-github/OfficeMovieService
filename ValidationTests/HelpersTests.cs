using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Svitla.MovieService.Tests.ValidationTests
{
    [TestClass]
    public class HelpersTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DeploymentItem("TestCases.mdb")]
        [DataSource("System.Data.OleDb", "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=\"TestCases.mdb\"", "Url", DataAccessMethod.Sequential)]
        public void Url()
        {
            var actual = Core.Validation.Url.IsValid(TestContext.DataRow["Url"].ToString());
            var expected = Convert.ToBoolean(TestContext.DataRow["IsValid"]);
            Assert.AreEqual(expected, actual);
        }
    }
}
