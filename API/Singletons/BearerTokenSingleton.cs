namespace API.Singletons
{
    public class BearerTokenSingleton
    {
        public enum BearerDictKeysEnum
        {
            ARRIVAL_TIME,
            LIFE_TIME,
            TOKEN
        }
        /*
        "157.26.120.xxx"
            "ARRIVAL_TIME": DateTime
            "LIFE_TIME": int
            "TOKEN": string
         */
        public Dictionary<string, Dictionary<BearerDictKeysEnum, object>> BearerDict { get; set; } = new Dictionary<string, Dictionary<BearerDictKeysEnum, object>>();


        // Verify if bearer token is now valid
        public bool BearerLifeTimeIsValid(string ip)
        {
            DateTime arrival = (DateTime)this.BearerDict[ip][BearerDictKeysEnum.ARRIVAL_TIME];
            int arrivalTimeInSecond = arrival.Second;

            int timeNowInSecond = DateTime.Now.Second;
            int lifeTime = (int)this.BearerDict[ip][BearerDictKeysEnum.LIFE_TIME];

            if (timeNowInSecond - arrivalTimeInSecond >= lifeTime)
            {
                return false;
            }
            return true;
        }

        public void StoreBearerToken(string ip, int lifeTime, string token)
        {
            BearerDict.Remove(ip);
            BearerDict.Add(ip, new Dictionary<BearerDictKeysEnum, object>()
            {
                { BearerDictKeysEnum.ARRIVAL_TIME, DateTime.Now },
                { BearerDictKeysEnum.LIFE_TIME, lifeTime },
                { BearerDictKeysEnum.TOKEN, token }
            });
        }
    }
}
