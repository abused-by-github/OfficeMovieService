// <auto-generated />
namespace MovieService.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    public sealed partial class AddTmdbMovie : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddTmdbMovie));
        
        string IMigrationMetadata.Id
        {
            get { return "201310160822041_AddTmdbMovie"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
