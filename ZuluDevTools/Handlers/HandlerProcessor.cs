using System;
using System.Runtime.InteropServices;
using Zulu;
using ZuluDevTools.Handlers.Core;

namespace ZuluDevTools.Handlers
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class HandlerProcessor : Registrator, IZuluEvents
    {
        private static HandlerProcessor _handlerProcessor;

        private HandlerProcessor()
        { }
        private HandlerProcessor(params ProcessorModule[] modules)
        {
            LoadModules(modules);
        }
        public static HandlerProcessor GetInstance()
        {
            if (_handlerProcessor == null)
                _handlerProcessor = new HandlerProcessor();

            return _handlerProcessor;
        }
        public static HandlerProcessor GetInstance(params ProcessorModule[] modules)
        {
            if (_handlerProcessor == null)
            {
                foreach(ProcessorModule module in modules)
                {
                    module?.Load();
                }
                _handlerProcessor = new HandlerProcessor(modules);
            }
            else
            {
                _handlerProcessor.LoadModules(modules);
            }

            return _handlerProcessor;
        }
        public void OnZuluEvent(int EventID, object Source, object Param1, object Param2, object Param3)
        {
            try
            {
                if (_handlers.ContainsKey(EventID))
                {
                    IZuluEventHandler handler = _handlers[EventID];
                    handler.Execute(Source, Param1, Param2, Param3);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void LoadModules(params ProcessorModule[] modules)
        {
            foreach (ProcessorModule module in modules)
            {
                module?.Load();
                module?.Load(this);
            }
        }
    }
}
