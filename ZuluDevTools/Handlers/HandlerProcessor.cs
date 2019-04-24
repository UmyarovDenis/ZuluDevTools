using System;
using System.Runtime.InteropServices;
using Zulu;
using ZuluDevTools.Handlers.Core;

namespace ZuluDevTools.Handlers
{
    /// <summary>
    /// Предоставляет методы по регистрации обработчиков событий от системы Zulu
    /// </summary>
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
        /// <summary>
        /// Возвращает экземпляр по шаблону Singleton
        /// </summary>
        /// <returns></returns>
        public static HandlerProcessor GetInstance()
        {
            if (_handlerProcessor == null)
                _handlerProcessor = new HandlerProcessor();

            return _handlerProcessor;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modules">Загружаемые модули процессора, содержащие обработчики событий от системы Zulu</param>
        /// <returns></returns>
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
        /// <summary>
        /// Выполняет обработку поступившего события
        /// </summary>
        /// <param name="EventID">Код события из набора констант eZuluEvents</param>
        /// <param name="Source">Объект-источник события</param>
        /// <param name="Param1">Дополнительный параметр 1</param>
        /// <param name="Param2">Дополнительный параметр 2</param>
        /// <param name="Param3">Дополнительный параметр 3</param>
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
        /// <summary>
        /// Выполняет загрузку модулей, содержащих обработчики событий от системы Zulu
        /// </summary>
        /// <param name="modules">Загружаемые модули процессора, содержащие обработчики событий от системы Zulu</param>
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
