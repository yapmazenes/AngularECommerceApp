using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace ECommerce.API.Configurations.Serilog.ColumnWriters
{
    public class UsernameColumnWriter : ColumnWriterBase
    {
        public UsernameColumnWriter() : base(NpgsqlDbType.Varchar)
        {
        }

        public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
            var (userName, value) = logEvent.Properties.FirstOrDefault(p => p.Key == "userName");

            return value?.ToString() ?? string.Empty;
        }
    }
}
