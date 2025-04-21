// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.QueryHandler;

public interface IQuery<TResult>
{
}

public interface IBaseQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>;

public interface IAsyncQueryHandler<in TQuery, TResult> : IBaseQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query);
}

public interface ISyncQueryHandler<in TQuery, TResult> : IBaseQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    TResult Handle(TQuery query);
}
