using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crypto.Util
{
    public class CoinData
    {
        public String status;
        public Data data;
    }

    public class Coin
    {
        public String uuid;
        public String symbol;
        public String name;
        public String color;
        public String iconUrl;
        public String marketCap;
        public String price;
        public int listedAt;
        public int tier;
        public String change;
        public int rank;
        public List<String> sparkline;
        public bool lowVolume;
        public String coinrankingUrl;

    public String _24hVolume;
        public String btcPrice;
    }

    public class Data
    {
        public Stats stats;
        public List<Coin> coins;
    }

    public class Stats
    {
        public int total;
        public int totalCoins;
        public int totalMarkets;
        public int totalExchanges;
        public String totalMarketCap;
        public String total24hVolume;
    }
}