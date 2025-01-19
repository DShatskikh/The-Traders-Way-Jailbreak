using System.Collections.Generic;
using YG;

namespace Game
{
    public class YandexAnalytics : IAnalyticsService
    {
        public void Send(string id)
        {
            YandexMetrica.Send(id);
        }

        public void Send(string id, string arg)
        {
            YandexMetrica.Send(id, new Dictionary<string, string>() { {id, arg} });
        } 
    }
}