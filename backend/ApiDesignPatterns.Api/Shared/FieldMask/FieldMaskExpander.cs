// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.FieldMask;

public class FieldMaskExpander
{
    public HashSet<string> Expand(IList<string> fieldMask, HashSet<string> allFieldPaths)
    {
        HashSet<string> expandedFields = new(StringComparer.OrdinalIgnoreCase);

        if (fieldMask.Contains("*") || !fieldMask.Any())
        {
            return allFieldPaths;
        }

        foreach (string field in fieldMask.Select(f => f.ToLowerInvariant()))
        {
            if (field.EndsWith(".*"))
            {
                // The prefix would remove the .* at the end of the property
                string prefix = field[..^2];
                expandedFields.UnionWith(allFieldPaths.Where(p => p.StartsWith(prefix + ".")));
            }
            else if (allFieldPaths.Contains(field))
            {
                expandedFields.Add(field);
            }
        }

        return expandedFields.Count == 0 ? allFieldPaths : expandedFields;
    }
}
