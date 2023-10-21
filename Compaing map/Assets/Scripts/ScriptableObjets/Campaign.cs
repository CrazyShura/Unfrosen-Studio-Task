using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New campaign", menuName = "Create new campaign")]
public class Campaign : ScriptableObject
{
	#region Fields
	[SerializeField]
	List<Mission> missions;
	[SerializeField]
	List<HeroSO> startingHeroes;
	#endregion

	#region Properties
	public ReadOnlyCollection<Mission> Missions { get => new ReadOnlyCollection<Mission>(missions); }
	public ReadOnlyCollection<HeroSO> StartingHeroes { get => new ReadOnlyCollection<HeroSO>(startingHeroes); }
	#endregion

	#region Methods
	/// <summary>
	/// Checks if the campaign is valid for use. Has start mission, does not have a duplicate mission and every mission dependency is met.
	/// </summary>
	/// <param name="campaign">Campaign to check</param>
	/// <returns>True if campaign does meet all the reqirements and false if not.</returns>
	public static bool ValidateCampaign(Campaign campaign)
	{
		bool _campaignPossible = true;
		List<int> _missionIDList = new List<int>();
		List<int> _alternetiveMissonsIDList = new List<int>();
		if(campaign.startingHeroes.Count != campaign.startingHeroes.Distinct().Count())
		{
			Debug.LogWarning("Capmaing has a duplicate starting heroes.This suld not happen, the duplicates will be skipped.");
		}
		foreach (Mission _mission in campaign.Missions)
		{
			if (_missionIDList.Contains(_mission.ID))
			{
				Debug.LogError($"Trying to create a campaign with duplicate <color=yellow>{_mission.name}</color> mission.");
				_campaignPossible = false;
			}
			else
			{
				_missionIDList.Add(_mission.ID);
			}
			if (_mission is DoubleMission)
			{
				DoubleMission _doubleMission = (DoubleMission)_mission;
				if (_missionIDList.Contains(_doubleMission.AlternetiveMisson.ID))
				{
					Debug.LogError($"Trying to create a campaign with duplicate <color=yellow>{_mission.name}</color> mission.");
					_campaignPossible = false;
				}
				else
				{
					_missionIDList.Add(_doubleMission.AlternetiveMisson.ID);
					_alternetiveMissonsIDList.Add(_doubleMission.AlternetiveMisson.ID);
				}
			}
		}
		bool _hasStartingMission = false;
		foreach (Mission _mission in campaign.Missions)
		{
			if (_mission.PrerequisiteMissions.Count == 0)
			{
				if (!_alternetiveMissonsIDList.Contains(_mission.ID))
				{
					_hasStartingMission = true;
				}
			}
			else
			{
				foreach (Mission _prerequsite in _mission.PrerequisiteMissions)
				{
					if (!_missionIDList.Contains(_prerequsite.ID))
					{
						Debug.LogError($"Trying to create a campaign with mission that cannot be unlocked.<color=yellow>{_mission.name}</color> mission requires <color=yellow>{_prerequsite.name}</color> mission to be in the campaign");
						_campaignPossible = false;
					}
				}
				foreach (Mission _disaibledMission in _mission.TemperaralyDisabledMissions)
				{
					if (!_missionIDList.Contains(_disaibledMission.ID))
					{
						Debug.LogError($"Trying to create a campaign with mission that blocks mission that is not in campaign.<color=yellow>{_mission.name}</color> mission requires <color=yellow>{_disaibledMission.name}</color> mission to be in the campaign");
						_campaignPossible = false;
					}
				}
			}
		}
		if (!_hasStartingMission)
		{
			Debug.LogError("Campaign does not have a starting mission (a mission wiht no prerequsite missions)");
			_campaignPossible = false;
		}
		return _campaignPossible;
	}
	#endregion
}

