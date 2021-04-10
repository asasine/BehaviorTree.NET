using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("BehaviorTree.NET.Test")]

namespace BehaviorTree.NET.Blackboard
{
    internal class DictionaryReaderWriter<T> : IBlackboardReader<T>, IBlackboardWriter<T>
    {
        private readonly BlackboardKey key;
        private readonly IDictionary<string, object> entries;

        public DictionaryReaderWriter(BlackboardKey key, IDictionary<string, object> entries)
        {
            this.key = key;
            this.entries = entries;
        }

        public ref readonly BlackboardKey Key => ref key;

        public bool TryGetValue(out T value)
        {
            if (this.entries.TryGetValue(this.key, out object currentValue))
            {
                value = (T)currentValue;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public void SetValue(in T value) => this.entries[this.key] = value;
    }
}
