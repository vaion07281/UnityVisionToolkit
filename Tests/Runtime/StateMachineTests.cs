using NUnit.Framework;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Tests.Runtime
{
    public class StateMachineTests
    {
        private class TestState : IState
        {
            public bool IsEntered { get; private set; }
            public bool IsExited { get; private set; }
            public bool IsUpdated { get; private set; }

            public void OnEnter() => IsEntered = true;
            public void OnExit() => IsExited = true;
            public void OnUpdate() => IsUpdated = true;
            public void OnFixedUpdate() {}
        }

        [Test]
        public void StateMachine_ChangesState_Successfully()
        {
            // Arrange
            var stateMachine = new StateMachine();
            var state = new TestState();

            // Act
            stateMachine.ChangeState(state);

            // Assert
            Assert.IsTrue(state.IsEntered);
            Assert.IsFalse(state.IsExited);
        }

        [Test]
        public void StateMachine_UpdatesCurrentState()
        {
            // Arrange
            var stateMachine = new StateMachine();
            var state = new TestState();
            stateMachine.ChangeState(state);

            // Act
            stateMachine.Update();

            // Assert
            Assert.IsTrue(state.IsUpdated);
        }
    }
}