using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin.Configuration
{
    public class PluginConfig: IDictionary<string, object>
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

        public PluginConfig GetNode(string path)
        {
            return (PluginConfig)_Get(Source, path.Split('.'));
        }

        public string GetString(string path)
        {
            return (string)_Get(Source, path.Split('.'));
        }

        /// <summary>
        /// Attemp to get a config value for the named path, and return a provided default value instead if not found
        /// </summary>
        /// <param name="path"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public string GetString(string path, string defawlt)
        {
            var value = _Get(Source, path.Split('.'));
            return value == Undefined.Instance ?
                defawlt :
                (string)value;
        }

        public T[] GetArray<T>(string path)
        {
            ArrayList arrayList = (ArrayList)_Get(Source, path.Split('.'));
            T[] array = new T[arrayList.Count];
            int index = 0;
            foreach (var item in arrayList) {
                array[index++] = (T)Convert.ChangeType(item, typeof(T));
            }
            return array;

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
            set
            {
                throw new NotImplementedException();
            }
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

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { throw new NotImplementedException(); }
        }

      
        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }
    }
}
