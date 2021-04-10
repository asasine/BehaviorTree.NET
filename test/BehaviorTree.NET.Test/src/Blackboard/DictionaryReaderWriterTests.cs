using System.Collections.Generic;
using Xunit;

namespace BehaviorTree.NET.Blackboard.Test
{
    public class DictionaryReaderWriterTests
    {
        [Fact]
        public void UnsetReaderReturnsFalse()
        {
            var key = new BlackboardKey("i", BlackboardEntryType.Input);
            var dictionary = new Dictionary<string, object>();
            var readerWriter = new DictionaryReaderWriter<int>(key, dictionary);
            Assert.False(readerWriter.TryGetValue(out int actual));
        }

        [Fact]
        public void ImplementsReaderInterfaceKeyProperty()
        {
            var key = new BlackboardKey("i", BlackboardEntryType.Input);
            var dictionary = new Dictionary<string, object>();
            IBlackboardReader<int> reader = new DictionaryReaderWriter<int>(key, dictionary);
            Assert.Equal(reader.Key, key);
            Assert.Equal(reader.Key.key, key.key);
            Assert.Equal(reader.Key.type, key.type);
        }

        [Fact]
        public void ImplementsWriterInterfaceKeyProperty()
        {
            var key = new BlackboardKey("i", BlackboardEntryType.Input);
            var dictionary = new Dictionary<string, object>();
            IBlackboardWriter<int> writer = new DictionaryReaderWriter<int>(key, dictionary);
            Assert.Equal(writer.Key, key);
            Assert.Equal(writer.Key.key, key.key);
            Assert.Equal(writer.Key.type, key.type);
        }

        [Fact]
        public void ReaderWriterRoundTrip()
        {
            var key = new BlackboardKey("i", BlackboardEntryType.Input | BlackboardEntryType.Output);
            var dictionary = new Dictionary<string, object>();
            var readerWriter = new DictionaryReaderWriter<int>(key, dictionary);

            for (int i = 0; i < 10; i++)
            {
                readerWriter.SetValue(i);
                Assert.True(readerWriter.TryGetValue(out int actual));
                Assert.Equal(actual, i);
            }
        }
    }
}
