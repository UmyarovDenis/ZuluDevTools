using System;
using ZB;

namespace ZuluDevTools.Annotations.DataBases
{
    /// <summary>
    /// Предоставляет описание связи между таблицами
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableLinkAttribute : Attribute
    {
        private readonly object _source;
        private readonly string _sourceField;
        private readonly Type _target;
        private readonly string _targetField;
        private readonly zbLinkType _linkType;

        public TableLinkAttribute(object source, string sourceField, Type target, string targetField, zbLinkType linkType)
        {
            _source = source;
            _sourceField = sourceField;
            _target = target;
            _targetField = targetField;
            _linkType = linkType;
        }
        public object Source
        {
            get { return _source; }
        }
        public string SourceField
        {
            get { return _sourceField; }
        }
        public Type Target
        {
            get { return _target; }
        }
        public string TargetField
        {
            get { return _targetField; }
        }
        public zbLinkType LinkType
        {
            get { return _linkType; }
        }
    }
}
