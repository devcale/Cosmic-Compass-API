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
            _provider.Connection.CreateIndex(typeof(StarSystem));
        }

        public ICollection<Planet> FindAll(string starSystemId)
        {
            StarSystem? starSystem = _provider.Connection.Get<StarSystem>(typeof(StarSystem) + ":" + starSystemId);
            ICollection<Planet>? planets = null;
            if (starSystem == null)
            {
                throw new Exception("The requested star system does not exist");
            }
            planets = starSystem.Planets;
            return planets;
        }

        public Planet? Get(string starSystemId, string planetId)
        {
            Planet? planet = _provider.Connection.Get<Planet>(typeof(Planet) + ":" + planetId);
            if(planet == null)
            {
                throw new Exception("The requested planet does not exist"); 
            }
            else
            {
                if (planet.StarSystemId != starSystemId)
                {
                    throw new Exception("The requested planet is not part of the requested star system.");
                }
            }
            return planet;
        }

        public string Create(Planet planet)
        {
            string id = _provider.Connection.Set(planet);

            return id;
        }

        public string Update(string starSystemId, string planetId, Planet updatedPlanet)
        {
            string updatedId = _provider.Connection.Set(updatedPlanet);
            return updatedId;
        }
    }
}
