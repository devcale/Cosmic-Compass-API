﻿using Redis.OM.Searching;
using Redis.OM;
using Cosmic_Compass.Documents;

namespace Cosmic_Compass.Repository
{
    public class StarRepository
    {

        private RedisConnectionProvider _provider;

        public StarRepository()
        {
            _provider = new RedisConnectionProvider("redis://:" + Environment.GetEnvironmentVariable("ASPNETCORE_REDISPASS") + "@redis-13421.c57.us-east-1-4.ec2.cloud.redislabs.com:13421");
            _provider.Connection.CreateIndex(typeof(Star));
        }

        public ICollection<Star> FindAll(string starSystemId)
        {
            StarSystem? starSystem = _provider.Connection.Get<StarSystem>(typeof(StarSystem) + ":" + starSystemId);
            ICollection<Star>? stars = null;
            if (starSystem == null)
            {
                throw new Exception("The requested star system does not exist");
            }
            stars = starSystem.Stars;
            return stars;
        }

        public Star Get(string id)
        {
            Star star = _provider.Connection.Get<Star>(typeof(Star) + ":" + id);
            if (star == null)
            {
                throw new Exception("The requested star doesnt exist.");
            }

            return star;
        }

        public string Create(Star star)
        {
            string id = _provider.Connection.Set(star);

            return id;
        }   
    }
}
