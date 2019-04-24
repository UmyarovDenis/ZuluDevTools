using System.Collections.Generic;
using ZuluLib;

namespace ZuluDevTools.Extensions
{
    public delegate TDto LayerDtoHandler<TDto>(Layer layer) where TDto : class;
    public delegate TDto ObjectTypeDtoHandler<TDto>(ObjectType objectType) where TDto : class;
    public delegate TDto ElementDtoHandler<TDto>(Element element) where TDto : class;

    public static class LayerExt
    {
        /// <summary>
        /// Транслирует объекты типа Layer в последовательность
        /// </summary>
        /// <param name="layers">Интерфейс, представляющий коллекцию слоев</param>
        /// <returns></returns>
        public static IEnumerable<Layer> ToEnumerable(this Layers layers)
        {
            for (short i = 1; i <= layers.Count; i++)
                yield return layers[i];
        }
        /// <summary>
        /// Транслирует объекты типа Layer в последовательность объектов передачи данных
        /// </summary>
        /// <typeparam name="TDto">Объект передачи данных</typeparam>
        /// <param name="layers">Интерфейс, представляющий коллекцию слоев</param>
        /// <param name="activator">Конструктор объекта передачи данных</param>
        /// <returns></returns>
        public static IEnumerable<TDto> ToEnumerable<TDto>(this Layers layers, LayerDtoHandler<TDto> activator) where TDto : class
        {
            for (short i = 1; i <= layers.Count; i++)
                yield return activator(layers[i]);
        }
        /// <summary>
        /// Извлекает объекты типа ObjectType и транслирует в новую последовательность
        /// </summary>
        /// <param name="layer">Интерфейс, представляющий тип слоя</param>
        /// <returns></returns>
        public static IEnumerable<ObjectType> FetchObjectTypes(this Layer layer)
        {
            ObjectTypes objectTypes = layer.ObjectTypes;

            for (int i = 0; i < objectTypes.Count; i++)
                yield return objectTypes.GetItemByIndex(i);
        }
        /// <summary>
        /// Извлекает объекты типа ObjectType и транслирует в новую последовательность объектов передачи данных
        /// </summary>
        /// <typeparam name="TDto">Объект передачи данных</typeparam>
        /// <param name="layer">Интерфейс, представляющий тип слоя</param>
        /// <param name="activator">Конструктор объекта передачи данных</param>
        /// <returns></returns>
        public static IEnumerable<TDto> FetchObjectTypes<TDto>(this Layer layer, ObjectTypeDtoHandler<TDto> activator) where TDto : class
        {
            ObjectTypes objectTypes = layer.ObjectTypes;
            
            for (int i = 0; i < objectTypes.Count; i++)
                yield return activator(objectTypes.GetItemByIndex(i));
        }
        /// <summary>
        /// Извлекает объекты типа Elements и транслирует в новую последовательность
        /// </summary>
        /// <param name="layer">Интерфейс, представляющий тип слоя</param>
        /// <returns></returns>
        public static IEnumerable<Element> FetchElements(this Layer layer)
        {
            Elements elements = layer.Elements;

            for (int i = 0; i < elements.Count; i++)
                yield return elements.GetElement(i);
        }
        /// <summary>
        /// Извлекает объекты типа Elements и транслирует в новую последовательность объектов передачи данных
        /// </summary>
        /// <typeparam name="TDto">Объект передачи данных</typeparam>
        /// <param name="layer">Интерфейс, представляющий тип слоя</param>
        /// <param name="activator">Конструктор объекта передачи данных</param>
        /// <returns></returns>
        public static IEnumerable<TDto> FetchElements<TDto>(this Layer layer, ElementDtoHandler<TDto> activator) where TDto : class
        {
            Elements elements = layer.Elements;

            for (int i = 0; i < elements.Count; i++)
                yield return activator(elements.GetElement(i));
        }
    }
}
