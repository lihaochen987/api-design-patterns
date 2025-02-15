// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared;

public class QueryService<TEntity> where TEntity : Identifier
{
    public List<TEntity> Paginate(
        List<TEntity> existingItems,
        int maxPageSize,
        out string? nextPageToken)
    {
        if (existingItems.Count <= maxPageSize)
        {
            nextPageToken = null;
            return existingItems;
        }

        TEntity lastItemInPage = existingItems[maxPageSize - 1];
        nextPageToken = lastItemInPage.Id.ToString();
        return existingItems.Take(maxPageSize).ToList();
    }
}
