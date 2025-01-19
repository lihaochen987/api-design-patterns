// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Shared.QueryProcessor;

public interface IQueryProcessor
{
    Task<TResult?> Process<TResult>(IQuery<TResult> query);
}
