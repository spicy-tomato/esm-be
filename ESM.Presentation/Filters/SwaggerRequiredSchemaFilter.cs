using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ESM.Presentation.Filters;

public class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
            return;

        foreach (var schemaProp in schema.Properties)
        {
            if (schemaProp.Value.Nullable)
                continue;

            schema.Required.Add(schemaProp.Key);
        }
    }
}