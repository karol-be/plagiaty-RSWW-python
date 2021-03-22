using System.Threading;
using System.Threading.Tasks;
using Commands;

namespace CommandService.Handlers
{
    public interface IHandler<in T> where T : BaseCommand
    {
        Task<Task> HandleAsync(T command, CancellationToken cancellationToken);
    }
}