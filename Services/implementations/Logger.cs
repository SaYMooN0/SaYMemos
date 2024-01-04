namespace SaYMemos.Services.implementations
{
    public class Logger : interfaces.ILogger
    {
        private const string
            LogsFolder = "logs",
            RuntimeFolder = "runtime logs";
        private readonly string _runtimeFile, _logFile;

        public Logger()
        {
            string timestamp = CurentTime();
            _runtimeFile = $"{timestamp}-runtime-log.txt";
            _logFile = $"{timestamp}-log.txt";

            EnsureLogFileExists(_runtimeFile, RuntimeFolder);
            EnsureLogFileExists(_logFile, LogsFolder);

            Runtime("Logger Instance Created");
        }

        public void Runtime(string message) => WriteLog(_runtimeFile, $"{CurentTime()} {message}", RuntimeFolder);
        public void CriticalError(string message) => Log(message, "CRITICAL");

        public void Error(string message) => Log(message, "ERROR");

        public void Info(string message) => Log(message, "INFO");

        private void Log(string message, string level) =>
            WriteLog(_logFile, $"[{level}] {CurentTime()} {message}", LogsFolder);

        private void WriteLog(string fileName, string message, string folder) =>
            File.AppendAllText(Path.Combine(folder, fileName), message + Environment.NewLine);

        private void EnsureLogFileExists(string fileName, string folder)
        {
            string filePath = Path.Combine(folder, fileName);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            if (!File.Exists(filePath)) File.Create(filePath).Close();
        }

        private string CurentTime() => DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss");
    }
}
