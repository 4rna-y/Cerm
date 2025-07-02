namespace Cerm.Config
{
    public interface IConfigService
    {
        AppConfig Get();
        void Load();
        void Save();
    }
}