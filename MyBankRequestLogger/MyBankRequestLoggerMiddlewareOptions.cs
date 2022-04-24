using Serilog;

namespace MyBankRequestLogger
{
    public class MyBankRequestLoggerMiddlewareOptions
    {
        public LoggerConfiguration LoggerConfiguration { get; set; }
        public string ModuleName { get; set; }
    }
}