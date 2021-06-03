using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace URA.API.TESTS
{
    public class BaseTest
    {
        private static Lazy<IHost> Container = new Lazy<IHost>(() =>
        {            
            return Program.CreateHostBuilder(new string[] { }).Build();
        });

        public static T GetService<T>()
        {
            return (T)Container.Value.Services.GetService(typeof(T));
        }
    }
}
