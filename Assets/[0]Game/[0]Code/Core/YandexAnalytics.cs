using System.Collections.Generic;
using YG;

namespace Game
{
    public class YandexAnalytics : IAnalyticsService
    {
        public void Send(string id)
        {
            YG2.MetricaSend(id);
        }

        public void Send(string id, string arg)
        {
            YG2.MetricaSend(id, new Dictionary<string, string>() { { id, arg } });
        }
    }
}