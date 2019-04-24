using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Zulu;

namespace ZuluDevTools.Handlers.Core
{
    [ComVisible(true)]
    public class Registrator
    {
        protected Dictionary<int, IZuluEventHandler> _handlers = new Dictionary<int, IZuluEventHandler>();

        public void RegisterHandler(eZuluEvents zuluEvent, IZuluEventHandler handler)
        {
            RegisterHandler(Convert.ToInt32(zuluEvent), handler);
        }
        public void RegisterHandler(int zuluEvent, IZuluEventHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("Handler was null");

            if (!_handlers.ContainsKey(zuluEvent) && !_handlers.ContainsValue(handler))
            {
                _handlers.Add(zuluEvent, handler);
            }
        }
    }
}
