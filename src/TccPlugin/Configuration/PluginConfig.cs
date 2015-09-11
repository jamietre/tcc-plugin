using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.Configuration
{
    public class PluginConfig: IReadOnlyDictionary<string, object>
    {
        public PluginConfig()
        {
            this.Source = new Dictionary<string, object>();
        }
        public PluginConfig(IDictionary<string, object> source)
        {
            this.Source = source;
        }
        private IDictionary<string, object> Source;

        public object Get(string path)
        {
            return _Get(Source, path.Split('.'));
        }

        private object _Get(IDictionary<string, object> source, IEnumerable<string> path)
        {
            object value;

            if (source.TryGetValue(path.First(), out value))
            {
                IDictionary<string, object> sub = value as IDictionary<string, object>;

                if (sub != null)
                {
                    if (path.Count() > 1)
                    {
                        return _Get(sub, path.Skip(1));
                    }
                    return new PluginConfig(sub);
                }
                return value;
            }
            else
            {
                return Undefined.Instance;
            }
        }

        private bool IsObject(object value)
        {
            return value is IDictionary<string, object>;
        }

        public bool ContainsKey(string key)
        {
            return Get(key) != Undefined.Instance;
        }

        public IEnumerable<string> Keys
        {
            get { return Source.Keys; }
        }

        public bool TryGetValue(string key, out object value)
        {
            value = Get(key);
            return value != Undefined.Instance;
        }

        public IEnumerable<object> Values
        {
            get { return Source.Values; }
        }

        public object this[string key]
        {
            get { return Get(key); }
        }

        public int Count
        {
            get { return Source.Count; }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
