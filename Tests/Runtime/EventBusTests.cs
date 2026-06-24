using NUnit.Framework;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Tests.Runtime
{
    public class EventBusTests
    {
        private struct TestEvent
        {
            public int Value;
        }

        [Test]
        public void EventBus_RaisesEvent_Successfully()
        {
            // Arrange
            int receivedValue = 0;
            EventBus<TestEvent>.Register(e => receivedValue = e.Value);

            // Act
            EventBus<TestEvent>.Raise(new TestEvent { Value = 42 });

            // Assert
            Assert.AreEqual(42, receivedValue);

            // Clean up
            EventBus<TestEvent>.Clear();
        }

        [Test]
        public void EventBus_DeregistersEvent_Successfully()
        {
            // Arrange
            int receivedValue = 0;
            System.Action<TestEvent> handler = e => receivedValue = e.Value;
            EventBus<TestEvent>.Register(handler);
            EventBus<TestEvent>.Deregister(handler);

            // Act
            EventBus<TestEvent>.Raise(new TestEvent { Value = 42 });

            // Assert
            Assert.AreEqual(0, receivedValue);

            // Clean up
            EventBus<TestEvent>.Clear();
        }
    }
}