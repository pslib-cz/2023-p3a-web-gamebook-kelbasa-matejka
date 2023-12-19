using Game.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Game.Services
{
    public class SessionService: ISessionService
    {

        private ISession _session;

        public SessionService(IHttpContextAccessor hca)
        {
            _session = hca.HttpContext.Session;
        }

        public void SaveSession<T>(string key, T value)
        {
               SessionExtension.Set(_session, key, value);
        }

        public T GetSession<T>(string key)
        {
            
            return SessionExtension.Get<T>(_session, key);
            
        }

    }
}
