using System.Text.Json.Serialization;

namespace PubSubRouting.Models {
    public class CloudEvent<T>
    {
        public string type { get; set; }

        public string source { get; set; }
        public T data { get; set; }
    }

    public class Product
    {
        public string Description { get; set; }
    }

    public class Gadget : Product
    {
        public string GadgetField { get; set; }
    }

    public class Widget : Product
    {
        public string WidgetField { get; set; }
    }
}