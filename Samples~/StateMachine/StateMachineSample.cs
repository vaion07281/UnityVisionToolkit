using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class IdleState : State<StateMachineSample>
    {
        public IdleState(StateMachineSample owner, StateMachine<StateMachineSample> stateMachine) : base(owner, stateMachine) { }

        public override void Enter() => Debug.Log("Entered Idle State");
        public override void Exit() => Debug.Log("Exited Idle State");
        public override void LogicUpdate() => Debug.Log("Updating Idle State");
    }

    public class WalkState : State<StateMachineSample>
    {
        public WalkState(StateMachineSample owner, StateMachine<StateMachineSample> stateMachine) : base(owner, stateMachine) { }

        public override void Enter() => Debug.Log("Entered Walk State");
        public override void Exit() => Debug.Log("Exited Walk State");
        public override void LogicUpdate() => Debug.Log("Updating Walk State");
    }

    public class StateMachineSample : MonoBehaviour
    {
        public StateMachine<StateMachineSample> StateMachine { get; private set; }

        private void Start()
        {
            StateMachine = new StateMachine<StateMachineSample>();
            StateMachine.Initialize(new IdleState(this, StateMachine));
        }

        private void Update()
        {
            StateMachine?.OnUpdate();
        }

        public void SwitchToWalk()
        {
            if (StateMachine != null)
            {
                StateMachine.ChangeState(new WalkState(this, StateMachine));
            }
        }

        public void SwitchToIdle()
        {
            if (StateMachine != null)
            {
                StateMachine.ChangeState(new IdleState(this, StateMachine));
            }
        }
    }
}
