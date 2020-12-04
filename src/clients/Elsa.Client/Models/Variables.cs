using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elsa.Client.Models
{
    [DataContract]
    public class Variables
    {
        public Variables()
        {
            Data = new Dictionary<string, object?>();
        }

        public Variables(Variables other) : this(other.Data)
        {
        }

        public Variables(IDictionary<string, object?> data)
        {
            Data = data;
        }

        [DataMember(Order = 1)]public IDictionary<string, object?> Data { get; set; }

        public object? Get(string name) => Has(name) ? Data[name] : default;

        public T Get<T>(string name)
        {
            if (!Has(name))
                return default!;

            var value = Get(name);

            if (value == null)
                return default!;

            return (T)value!;
        }

        public Variables Set(string name, object? value)
        {
            Data[name] = value;
            return this;
        }

        public bool Has(string name) => Data.ContainsKey(name);
    }
}