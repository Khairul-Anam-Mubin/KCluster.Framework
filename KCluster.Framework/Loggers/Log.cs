﻿using KCluster.Framework.ORM.Interfaces;

namespace KCluster.Framework.Loggers;

public class Log : IRepositoryItem
{
    public string Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }

    public Log(string message)
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
        Message = message;
    }
}