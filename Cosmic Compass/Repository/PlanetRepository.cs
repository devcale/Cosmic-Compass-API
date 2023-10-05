using Cosmic_Compass.Documents;
using Redis.OM.Searching;
using Redis.OM;

namespace Cosmic_Compass.Repository
{
    public class PlanetRepository
    {
        private RedisConnectionProvider _provider;

        public PlanetRepository()
        {
            _provider = new RedisConnectionProvider("redis://:" + Environment.GetEnvironmentVariable("ASPNETCORE_REDISPASS") + "@redis-13421.c57.us-east-1-4.ec2.cloud.redislabs.com:13421");
            _provider.Connection.CreateIndex(typeof(Planet));
        }

        public IRedisCollection<Planet> FindAll()
        {
            IRedisCollection<Planet> planets = _provider.RedisCollection<Planet>();

            return planets;
        }

        public Planet Get(string id)
        {
            Planet planet = _provider.Connection.Get<Planet>(typeof(Planet) + ":" + id);
            if (planet == null)
            {
                throw new Exception("The requested planet doesnt exist.");
            }

            return planet;
        }

        public string Create(Planet planet)
        {
            string id = _provider.Connection.Set(planet);

            return id;
        }
    }
}
