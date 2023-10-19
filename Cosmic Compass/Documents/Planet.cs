using Cosmic_Compass.Enums;
using Redis.OM.Modeling;
using System.ComponentModel;

namespace Cosmic_Compass.Documents
{
    [Document(StorageType = StorageType.Json)]
    public class Planet
    {
        [RedisIdField][Indexed(Sortable = true)] public Ulid PlanetId { get; set; }
        public string StarSystemId { get; set; }
        [DefaultValue("")] public string Name { get; set; }
        [DefaultValue(Enums.PlanetType.Unknown)] public PlanetType Type { get; set; }
        [DefaultValue(-1)] public double Radius { get; set; }    // In kilometers, -1 if unknown
        [DefaultValue(-1)] public double DistanceFromStar { get; set; } // In astronomical units (AU), -1 if unknown
        [DefaultValue(false)] public bool Habitable { get; set; }  // Whether the planet is habitable or not, false if unknown

    }
}
