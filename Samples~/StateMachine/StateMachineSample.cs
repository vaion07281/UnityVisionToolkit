using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class IdleState : IState
    {
        public void OnEnter() => Debug.Log("Entered Idle State");
        public void OnExit() => Debug.Log("Exited Idle State");
        public void OnUpdate() => Debug.Log("Updating Idle State");
        public void OnFixedUpdate() {}
    }

    public class WalkState : IState
    {
        public void OnEnter() => Debug.Log("Entered Walk State");
        public void OnExit() => Debug.Log("Exited Walk State");
        public void OnUpdate() => Debug.Log("Updating Walk State");
        public void OnFixedUpdate() {}
    }

    public class StateMachineSample : MonoBehaviour
    {
        private StateMachine _stateMachine;

        private void Start()
        {
            _stateMachine = new StateMachine();
            _stateMachine.ChangeState(new IdleState());
        }

        private void Update()
        {
            _stateMachine.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _stateMachine.ChangeState(new WalkState());
            }
        }
    }
}