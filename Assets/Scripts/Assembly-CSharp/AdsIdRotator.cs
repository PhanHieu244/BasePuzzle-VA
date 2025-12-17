public class AdsIdRotator
{
	private string[] _androidUnitIds;

	private string[] _iosUnitIds;

	private string[] _currentUnitIds;

	private int _currentAdIdCounter;

	public string[] currentUnitIds
	{
		get
		{
			return _currentUnitIds;
		}
	}

	public int currentAdIdCounter
	{
		get
		{
			return _currentAdIdCounter;
		}
	}

	public int totalAdIds
	{
		get
		{
			return _currentUnitIds.Length;
		}
	}

	public string currentAdId
	{
		get
		{
			return _currentUnitIds[_currentAdIdCounter];
		}
	}

	public AdsIdRotator(string[] androidIds, string[] iosIds)
	{
		_androidUnitIds = androidIds;
		_iosUnitIds = iosIds;
		_currentUnitIds = _androidUnitIds;
	}

	public string GetUnitId(int id)
	{
		return _currentUnitIds[id];
	}

	public void NextAd()
	{
		_currentAdIdCounter++;
		if (_currentAdIdCounter >= _currentUnitIds.Length)
		{
			_currentAdIdCounter = 0;
		}
	}

	public void UseFirstAdId()
	{
		_currentAdIdCounter = 0;
	}
}
