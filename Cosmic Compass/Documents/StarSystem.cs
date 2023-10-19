using System.ComponentModel;
using Redis.OM.Modeling;

namespace Cosmic_Compass.Documents
{
    [Document(StorageType = StorageType.Json)]
    public class StarSystem
    {
        public StarSystem()
        {
            Stars = new List<Star>();
            Planets = new List<Planet>();
        }

        [RedisIdField][Indexed(Sortable = true)] public Ulid StarSystemId { get; set; }
        [DefaultValue("")] public string Name { get; set; }
        public ICollection<Star> Stars { get; set; }
        public ICollection<Planet> Planets { get; set; }
    }
}
