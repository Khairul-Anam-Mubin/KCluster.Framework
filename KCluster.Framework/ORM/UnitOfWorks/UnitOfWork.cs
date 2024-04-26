﻿using KCluster.Framework.ORM.MongoDb;

namespace KCluster.Framework.ORM.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly List<Func<Task>> _commands;
    private readonly IMongoClientManager _mongoClientManager;
    private readonly DatabaseInfo _databaseInfo;

    public UnitOfWork(IMongoClientManager mongoClientManager, DatabaseInfo databaseInfo)
    {
        _commands = new();
        _mongoClientManager = mongoClientManager;
        _databaseInfo = databaseInfo;
    }

    public void AddCommand(Func<Task> func)
    {
        _commands.Add(func);
    }

    public async Task<bool> SaveChangesAsync()
    {
        using var session = await _mongoClientManager.GetClient(_databaseInfo).StartSessionAsync();

        session.StartTransaction();

        try
        {
            var commandTasks = _commands.Select(c => c());

            await Task.WhenAll(commandTasks);

            await session.CommitTransactionAsync();

            return true;
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
        }

        _commands.Clear();

        return false;
    }
}