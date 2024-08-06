using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Optimal.Framework.Data.ConfigManager;
using Optimal.Framework.Data.Migration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.Infrastructure.Extensions
{
    public static class ApplicationExtensions
    {
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }
        //public static void StartEngine(this IApplicationBuilder application)
        //{
        //    IEngine current = EngineContext.Current;
        //    if (!DataSettingsManager.IsDatabaseInstalled())
        //    {
        //        return;
        //    }

        //    if (instance.Get<OptimalConfiguration>().RunMigration)
        //    {
        //        IMigrationManager migrationManager = current.Resolve<IMigrationManager>();
        //        IEnumerable<Assembly> enumerable = from ass in AppDomain.CurrentDomain.GetAssemblies()
        //                                           where ass.FullName.StartsWith("Jits.Neptune.Data", StringComparison.InvariantCultureIgnoreCase)
        //                                           select ass;
        //        Console.WriteLine("Applying data engine migration");
        //        foreach (Assembly item in enumerable)
        //        {
        //            migrationManager.ApplyUpMigrations(item, MigrationProcessType.NoMatter);
        //        }

        //        enumerable = from ass in AppDomain.CurrentDomain.GetAssemblies()
        //                     where ass.FullName.StartsWith("Jits.Neptune.Web", StringComparison.InvariantCultureIgnoreCase)
        //                     select ass;
        //        Console.WriteLine("Applying service data migration");
        //        foreach (Assembly item2 in enumerable)
        //        {
        //            migrationManager.ApplyUpMigrations(item2, MigrationProcessType.NoMatter);
        //        }
        //    }

        //}
    }
}
