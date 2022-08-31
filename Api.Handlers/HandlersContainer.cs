global using Api.Handlers.Utilitaries;
global using Api.Models;
global using Api.Handlers.Dtos;
global using Api.Handlers.Kernel;
global using Api.Models.Entities;
global using Api.Models.Enums;
global using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.Handlers;

/// <summary>
/// 
/// </summary>
public class HandlersContainer
{
    private readonly List<Handler> handlers;

    public HandlersContainer(ModelsContext context)
    {
        this.handlers = this.RegisterHandlers(context);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="HandlerNotFoundException"></exception>
    public T Get<T>() where T : Handler
    {
        return (T) this.handlers.FirstOrDefault(h => h.GetType().Equals(typeof(T))) ??
            throw new HandlerNotFoundException(typeof(T).Name);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private List<Handler> RegisterHandlers(ModelsContext context)
    {
        List<Handler> handlers = new List<Handler>();
        Assembly.GetAssembly(typeof(HandlerBase))
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(HandlerBase)))
            .ToList()
            .ForEach(t => handlers.Add((Handler)Activator.CreateInstance(t, context)));

        return handlers;
    }
}

public class HandlerNotFoundException : Exception
{
    public HandlerNotFoundException(string handlerType) : base($"Handler {handlerType} not found") { }
}