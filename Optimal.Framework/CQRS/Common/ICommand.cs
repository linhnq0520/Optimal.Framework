using MediatR;

namespace Optimal.Framework.CQRS.Common
{
    public interface ICommand<TResult> { }

    public interface ICommand : ICommand<Unit> { }
}
