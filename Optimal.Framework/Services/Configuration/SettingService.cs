using System.ComponentModel;
using System.Reflection;
using LinqToDB;
using Optimal.Framework.Data;
using Optimal.Framework.Domain;
using Optimal.Framework.Helper;

namespace Optimal.Framework.Services.Configuration
{
    internal class SettingService : ISettingService
    {
        private readonly IRepository<Setting> _settingRepository;

        public SettingService(IRepository<Setting> settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public virtual async Task<ISetting> LoadSetting(Type type)
        {
            var settings = Activator.CreateInstance(type) as ISetting;
            try
            {
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    if (prop.CanRead && prop.CanWrite)
                    {
                        string key = type.Name + "." + prop.Name;
                        string setting = await GetSettingByKey<string>(key, null);
                        if (
                            setting != null
                            && TypeDescriptor
                                .GetConverter(prop.PropertyType)
                                .CanConvertFrom(typeof(string))
                            && TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting)
                        )
                        {
                            object value = TypeDescriptor
                                .GetConverter(prop.PropertyType)
                                .ConvertFromInvariantString(setting);
                            prop.SetValue(settings, value);
                        }
                    }
                }
            }
            catch { }

            return settings;
        }

        public virtual async Task<T> GetSettingByKey<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            IDictionary<string, IList<Setting>> settings = await GetAllSettingsDictionary();
            key = key.Trim().ToLowerInvariant();
            if (!settings.ContainsKey(key))
            {
                return defaultValue;
            }

            IList<Setting> settingsByKey = settings[key];
            Setting setting = settingsByKey.FirstOrDefault();

            return (setting != null) ? CommonHelper.To<T>(setting.Value) : defaultValue;
        }

        protected virtual async Task<IDictionary<string, IList<Setting>>> GetAllSettingsDictionary()
        {
            IList<Setting> settings = await GetAllSettings();
            Dictionary<string, IList<Setting>> dictionary =
                new Dictionary<string, IList<Setting>>();
            foreach (Setting s in settings)
            {
                string resourceName = s.Name.ToLowerInvariant();
                Setting settingForCaching = new Setting
                {
                    Id = s.Id,
                    Name = s.Name,
                    Value = s.Value,
                };
                if (!dictionary.ContainsKey(resourceName))
                {
                    dictionary.Add(resourceName, new List<Setting> { settingForCaching });
                }
                else
                {
                    dictionary[resourceName].Add(settingForCaching);
                }
            }

            return dictionary;
        }

        public virtual async Task<IList<Setting>> GetAllSettings()
        {
            return await _settingRepository.Table.ToListAsync();
        }
    }
}
