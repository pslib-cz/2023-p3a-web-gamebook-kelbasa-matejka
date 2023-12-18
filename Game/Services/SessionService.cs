using Game.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Game.Services
{
    public class SessionService<T> : ISessionService
    {

        private ISession _session;

        public SessionService(IHttpContextAccessor hca)
        {
            _session = hca.HttpContext.Session;
        }

        public void SaveSession<T>(string key, T value)
        {
            if (value != null)
            {
                var serialized = JsonSerializer.Serialize(value);
                _session.Set(key, serialized);
            }
        }

        public T GetSession<T>(string key)
        {
            {
                string result = _session.Get<string>(key);
                return JsonSerializer.Deserialize<T>(result);
            }
            
        }

    }
}
