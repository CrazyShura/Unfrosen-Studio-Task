using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionHandler : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Button button;
	[SerializeField]
	Image background;
	[SerializeField]
	TMP_Text textField;
	[SerializeField]
	Color activeColor, blockedColor, completedColor;

	Mission mission;
	MapMissionState state;
	List<int> prereqisiteMissionsIDs;
	List<int> temperaralyBlockedMissionsIDs;
	#endregion

	#region Properties
	public Mission Mission
	{
		get => mission;
		set
		{
			if (mission == null)
			{
				mission = value;
				SetLists();
				textField.text = mission.ID.ToString();
				button.onClick.AddListener(HandleClickEvent);
				CampaignManager.Instance.AddListenerForMIssionComplition(HandleMissionCompliteEvent);
			}
			else
			{
				Debug.LogWarning("Trying to assign mission to mission handler when it already has a mission assigned. This suld not happen", this);
			}
		}
	}

	public MapMissionState State
	{
		get => state;
		set
		{
			switch (value)
			{
				case MapMissionState.Active:
					button.interactable = true;
					gameObject.SetActive(true);
					background.color = activeColor;
					break;
				case MapMissionState.Hidden:
					button.interactable = false;
					gameObject.SetActive(false);
					break;
				case MapMissionState.Blocked:
					button.interactable = false;
					gameObject.SetActive(true);
					background.color = blockedColor;
					break;
				case MapMissionState.Complited:
					button.interactable = false;
					gameObject.SetActive(true);
					background.color = completedColor;
					break;
			}
			state = value;
		}
	}

	public int MissionID { get => mission.ID; }
	public IReadOnlyCollection<int> PrereqisiteMissionsIDs { get => prereqisiteMissionsIDs; }
	public IReadOnlyCollection<int> TemperaralyBlockedMissionsIDs { get => temperaralyBlockedMissionsIDs; }
	#endregion

	#region Methods
	void HandleClickEvent()
	{
		CampaignManager.Instance.HandleMissionSelection(mission.ID);
	}
	void HandleMissionCompliteEvent(int iD)
	{
		if (iD == mission.ID)
		{
			State = MapMissionState.Complited;
			CampaignManager.Instance.UnBlockMissions(temperaralyBlockedMissionsIDs);
		}
		else if(prereqisiteMissionsIDs.Contains(iD) && State != MapMissionState.Complited)
		{
			State = MapMissionState.Active;
			CampaignManager.Instance.BlockMissions(temperaralyBlockedMissionsIDs);
		}
	}
	void SetLists()
	{
		prereqisiteMissionsIDs = new List<int>();
		foreach (Mission _mission in mission.PrerequisiteMissions)
		{
			prereqisiteMissionsIDs.Add(_mission.ID);
		}
		temperaralyBlockedMissionsIDs = new List<int>();
		foreach (Mission _mission in mission.TemperaralyDisabledMissions)
		{
			temperaralyBlockedMissionsIDs.Add(_mission.ID);
		}
	}
	#endregion
}

