namespace Game.Services
{
    public interface ISessionService
    {
        public void SaveSession<T>(string key, T value);
        public T GetSession<T>(string key);
    }
}
