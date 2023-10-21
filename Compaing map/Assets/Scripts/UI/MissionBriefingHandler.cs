using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionBriefingHandler : MonoBehaviour
{
	#region Fields
	[SerializeField]
	TMP_Text missionName;
	[SerializeField]
	TMP_Text missionBriefing;
	[SerializeField]
	Button startMissionButton;

	int missionID;
	#endregion

	#region Properties

	#endregion

	#region Methods
	public void DisplayBriefing(Mission mission)
	{
		missionName.text = mission.MissionName;
		missionBriefing.text = mission.MissionBriefing;
		startMissionButton.onClick.RemoveAllListeners();
		missionID = mission.ID;
		startMissionButton.onClick.AddListener(HandleOnClickEvent);
	}

	void HandleOnClickEvent()
	{
		CampaignManager.Instance.HandleMissionStart(missionID);
	}
	#endregion
}

