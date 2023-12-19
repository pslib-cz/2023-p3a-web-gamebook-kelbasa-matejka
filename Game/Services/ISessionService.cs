namespace Game.Services
{
    public interface ISessionService
    {
        public void SaveSession<T>(HttpContext httpContext, string key, T value);
        public T GetSession<T>(HttpContext httpContext, string key);
    }
}
