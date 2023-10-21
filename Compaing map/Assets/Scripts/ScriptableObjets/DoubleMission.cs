using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New double mission", menuName = "Create new double mission")]
public class DoubleMission : Mission
{
	#region Fields
	[Space(10)]
	[SerializeField, Header("Alternetive mission")]
	Mission alternetiveMisson;
	#endregion

	#region Properties
	public Mission AlternetiveMisson { get => alternetiveMisson;}
	#endregion

	#region Methods

	#endregion
}

