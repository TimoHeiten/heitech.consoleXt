
namespace heitech.consoleXt.core.ScriptEnv
{
    internal class LoopContext
    {
        private bool _isAlive = true;
        internal bool IsAlive => _isAlive;
        internal LoopContext() { }

        internal void StopAlive() => _isAlive = false;

    }
}