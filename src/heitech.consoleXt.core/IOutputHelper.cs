using System.Threading.Tasks;

namespace heitech.consoleXt.core
{
    public interface IOutputHelper
    {
        Task WriteAsync(object obj);
    }
}
