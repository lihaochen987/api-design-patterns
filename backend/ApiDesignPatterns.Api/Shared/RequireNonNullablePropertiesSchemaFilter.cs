// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace backend.Shared;

public abstract class RequireNonNullablePropertiesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
        {
            return;
        }

        schema.Required ??= new HashSet<string>();

        foreach (var property in schema.Properties)
        {
            if (property.Value.Nullable == false)
            {
                schema.Required.Add(property.Key);
            }

            property.Value.Nullable = false;
        }
    }
}
