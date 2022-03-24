using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using VCCSharp.IoC;

namespace VCCSharp.Models.Configuration.Support;

/// <summary>
/// Load/Save an object via the given path.
/// </summary>
public interface IPersistence<T>
{
    T Load(string path);
    void Save(string path, T model);
}

public abstract class Persistence<T> : IPersistence<T> 
{
    private readonly IFactory _factory;

    protected Persistence(IFactory factory)
    {
        _factory = factory;
    }

    public T Load(string path)
        => Load(_factory.Get<T>(), path);

    private static T Load([NotNull] T instance, string path)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        try
        {
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                JsonConvert.PopulateObject(content, instance);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"An error occurred trying to read configuration file: {e.Message} -- Using default configuration");
        }

        return instance;
    }

    public void Save(string path, T model)
        => File.WriteAllText(path, JsonConvert.SerializeObject(model, Formatting.Indented));
}
