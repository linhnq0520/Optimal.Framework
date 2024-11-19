namespace Optimal.Framework.Services.Configuration
{
    public interface ISettingService
    {
        Task<ISetting> LoadSetting(Type type);
    }
}
