using Microsoft.Extensions.Configuration;

namespace ChatApp.Requires.Jwt
{
    public class JwtConfig
    {
        public string JwtKey { get; private set; }
        public string JwtRefreshKey { get; private set; }
        public string Audience { get; private set; }
        public string Issuer { get; private set; }
        public long ExpiredTimeSeconds { get; private set; } = 3600; // 60 mins
        public long ExpiredRefreshKeyTimeSeconds { get; private set; } = 2629800; // one month

        public JwtConfig(IConfiguration configuration)
        {
            JwtKey = configuration["AuthConfig:JwtKey"];
            JwtRefreshKey = configuration["AuthConfig:JwtRefreshKey"];
            Audience = configuration["AuthConfig:Audience"];
            Issuer = configuration["AuthConfig:Issuer"];

            string ets = configuration["AuthConfig:ExpiredTimeSeconds"];

            if (long.TryParse(ets, out long etsNumber))
            {
                ExpiredTimeSeconds = etsNumber;
            }

            string erts = configuration["AuthConfig:ExpiredRefreshKeyTimeSeconds"];

            if (long.TryParse(erts, out long ertsNumber))
            {
                ExpiredRefreshKeyTimeSeconds = ertsNumber;
            }
        }
    }
}