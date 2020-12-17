namespace BehaviorTree
{
	[System.Serializable]
	public class SharedFloat : SharedVariable<float>
	{
		public static implicit operator SharedFloat(float value) { return new SharedFloat { m_Value = value }; }
	}
}