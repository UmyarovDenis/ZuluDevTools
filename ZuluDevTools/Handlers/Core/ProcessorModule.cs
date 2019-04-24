using System.Collections.Generic;

namespace ZuluDevTools.Handlers.Core
{
    /// <summary>
    /// Предоставляет методы по загрузке модулей, содержащих обработчики событий от системы Zulu
    /// </summary>
    public abstract class ProcessorModule : Registrator
    {
        /// <summary>
        /// Выполняет загрузку модулей
        /// </summary>
        public abstract void Load();
        /// <summary>
        /// Внедряет модули в процессор обработки событий
        /// </summary>
        /// <param name="registrator">Экземпляр класса процессора обработки событий</param>
        internal void Load(HandlerProcessor registrator)
        {
            foreach(KeyValuePair<int, IZuluEventHandler> pair in _handlers)
            {
                registrator.RegisterHandler(pair.Key, pair.Value);
            }
        }
    }
}
