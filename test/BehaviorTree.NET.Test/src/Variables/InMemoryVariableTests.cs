using Xunit;

namespace BehaviorTree.NET.Variables.Test
{
    public class InMemoryVariableTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void GetValue(int expected)
        {
            IConstant<int> variable = new InMemoryVariable<int>(expected);
            var actual = variable.GetValue();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValue_ObjectIsSameInstance()
        {
            var expected = new object();
            IConstant<object> variable = new InMemoryVariable<object>(expected);
            var actual = variable.GetValue();
            Assert.Same(expected, actual);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void SetValueMakesRoundTrip(int expected)
        {
            IVariable<int> variable = new InMemoryVariable<int>();
            variable.SetValue(expected);
            var actual = variable.GetValue();
            Assert.Equal(expected, actual);
        }
    }
}
