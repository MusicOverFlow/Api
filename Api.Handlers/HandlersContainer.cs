global using Api.Handlers.Utilitaries;
global using Api.Models;
global using Api.Handlers.Dtos;
global using Api.Handlers.Kernel;
global using Api.Models.Entities;
global using Api.Models.Enums;
global using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Handlers;

public class HandlersContainer
{
    public IServiceProvider Services { get; }
    private readonly IServiceScope scope;
    private readonly Dictionary<Type, Func<Handler>> handlersFactories;

    /// <summary>
    /// The application's handlers container, inject the context factory lambda
    /// </summary>
    /// <param name="modelsContext"></param>
    public HandlersContainer(IServiceProvider services)
    {
        this.Services = services;
        this.scope = this.Services.CreateScope();
        this.handlersFactories = this.RegisterHandlers();
    }

    /// <summary>
    /// Method to retrieve a handler by its type, the handler will be instanciated with the context factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>The instanciated handler</returns>
    /// <exception cref="HandlerNotFoundException"></exception>
    public T Get<T>() where T : Handler
    {
        if (this.handlersFactories.ContainsKey(typeof(T)))
        {
            return (T) this.handlersFactories[typeof(T)]();
        }
        throw new HandlerNotFoundException(typeof(T).Name);
    }

    /// <summary>
    /// Method reading the assembly to register all handlers, storing them in a dictionary with a lambda to instanciate them later
    /// </summary>
    /// <param name="context"></param>
    /// <returns>The handlers dictionary</returns>
    private Dictionary<Type, Func<Handler>> RegisterHandlers()
    {
        return Assembly.GetAssembly(typeof(HandlerBase))
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(HandlerBase)))
            .ToDictionary(tKey => tKey, tValue =>
            {
                Func<Handler> factory = () => (Handler)Activator.CreateInstance(tValue, this.scope.ServiceProvider.GetRequiredService<ModelsContext>());
                return factory;
            });
    }
}

public class HandlerNotFoundException : Exception
{
    public HandlerNotFoundException(string handlerType) : base($"Handler {handlerType} not found") { }
}