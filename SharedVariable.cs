using System;

namespace BehaviorTree
{
	public abstract class SharedVariable
	{
		public bool IsShared
		{
			get => m_IsShared;
			set => m_IsShared = value;
		}

		public bool IsGlobal
		{
			get => m_IsGlobal;
			set => m_IsGlobal = value;
		}

		public string Name
		{
			get => m_Name;
			set => m_Name = value;
		}

		public string PropertyMapping
		{
			get => m_PropertyMapping;
			set => m_PropertyMapping = value;
		}

		public bool IsNone => m_IsShared && string.IsNullOrEmpty(m_Name);

		public void ValueChanged()
		{
		}
		
		public abstract object GetValue();

		public abstract void SetValue(object value);

        public abstract SharedVariable Clone();

        [SerializeField]
        protected bool m_IsShared;

		[SerializeField]
		protected bool m_IsGlobal;

		[SerializeField]
		protected string m_Name;

		[SerializeField]
		protected string m_PropertyMapping;
	}

	public abstract class SharedVariable<T> : SharedVariable
	{
		public T Value
		{
			get => (m_Getter == null) ? m_Value : m_Getter();
			set
			{
				bool flag = !(m_Value?.Equals(value) ?? false);
				if (m_Setter != null)
				{
					m_Setter(value);
				}
				else
				{
					m_Value = value;
				}
				if (flag)
				{
					ValueChanged();
				}
			}
		}
		
		public override object GetValue()
		{
			return Value;
		}

		public override void SetValue(object value)
		{
			if (m_Setter != null)
			{
				m_Setter((T)value);
			}
			else
			{
				m_Value = (T)value;
			}
		}

		public override string ToString()
		{
			return Value?.ToString() ?? "(null)";
		}

        public override SharedVariable Clone()
        {
            return (SharedVariable)this.MemberwiseClone();
        }

        private Func<T> m_Getter;

		private Action<T> m_Setter;

		[SerializeField]
		protected T m_Value;
	}
}