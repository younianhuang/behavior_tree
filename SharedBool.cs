namespace BehaviorTree
{
    [System.Serializable]
    public class SharedBool : SharedVariable<bool>
    {
        public static implicit operator SharedBool(bool value) { return new SharedBool { m_Value = value }; }

    }
}