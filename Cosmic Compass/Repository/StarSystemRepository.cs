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

        public string Update(string id, StarSystem updatedStarSystem)
        {
            StarSystem starSystem = Get(id);
            if (starSystem == null)
            {
                throw new Exception("The requested star system does not exist.");
            }
            starSystem.Name = updatedStarSystem.Name;
            starSystem.Stars = updatedStarSystem.Stars;
            starSystem.Planets = updatedStarSystem.Planets;
            string updatedId = _provider.Connection.Set(starSystem);

            return updatedId;
        }

        public void Remove(string id)
        {
            _provider.Connection.Unlink(typeof(StarSystem) + ":" + id);
            return;
        }
    }
}
