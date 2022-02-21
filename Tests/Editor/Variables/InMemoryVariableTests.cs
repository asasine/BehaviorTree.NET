using NUnit.Framework;

namespace BehaviorTree.NET.Variables.Test
{
    [TestFixture]
    public class InMemoryVariableTests
    {
        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void GetValue(int expected)
        {
            IConstant<int> variable = new InMemoryVariable<int>(expected);
            var actual = variable.GetValue();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetValue_ObjectIsSameInstance()
        {
            var expected = new object();
            IConstant<object> variable = new InMemoryVariable<object>(expected);
            var actual = variable.GetValue();
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void SetValueMakesRoundTrip(int expected)
        {
            IVariable<int> variable = new InMemoryVariable<int>();
            variable.SetValue(expected);
            var actual = variable.GetValue();
            Assert.AreEqual(expected, actual);
        }
    }
}
