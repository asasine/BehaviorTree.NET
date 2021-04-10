using System.Collections.Generic;

namespace BehaviorTree.NET.Blackboard
{
    public class Blackboard : IBlackboard
    {
        private IDictionary<string, object> map = new Dictionary<string, object>();

        public IBlackboardReader<T> CreateInputEntry<T>(in BlackboardKey key) => new DictionaryReaderWriter<T>(key, this.map);
        public IBlackboardWriter<T> CreateOutputEntry<T>(in BlackboardKey key) => new DictionaryReaderWriter<T>(key, this.map);
    }
}
