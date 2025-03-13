using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TestDelaunayGenerator
{
    class CsvFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            output.Write(logEvent.Timestamp.ToString("dd-MM-yyyy H:mm"));
            output.Write(";");
            output.Write(logEvent.Level);
            output.Write(";");
            //записываем свойства объекта в формате csv
            foreach (KeyValuePair<string, LogEventPropertyValue> kv in logEvent.Properties)
            {
                PropertyInfo[] objProperties = kv.Value.GetType().GetProperties();
                
                var value = (LogEventProperty[])objProperties[objProperties.Length - 1].GetValue(kv.Value);
                List<string> properties = new List<string>();
                foreach (var val in value)
                    properties.Add(val.Value.ToString());
                output.Write(string.Join(";", properties));

            }
            output.Write(";");

            output.WriteLine();
        }
    }
}
