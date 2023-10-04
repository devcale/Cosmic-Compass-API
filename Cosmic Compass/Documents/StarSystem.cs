using System.ComponentModel;
using Redis.OM.Modeling;

namespace Cosmic_Compass.Documents
{
    [Document(StorageType = StorageType.Json)]
    public class StarSystem
    {
        [RedisIdField][Indexed(Sortable = true)] public Ulid StarSystemId { get; set; }
        [DefaultValue("")] public string Name { get; set; }
        //public ICollection<Star> Stars { get; set; }
    }
}
