using KCluster.Framework.Extensions;
using KCluster.Framework.Mediators;
using KCluster.Framework.Results;

namespace KCluster.Framework.CQRS;

public class CommandExecutor : ICommandExecutor
{
    private readonly IMediator _mediator;

    public CommandExecutor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IResult> ExecuteAsync<TCommand>(TCommand command)
        where TCommand : class, ICommand
    {
        var validationResult = command.GetValidationResult();

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        try
        {
            var result = await _mediator.SendAsync<TCommand, IResult>(command);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return Result.Error(e.Message);
        }
    }

    public async Task<IResult<TResponse>> ExecuteAsync<TCommand, TResponse>(TCommand command)
        where TCommand : class, ICommand
        where TResponse : class
    {
        var validationResult = command.GetValidationResult<TResponse>();

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        try
        {
            var result = await _mediator.SendAsync<TCommand, IResult<TResponse>>(command);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return Result.Error<TResponse>(e.Message);
        }
    }
}