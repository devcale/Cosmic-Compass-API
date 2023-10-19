using Redis.OM;
using Cosmic_Compass.Documents;
using Redis.OM.Searching;
using System;

namespace Cosmic_Compass.Repository
{
    public class StarSystemRepository
    {
        private RedisConnectionProvider _provider;

        public StarSystemRepository()
        {
            _provider = new RedisConnectionProvider("redis://:" + Environment.GetEnvironmentVariable("ASPNETCORE_REDISPASS") + "@redis-13421.c57.us-east-1-4.ec2.cloud.redislabs.com:13421");
            _provider.Connection.CreateIndex(typeof(StarSystem));
        }

        public IRedisCollection<StarSystem> FindAll()
        {
            IRedisCollection<StarSystem> starSystems = _provider.RedisCollection<StarSystem>();

            return starSystems;
        }

        public StarSystem Get(string id)
        {
            StarSystem starSystem = _provider.Connection.Get<StarSystem>(typeof(StarSystem) + ":" + id);

            return starSystem;
        }

        public string Create(StarSystem starSystem)
        {
            string id = _provider.Connection.Set(starSystem);

            return id;
        }

        public void Update(string id, StarSystem updatedStarSystem)
        {
            _provider.Connection.Set(updatedStarSystem);

        }

        public void Remove(string id)
        {
            _provider.Connection.Unlink(typeof(StarSystem) + ":" + id);
            return;
        }
    }
}
