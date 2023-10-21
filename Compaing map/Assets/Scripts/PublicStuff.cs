using System;
using System.Runtime.CompilerServices;

public static class PublicStuff
{
	#region Fields

	#endregion

	#region Properties

	#endregion

	#region Methods

	#endregion
}

[System.Serializable]
public class HeroPowerPair
{
	public HeroSO hero; //HACK: i dont like leaving public fields but its the only way to let it be seen in inspector :<
	public int power;
	public int HeroID { get => hero.ID; }
}

[System.Serializable]
public enum Team { NoTeam, Jackdaws, Jays, Sparrows, Eagles, Seagulls, Owls, Crows, Phoenixes }
[System.Serializable]
public enum MapMissionState { Active, Hidden, Blocked, Complited }