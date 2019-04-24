namespace ZuluDevTools.Handlers
{
    /// <summary>
    /// Предоставляет метод обработки события Zulu
    /// </summary>
    public interface IZuluEventHandler
    {
        void Execute(object source, object param1, object param2, object param3);
    }
}
