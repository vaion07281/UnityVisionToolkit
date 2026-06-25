using NUnit.Framework;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Tests.Runtime
{
    public class StateMachineTests
    {
        private class TestOwner { }

        private class TestState : State<TestOwner>
        {
            public bool IsEntered { get; private set; }
            public bool IsExited { get; private set; }
            public bool IsUpdated { get; private set; }

            public TestState(TestOwner owner, StateMachine<TestOwner> stateMachine) : base(owner, stateMachine) { }

            public override void Enter() => IsEntered = true;
            public override void Exit() => IsExited = true;
            public override void LogicUpdate() => IsUpdated = true;
        }

        [Test]
        public void StateMachine_ChangesState_Successfully()
        {
            // Arrange
            var owner = new TestOwner();
            var stateMachine = new StateMachine<TestOwner>();
            var state1 = new TestState(owner, stateMachine);
            var state2 = new TestState(owner, stateMachine);

            stateMachine.Initialize(state1);

            // Act
            stateMachine.ChangeState(state2);

            // Assert
            Assert.IsTrue(state1.IsExited);
            Assert.IsTrue(state2.IsEntered);
            Assert.IsFalse(state2.IsExited);
        }

        [Test]
        public void StateMachine_UpdatesCurrentState()
        {
            // Arrange
            var owner = new TestOwner();
            var stateMachine = new StateMachine<TestOwner>();
            var state = new TestState(owner, stateMachine);

            stateMachine.Initialize(state);

            // Act
            stateMachine.OnUpdate();

            // Assert
            Assert.IsTrue(state.IsUpdated);
        }
    }
}
