using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Zulu;

namespace ZuluDevTools.Handlers.Core
{
    /// <summary>
    /// Предоставляет функционал по регистрации обработчиков событий от системы Zulu
    /// </summary>
    [ComVisible(true)]
    public class Registrator
    {
        protected Dictionary<int, IZuluEventHandler> _handlers = new Dictionary<int, IZuluEventHandler>();

        /// <summary>
        /// Выполняет регистрацию обработчика событий от системы Zulu
        /// </summary>
        /// <param name="zuluEvent">Константа, описывающая код события</param>
        /// <param name="handler">Интерфейс обработчика</param>
        public void RegisterHandler(eZuluEvents zuluEvent, IZuluEventHandler handler)
        {
            RegisterHandler(Convert.ToInt32(zuluEvent), handler);
        }
        /// <summary>
        /// Выполняет регистрацию обработчика событий от системы Zulu
        /// </summary>
        /// <param name="zuluEvent">Код события, описываемый набором констант</param>
        /// <param name="handler">Интерфейс обработчика</param>
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
