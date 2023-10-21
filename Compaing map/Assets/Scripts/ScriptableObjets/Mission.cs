using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UnityEngine;
[CreateAssetMenu(fileName = "New mission", menuName = "Create new mission")]
public class Mission : ScriptableObject
{
	#region Fields
	[SerializeField, Min(0), Header("Unique number that is ised to identefy mission")]
	int iD;
	[SerializeField]
	Vector2 mapPosition;

	[Space(10)]
	[Header("Mission description")]
	[SerializeField]
	string missionName;
	[Space(5)]
	[SerializeField, Multiline]
	string missionBriefing;
	[Space(5)]
	[SerializeField, Multiline]
	string missionDebriefing;
	[Space(5)]
	[SerializeField]
	List<Team> playerSide;
	[Space(5)]
	[SerializeField]
	List<Team> enemySide;

	[Space(10)]
	[Header("Mission connections")]
	[SerializeField]
	List<Mission> prerequisiteMissions;
	[Space(5)]
	[SerializeField]
	List<Mission> temperaralyDisabledMissions;
	[Space(5)]
	[SerializeField]
	List<HeroSO> heroUnlocks;
	[Space(5)]
	[SerializeField, Header("How much power will be added to hero who was selected for this mission")]
	int powerAddedToPlayer;
	[Space(5)]
	[SerializeField]
	List<HeroPowerPair> heroPowerchanges;
	#endregion

	#region Properties
	public int ID { get => iD; }
	public Vector2 MapPosition { get => mapPosition; }
	public string MissionName { get => missionName; }
	public string MissionBriefing { get => missionBriefing; }
	public string MissionDebriefing { get => missionDebriefing; }
	public ReadOnlyCollection<Team> PlayerSide { get => new ReadOnlyCollection<Team>(playerSide); }
	public ReadOnlyCollection<Team> EnemySide { get => new ReadOnlyCollection<Team>(enemySide); }
	public ReadOnlyCollection<Mission> PrerequisiteMissions { get => new ReadOnlyCollection<Mission>(prerequisiteMissions); }
	public ReadOnlyCollection<Mission> TemperaralyDisabledMissions { get => new ReadOnlyCollection<Mission>(temperaralyDisabledMissions); }
	public ReadOnlyCollection<HeroSO> HeroUnlocks { get => new ReadOnlyCollection<HeroSO>(heroUnlocks); }
	public int PowerAddedToPlayer { get => powerAddedToPlayer; }
	public ReadOnlyCollection<HeroPowerPair> HeroPowerchanges { get => new ReadOnlyCollection<HeroPowerPair>(heroPowerchanges); }
	#endregion

	#region Methods

	#endregion
}

