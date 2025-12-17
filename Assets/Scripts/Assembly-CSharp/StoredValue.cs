using System;

public class StoredValue<T>
{
	public Action onValueChanged;

	public string key;

	public T defaultValue;

	public StoredValue(string key, T defaultValue)
	{
		this.key = key;
		this.defaultValue = defaultValue;
	}

	public void ChangeValue(T delta)
	{
		if (typeof(T) == typeof(int))
		{
			int @int = CPlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue));
			CPlayerPrefs.SetInt(key, @int + Convert.ToInt32(delta));
		}
		else if (typeof(T) == typeof(float))
		{
			float @float = CPlayerPrefs.GetFloat(key, Convert.ToSingle(defaultValue));
			CPlayerPrefs.SetFloat(key, @float + Convert.ToSingle(delta));
		}
		else if (typeof(T) == typeof(double))
		{
			double @double = CPlayerPrefs.GetDouble(key, Convert.ToDouble(defaultValue));
			CPlayerPrefs.SetDouble(key, @double + Convert.ToDouble(delta));
		}
		if (onValueChanged != null)
		{
			onValueChanged();
		}
	}

	public void SetValue(T value)
	{
		if (typeof(T) == typeof(int))
		{
			CPlayerPrefs.SetInt(key, Convert.ToInt32(value));
		}
		else if (typeof(T) == typeof(float))
		{
			CPlayerPrefs.SetFloat(key, Convert.ToSingle(value));
		}
		else if (typeof(T) == typeof(double))
		{
			CPlayerPrefs.SetDouble(key, Convert.ToDouble(value));
		}
	}

	public T GetValue()
	{
		if (typeof(T) == typeof(int))
		{
			int @int = CPlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue));
			return (T)Convert.ChangeType(@int, typeof(T));
		}
		if (typeof(T) == typeof(float))
		{
			float @float = CPlayerPrefs.GetFloat(key, Convert.ToSingle(defaultValue));
			return (T)Convert.ChangeType(@float, typeof(T));
		}
		double @double = CPlayerPrefs.GetDouble(key, Convert.ToDouble(defaultValue));
		return (T)Convert.ChangeType(@double, typeof(T));
	}
}
