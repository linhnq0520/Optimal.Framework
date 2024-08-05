using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Optimal.Framework.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyAllConfigurationsFromCurrentAssembly(this ModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.GetInterfaces().Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            return modelBuilder;
        }
    }
}
