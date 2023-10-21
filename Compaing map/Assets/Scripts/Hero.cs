using UnityEngine.Events;

public class Hero
{
	#region Fields
	int iD;
	string heroName;
	Team side;
	int heroPower;

	UnityEvent<int> powerChanged = new UnityEvent<int>();
	#endregion

	#region Properties
	public int ID { get => iD; }
	public string HeroName { get => heroName; }
	public Team Side { get => side; }
	public int HeroPower { get => heroPower; }
	#endregion

	#region Methods
	public Hero(HeroSO heroSO)
	{
		iD = heroSO.ID;
		heroName = heroSO.HeroName;
		side = heroSO.Side;
		heroPower = heroSO.HeroPower;
	}
	public void ChangePower(int amount)
	{
		heroPower += amount;
		powerChanged.Invoke(heroPower);
	}
	public void AddListenerForPowerChange(UnityAction<int> action)
	{
		powerChanged.AddListener(action);
	}
	#endregion
}

