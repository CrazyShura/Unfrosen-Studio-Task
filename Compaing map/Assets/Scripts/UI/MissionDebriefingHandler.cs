using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDebriefingHandler : MonoBehaviour
{
	#region Fields
	[SerializeField]
	TMP_Text missionName;
	[SerializeField]
	TMP_Text missionDebriefing;
	[SerializeField]
	TMP_Text playerSide;
	[SerializeField]
	TMP_Text enemySide;
	[SerializeField]
	Button completeMissionButton;

	int missionID;
	#endregion

	#region Properties

	#endregion

	#region Methods
	public void DisplayDebriefing(Mission mission)
	{
		missionName.text = mission.MissionName;
		missionDebriefing.text = mission.MissionDebriefing;
		string _temp = "";
		foreach (Team _team in mission.PlayerSide)
		{
			switch (_team)
			{
				case Team.NoTeam:
					_temp += "Нет гнезда ";
					break;
				case Team.Jackdaws:
					_temp += "Галки ";
					break;
				case Team.Jays:
					_temp += "Cойки ";
					break;
				case Team.Sparrows:
					_temp += "Воробьи ";
					break;
				case Team.Eagles:
					_temp += "Орлы ";
					break;
				case Team.Seagulls:
					_temp += "Чайки ";
					break;
				case Team.Owls:
					_temp += "Совы ";
					break;
				case Team.Crows:
					_temp += "Вороны ";
					break;
				case Team.Phoenixes:
					_temp += "Фениксы ";
					break;
			}
		}
		playerSide.text = _temp;
		_temp = string.Empty;
		foreach (Team _team in mission.EnemySide)
		{
			switch (_team)
			{
				case Team.NoTeam:
					_temp += "Нет гнезда ";
					break;
				case Team.Jackdaws:
					_temp += "Галки ";
					break;
				case Team.Jays:
					_temp += "Cойки ";
					break;
				case Team.Sparrows:
					_temp += "Воробьи ";
					break;
				case Team.Eagles:
					_temp += "Орлы ";
					break;
				case Team.Seagulls:
					_temp += "Чайки ";
					break;
				case Team.Owls:
					_temp += "Совы ";
					break;
				case Team.Crows:
					_temp += "Вороны ";
					break;
				case Team.Phoenixes:
					_temp += "Фениксы ";
					break;
			}
		}
		enemySide.text = _temp;
		completeMissionButton.onClick.RemoveAllListeners();
		missionID = mission.ID;
		completeMissionButton.onClick.AddListener(HandleOnClickEvent);
	}
	void HandleOnClickEvent()
	{
		CampaignManager.Instance.HandleMissionComplition(missionID);
	}
	#endregion
}
