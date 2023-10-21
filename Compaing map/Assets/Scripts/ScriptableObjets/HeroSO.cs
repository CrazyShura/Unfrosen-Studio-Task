using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New hero", menuName = "Create new hero")]
public class HeroSO : ScriptableObject
{
	#region Fields
	[SerializeField]
	int iD;
	[SerializeField]
	string heroName;
	[SerializeField]
	Team side;
	[SerializeField]
	int heroPower;
	#endregion

	#region Properties
	public int ID { get => iD; }
	public string HeroName { get => heroName; }
	public Team Side { get => side; }
	public int HeroPower { get => heroPower; }
	#endregion

	#region Methods

	#endregion
}

