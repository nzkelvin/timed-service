using System;
namespace TimedService.Config
{
    public interface IConfigurationHelper
    {
        T GetSection<T>() where T : class;
        T GetSection<T>(string sectionName) where T : class;
    }
}
