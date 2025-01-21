using UnityEngine;

namespace Game
{
    public class TestAnalyticsService : IAnalyticsService
    {
        public void Send(string id)
        {
            Debug.Log($"Analytic: {id}");
        }

        public void Send(string id, string arg)
        {
            Debug.Log($"Analytic: {id}, {arg}");
        }
    }
}