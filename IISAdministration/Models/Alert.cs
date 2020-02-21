using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IISAdministration.Models
{
    public class Alert
    {
        [Required]
        public const string TempDataKey = "TempDataAlerts";

        public AlertStyle Style { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }

        [JsonConstructor]
        public Alert(AlertStyle style, string message)
        {
            Style = style;
            Message = message;
            Dismissable = true;
        }

        public Alert(AlertStyle style, string message, bool dismissable)
        {
            Style = style;
            Message = message;
            Dismissable = dismissable;
        }

        public static Alert Error(string message)
        {
            return new Alert(AlertStyle.Danger, message);
        }

        public static Alert Warning(string message)
        {
            return new Alert(AlertStyle.Warning, message);
        }

        public static Alert Success(string message)
        {
            return new Alert(AlertStyle.Success, message);
        }
    }
}