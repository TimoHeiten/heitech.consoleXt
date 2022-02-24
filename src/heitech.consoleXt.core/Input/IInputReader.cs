
using System.Threading.Tasks;

namespace heitech.consoleXt.core.Input
{
    internal interface IInputReader
    {
        ///<summary>
        /// blocks until the user entered his input (essentially a readline wrapper)
        ///</summary>
        Task<LineResult> NextLineAsync();
    }
}