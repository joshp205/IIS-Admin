using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace IISAdministration.Models
{
    public class AlertStyle : SmartEnum<AlertStyle, string>
    {
        public static AlertStyle Success = new AlertStyle(nameof(Success), "success");
        public static AlertStyle Information = new AlertStyle(nameof(Information), "info");
        public static AlertStyle Warning = new AlertStyle(nameof(Warning), "warning");
        public static AlertStyle Danger = new AlertStyle(nameof(Danger), "danger");

        [JsonConstructor]
        protected AlertStyle(string name, string value) : base(name, value) { }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
