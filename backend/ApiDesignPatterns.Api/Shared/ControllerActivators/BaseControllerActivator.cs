// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Npgsql;

namespace backend.Shared.ControllerActivators;

public abstract class BaseControllerActivator(IConfiguration configuration) : IControllerActivator
{
    public abstract object? Create(ControllerContext context);

    object IControllerActivator.Create(ControllerContext context)
    {
        return Create(context) ?? throw new InvalidOperationException("Controller creation returned null");
    }

    public virtual void Release(ControllerContext context, object controller)
    {
        if (context.HttpContext.Items["Disposables"] is not List<IDisposable> disposables)
        {
            return;
        }

        disposables.Reverse();

        foreach (IDisposable disposable in disposables)
        {
            disposable.Dispose();
        }
    }

    protected static void TrackDisposable(ControllerContext context, IDisposable disposable)
    {
        IDictionary<object, object?> items = context.HttpContext.Items;

        if (!items.TryGetValue("Disposables", out object? list))
        {
            list = new List<IDisposable>();
            items["Disposables"] = list;
        }

        (list as List<IDisposable>)?.Add(disposable);
    }

    protected NpgsqlConnection CreateDbConnection()
    {
        return new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
}
