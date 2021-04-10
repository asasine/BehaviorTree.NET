using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree.NET.Blackboard
{
    public class Blackboard : IBlackboard, IDictionary<string, object>
    {
        private IDictionary<string, object> map = new Dictionary<string, object>();

        public object this[string key] { get => map[key]; set => map[key] = value; }

        public ICollection<string> Keys => map.Keys;

        public ICollection<object> Values => map.Values;

        public int Count => map.Count;

        public bool IsReadOnly => map.IsReadOnly;

        public void Add(string key, object value)
        {
            map.Add(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            map.Add(item);
        }

        public void Clear()
        {
            map.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return map.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return map.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            map.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return map.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return map.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return map.Remove(item);
        }

        public bool TryGetValue(string key, out object value)
        {
            return map.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)map).GetEnumerator();
        }
    }
}
