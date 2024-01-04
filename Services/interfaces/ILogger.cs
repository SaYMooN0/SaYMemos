namespace SaYMemos.Services.interfaces
{
    public interface ILogger
    {
        void Runtime(string message);
        void Info(string message);
        void Error(string message);
        void Error(string message, Exception ex);
        void CriticalError(string message);
    }
}
