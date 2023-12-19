using Game.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Game.Services
{
    public class SessionService: ISessionService
    {

        public void SaveSession<T>(HttpContext httpContext, string key, T value)
        {
            var session = httpContext.Session;
            SessionExtension.Set(session, key, value);
        }

        public T GetSession<T>(HttpContext httpContext, string key)
        {
            var session = httpContext.Session;
            return SessionExtension.Get<T>(session, key);
        }

    }
}
