using Autofac;
using System;
using System.IO;
using System.Reflection;

namespace ArcDocuments.Core.Configuration
{
    public static class ContainerSetup 
    {
        public static IContainer GetContainer(ContainerBuilder containerBuilder = null)
        {
            var builder = containerBuilder ?? (containerBuilder = new ContainerBuilder());

            LoadAssemblies();
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyModules(assembly);

            return builder.Build();
        }

        public static void LoadAssemblies(string startWith = "Arc")
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{startWith}*.dll");
            foreach (string file in files)
            {
                Assembly.LoadFile(file);
            }
        }
    }
}
