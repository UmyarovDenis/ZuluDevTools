using System;

namespace ZuluDevTools.Annotations.DataBases
{
    /// <summary>
    /// Предоставляет описание пользовательского названия поля
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FieldDisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Пользовательское название поля
        /// </summary>
        public string DisplayName { get; set; }
    }
}
