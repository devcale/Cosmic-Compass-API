using Redis.OM.Modeling;
using System;
using System.ComponentModel;
using Cosmic_Compass.Enums;

namespace Cosmic_Compass.Documents
{
    [Document(StorageType = StorageType.Json)]
    public class Star
    {
        [RedisIdField][Indexed(Sortable = true)] public Ulid StarId { get; set; }
        public string StarSystemId { get; set; }
        [DefaultValue("")] public string Name { get; set; }
        [DefaultValue(Enums.StarType.Unknown)] public StarType Type { get; set; }
        [DefaultValue(-1)] public double Mass { get; set; } // Unit is solar masses,-1 if unknown

    }
}
