// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.FieldMask;

public class FieldMaskConverterFactory(HashSet<string> allFieldPaths) : IFieldMaskConverterFactory
{
    public FieldMaskConverter Create(IList<string> fieldMask)
    {
        return new FieldMaskConverter(fieldMask, allFieldPaths);
    }
}
