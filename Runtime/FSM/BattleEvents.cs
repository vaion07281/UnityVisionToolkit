namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Event raised when the battle state changes (e.g., PlayerTurn -> EnemyTurn).
    /// </summary>
    public struct BattleStateChangedEvent
    {
        public string StateName;
        public BattleStateChangedEvent(string stateName) { StateName = stateName; }
    }

    /// <summary>
    /// Event raised when the battle ends (Win or Loss).
    /// </summary>
    public struct BattleEndedEvent
    {
        public bool IsWin;
        public BattleEndedEvent(bool isWin) { IsWin = isWin; }
    }
}
