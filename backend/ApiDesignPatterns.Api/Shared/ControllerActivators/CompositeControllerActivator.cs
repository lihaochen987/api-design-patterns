// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;

namespace backend.Shared.ControllerActivators;

public class CompositeControllerActivator(
    IConfiguration configuration,
    IEnumerable<BaseControllerActivator> activators)
    : BaseControllerActivator(configuration)
{
    public override object? Create(ControllerContext context)
    {
        var exceptions = new List<Exception>();
        var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();

        foreach (var activator in activators)
        {
            try
            {
                object? controller = activator.Create(context);
                if (controller != null)
                {
                    return controller;
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(new Exception(
                    $"Activator {activator.GetType().Name} failed to create controller {controllerType.Name}",
                    ex));
            }
        }

        throw new AggregateException(
            $"No activator could create controller for type: {controllerType.Name}",
            exceptions);
    }

    public override void Release(ControllerContext context, object controller)
    {
        base.Release(context, controller);

        foreach (var activator in activators)
        {
            try
            {
                activator.Release(context, controller);
            }
            catch (Exception)
            {
                // Log if needed, but continue trying other activators
            }
        }
    }
}
