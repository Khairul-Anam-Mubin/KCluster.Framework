﻿using System.Linq.Expressions;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Filters;
using Peacious.Framework.ORM.Interfaces;

namespace Peacious.Framework.ORM.Builders;

public class IndexBuilder<TEntity>
{
    private readonly IIndex _index;

    public IndexBuilder()
    {
        _index = new Filters.Index();
    }

    public IndexBuilder<TEntity> Ascending(string fieldKey)
    {
        _index.Add(new IndexKey(fieldKey, SortDirection.Ascending));
        return this;
    }

    public IndexBuilder<TEntity> Ascending<TField>(Expression<Func<TEntity, TField>> field)
    {
        return Ascending(ExpressionHelper.GetFieldKey(field));
    }

    public IndexBuilder<TEntity> Descending(string fieldKey)
    {
        _index.Add(new IndexKey(fieldKey, SortDirection.Descending));
        return this;
    }

    public IndexBuilder<TEntity> Descending<TField>(Expression<Func<TEntity, TField>> field)
    {
        return Descending(ExpressionHelper.GetFieldKey(field));
    }

    public IIndex Build()
    {
        return _index;
    }
}