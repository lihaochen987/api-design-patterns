// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.FieldMask;

public interface IFieldMaskConverterFactory
{
    FieldMaskConverter Create(IList<string> fieldMask);
}
