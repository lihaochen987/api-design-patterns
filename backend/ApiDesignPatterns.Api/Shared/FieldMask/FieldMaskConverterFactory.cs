// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.FieldMask;

public class FieldMaskConverterFactory(
    FieldMaskExpander expander,
    PropertyHandler propertyHandler)
    : IFieldMaskConverterFactory
{
    public FieldMaskConverter Create(IList<string> fieldMask, HashSet<string> allFieldPaths)
    {
        return new FieldMaskConverter(fieldMask, allFieldPaths, expander, propertyHandler);
    }
}
