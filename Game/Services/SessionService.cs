using Game.Helpers;
using Microsoft.AspNetCore.Http;

namespace Game.Services
{
    public class SessionService<T> : ISessionService<T>
    {

        private ISession _session;

        public SessionService(IHttpContextAccessor hca)
        {
            _session = hca.HttpContext.Session;
        }

        public void SaveSession<T>(string key, T value)
        {
            if(value != null)_session.Set(key, value);
        }

        public T GetSession<T>(string key)
        {
            T result = _session.Get<T>(key);
            return result;
        }

    }
}
