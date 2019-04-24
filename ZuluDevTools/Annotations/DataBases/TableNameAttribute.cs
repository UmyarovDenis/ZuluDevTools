using System;

namespace ZuluDevTools.Annotations.DataBases
{
    /// <summary>
    /// Предоставляет имя таблицы базы данных Zulu
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableNameAttribute : Attribute
    {
        private readonly string _tableName;

        public TableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }
    }
}
