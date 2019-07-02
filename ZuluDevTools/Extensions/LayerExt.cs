using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ZB;
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
        /// <summary>
        /// Выполняет SQL запрос в контексте слоя и выозвращает набор данных
        /// </summary>
        /// <param name="layer">Интерфейс, представляющий тип слоя</param>
        /// <param name="sqlExpression">SQL запрос</param>
        /// <returns></returns>
        public static DataTable ExecuteSQL(this Layer layer, string sqlExpression, string tableName, object context = null)
        {
            IZSqlResult result = null;

            if(context != null)
            {
                result = layer.ExecSQL(sqlExpression, context);
            }
            else
            {
                result = layer.ExecSQL(sqlExpression);
            }

            if (result.RetCode != 0)
                throw new InvalidOperationException(string.Format("Error code: {0}\nError message: {1}"
                    , result.RetCode, result.ErrorString));

            IZbDataset dataSet = (IZbDataset)result.DataSet;

            DataTable dataTable = new DataTable(!string.IsNullOrEmpty(tableName) ? tableName : "Результат запроса");

            dataSet.MoveFirst();
            for(int i = 0; i < dataSet.FieldCount; i++)
            {
                dataTable.Columns.Add(new DataColumn($"{dataSet.GetFieldDef(i).Name}"));
            }

            dataSet.MoveFirst();
            while(dataSet.Eof != true)
            {
                object[] values = new object[dataSet.FieldCount];
                for(int i = 0; i < values.Length; i++)
                {
                    values[i] = dataSet.FieldValue[i];
                }

                dataTable.Rows.Add(values);
                dataSet.MoveNext();
            }

            return dataTable;
        }
        /// <summary>
        /// Упаковывает слой в заданный каталог
        /// </summary>
        /// <param name="layer">Интерфейс, представляющий тип слоя</param>
        /// <param name="destinationFolder">Каталог</param>
        /// <param name="layerPackName">Имя архива (расширение *.zpkg может быть опущено)</param>
        /// <returns></returns>
        public static bool PackLayer(this Layer layer, string destinationFolder, string layerPackName = null)
        {
            string packExtension = ".zpkg";
            string packageName = string.IsNullOrEmpty(layerPackName) ? string.Concat(layer.UserName, packExtension) :
                    !layerPackName.Contains(packExtension) ? string.Concat(layerPackName, packExtension) : layerPackName;

            FileInfo fileInfo = new FileInfo(Path.Combine(destinationFolder, packageName));

            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(destinationFolder);
            }

            ZuluToolsClass zuluTools = new ZuluToolsClass();

            return zuluTools.LayerPack(layer.Name, fileInfo.FullName, 0);
        }
        /// <summary>
        /// Асинхронно упаковывает слой в заданный каталог
        /// </summary>
        /// <param name="layer">Интерфейс, представляющий тип слоя</param>
        /// <param name="destinationFolder">Каталог</param>
        /// <param name="layerPackName">Имя архива (расширение *.zpkg может быть опущено)</param>
        /// <returns></returns>
        public static async Task<bool> PackLayerAsync(this Layer layer, string destinationFolder, string layerPackName = null)
        {
            return await Task.Run(() => PackLayer(layer, destinationFolder, layerPackName));
        }
        /// <summary>
        /// Упаковывает все слои загруженные в карту (имя каждого архива слоя задается в соответствии с пользовательским именем)
        /// </summary>
        /// <param name="layers">Интерфейс, представляющий коллекцию слоев</param>
        /// <param name="destinationFolder">Каталог</param>
        public static void PackLayers(this Layers layers, string destinationFolder)
        {
            for(short i = 1; i <= layers.Count; i++)
            {
                PackLayer(layers[i], destinationFolder);
            }
        }
        /// <summary>
        /// Асинхронно упаковывает все слои загруженные в карту (имя каждого архива слоя задается в соответствии с пользовательским именем)
        /// </summary>
        /// <param name="layers">Интерфейс, представляющий коллекцию слоев</param>
        /// <param name="destinationFolder">Каталог</param>
        public static async void PackLayersAsync(this Layers layers, string destinationFolder)
        {
            await Task.Run(() => PackLayers(layers, destinationFolder));
        }
    }
}
