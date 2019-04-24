using System.Collections.Generic;

namespace ZuluDevTools.Handlers.Core
{
    public abstract class ProcessorModule : Registrator
    {
        public abstract void Load();
        internal void Load(HandlerProcessor registrator)
        {
            foreach(KeyValuePair<int, IZuluEventHandler> pair in _handlers)
            {
                registrator.RegisterHandler(pair.Key, pair.Value);
            }
        }
    }
}
