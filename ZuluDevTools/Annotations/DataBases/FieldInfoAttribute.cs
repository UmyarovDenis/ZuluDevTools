using System;
using ZB;

namespace ZuluDevTools.Annotations.DataBases
{
    /// <summary>
    /// Предоставляет описание поля таблицы базы данных Zulu
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FieldInfoAttribute : Attribute
    {
        private readonly string _fieldName;
        private readonly zbFieldType _fieldType;
        private readonly short _size;
        private readonly short _prec;
        private readonly bool _key;

        public FieldInfoAttribute(string fieldName, zbFieldType fieldType, short size, short prec, bool key)
        {
            _fieldName = fieldName;
            _fieldType = fieldType;
            _size = size;
            _prec = prec;
            _key = key;
        }
        /// <summary>
        /// Имя поля
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
        }
        /// <summary>
        /// Тип поля, задается из набора констант zbFieldType
        /// </summary>
        public zbFieldType FieldType
        {
            get { return _fieldType; }
        }
        /// <summary>
        /// Размер поля (имеет значение для строковых полей, для нестроковых полей - 0)
        /// </summary>
        public short Size
        {
            get { return _size; }
        }
        /// <summary>
        /// Точность (для числовых полей), количество знаков после запятой
        /// </summary>
        public short Prec
        {
            get { return _prec; }
        }
        /// <summary>
        /// Признак ключевого поля. Если параметр задан как True, то добавляемое поля будет ключевым
        /// </summary>
        public bool Key
        {
            get { return _key; }
        }
    }
}
