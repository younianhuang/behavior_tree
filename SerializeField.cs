using System;

namespace BehaviorTree
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class SerializeField : Attribute
	{
	}

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class NonSerializeField : Attribute
    {

    }
}