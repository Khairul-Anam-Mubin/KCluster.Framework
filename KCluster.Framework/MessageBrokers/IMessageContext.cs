﻿namespace KCluster.Framework.MessageBrokers;

public interface IMessageContext<out TMessage> :
    IEventBus,
    ICommandBus
    where TMessage : class
{
    TMessage Message { get; }

    Task RespondAsync<TResponse>(TResponse response) where TResponse : class;
}