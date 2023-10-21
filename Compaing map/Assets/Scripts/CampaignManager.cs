using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton. Connects and tells other objects how to behave.
/// </summary>
public class CampaignManager : MonoBehaviour
{
	#region Fields
	static CampaignManager instance;

	List<UIMissionHandler> missionHandlers = new List<UIMissionHandler>();
	Dictionary<int, UIMissionHandler> handlerDictionary = new Dictionary<int, UIMissionHandler>();
	Dictionary<int, HeroCard> heroCardDictionary = new Dictionary<int, HeroCard>();
	Dictionary<int, Hero> heroDictionary = new Dictionary<int, Hero>();
	Hero currentlySelectedHero;
	UnityEvent<int> missionCompletedEvent = new UnityEvent<int>();
	UnityEvent<int> heroSelectedEvent = new UnityEvent<int>();

	[Space(5)]
	[SerializeField]
	UIMissionHandler missionObject;
	[Space(5)]
	[SerializeField]
	UIMissionHandler doubleMissionObject;
	[Space(5)]
	[SerializeField]
	HeroCard heroCardObject;
	[Space(5)]
	[SerializeField]
	Campaign currentCampaign;
	[Space(5)]
	[SerializeField]
	Transform mapParent;
	[Space(5)]
	[SerializeField]
	Transform heroCardsParent;

	[Space(5)]
	[Header("UI stuff")]
	[Space(5)]
	[SerializeField]
	MissionBriefingHandler briefingHandler;
	[Space(5)]
	[SerializeField]
	MissionBriefingHandler alternativeBriefingHandler;
	[Space(5)]
	[SerializeField]
	MissionDebriefingHandler debriefingHandler;
	#endregion

	#region Properties
	public static CampaignManager Instance { get => instance; }
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.LogWarning("Trying to create more the one instance of campaing manager. This suld not happen as this is a singlton.");
			Destroy(gameObject);
			return;
		}
	}

	private void Start()
	{
		if (!Campaign.ValidateCampaign(currentCampaign))
		{
			Debug.LogError("Could not create a map due to missing dependencies in the campaign", this);
			return;
		}
		foreach (HeroSO _heroSO in currentCampaign.StartingHeroes)
		{
			if (!heroDictionary.ContainsKey(_heroSO.ID))
			{
				Hero _hero = new Hero(_heroSO);
				heroDictionary.Add(_hero.ID, _hero);
				HeroCard _heroCard = Instantiate<HeroCard>(heroCardObject, heroCardsParent);
				_heroCard.Host = _hero;
				heroCardDictionary.Add(_hero.ID, _heroCard);
			}
		}
		foreach (Mission _mission in currentCampaign.Missions)
		{
			foreach (HeroSO _heroSO in _mission.HeroUnlocks)
			{
				if (!heroDictionary.ContainsKey(_heroSO.ID))
				{
					Hero _hero = new Hero(_heroSO);
					heroDictionary.Add(_hero.ID, _hero);
				}
			}
			UIMissionHandler _handler = Instantiate<UIMissionHandler>(missionObject, mapParent);
			_handler.transform.localPosition = _mission.MapPosition;
			_handler.Mission = _mission;
			if (_mission.PrerequisiteMissions.Count == 0)
			{
				_handler.State = MapMissionState.Active;
			}
			else
			{
				_handler.State = MapMissionState.Hidden;
			}
			missionHandlers.Add(_handler);
			handlerDictionary.Add(_handler.MissionID, _handler);
			if (_mission is DoubleMission)
			{
				DoubleMission _doubleMission = (DoubleMission)_mission;
				UIMissionHandler _alternativeHandler = Instantiate<UIMissionHandler>(missionObject, mapParent);
				_alternativeHandler.transform.localPosition = _mission.MapPosition;
				_alternativeHandler.Mission = _doubleMission.AlternetiveMisson;
				_alternativeHandler.State = MapMissionState.Hidden;
				missionHandlers.Add(_alternativeHandler);
				handlerDictionary.Add(_alternativeHandler.MissionID, _alternativeHandler);
			}
		}
	}

	//HACK: These methods are a hack to avoid an event manager system but since its a small demo project I think its jastified.
	/// <summary>
	/// Makes brifeing to appear for selected mission from the campaign.
	/// </summary>
	/// <param name="missionID">Selected mission ID</param>
	public void HandleMissionSelection(int missionID)
	{
		HideAllUI();
		Mission _mission = handlerDictionary[missionID].Mission;
		briefingHandler.DisplayBriefing(_mission);
		briefingHandler.gameObject.SetActive(true);
		heroCardsParent.gameObject.SetActive(true);
		if (_mission is DoubleMission)
		{
			DoubleMission _temp = (DoubleMission)_mission;
			alternativeBriefingHandler.DisplayBriefing(_temp.AlternetiveMisson);
			alternativeBriefingHandler.gameObject.SetActive(true);
		}
	}
	/// <summary>
	/// Makes debriefing to appear for selected mission from a campaign.
	/// </summary>
	/// <param name="missionID">Started mission ID</param>
	public void HandleMissionStart(int missionID) //TODO : since there is no actuall missions, debriefing is basicly the only thing that happesn after mission start so thats why there is a bit of confusion in names. This might need to be cleaned up.
	{
		if (currentlySelectedHero != null)
		{
			HideAllUI();
			Mission _mission = handlerDictionary[missionID].Mission;
			debriefingHandler.DisplayDebriefing(_mission);
			debriefingHandler.gameObject.SetActive(true);
		}
		else
		{
			Debug.Log("Trying tor start a mission wihotut a hero selected");
		}
	}
	/// <summary>
	/// Hides all UI and invokes the corresponding event.
	/// </summary>
	/// <param name="missionID">Completed mission ID</param>
	public void HandleMissionComplition(int missionID)
	{
		Mission _mission = handlerDictionary[missionID].Mission;
		currentlySelectedHero.ChangePower(_mission.PowerAddedToPlayer);
		foreach (HeroPowerPair _heroPowerPair in _mission.HeroPowerchanges)
		{
			heroDictionary[_heroPowerPair.HeroID].ChangePower(_heroPowerPair.power);
		}
		foreach (HeroSO _heroSO in _mission.HeroUnlocks)
		{
			if (!heroCardDictionary.ContainsKey(_heroSO.ID))
			{
				HeroCard _heroCard = Instantiate<HeroCard>(heroCardObject, heroCardsParent);
				_heroCard.Host = heroDictionary[_heroSO.ID];
				heroCardDictionary.Add(_heroSO.ID, _heroCard);
			}
		}
		HideAllUI();
		missionCompletedEvent.Invoke(missionID);
	}
	public void BlockMissions(List<int> missionIDs)
	{
		foreach (UIMissionHandler _handler in GetMissionHandlerByID(missionIDs))
		{
			if (_handler.State == MapMissionState.Active)
			{
				_handler.State = MapMissionState.Blocked;
			}
		}
	}
	//TODO : Unblocking missions does not take in consideration the possobility that a mission can be blocked by two separate missions. This could be a problem.
	public void UnBlockMissions(List<int> missionIDs)
	{
		foreach (UIMissionHandler _handler in GetMissionHandlerByID(missionIDs))
		{
			if (_handler.State == MapMissionState.Blocked)
			{
				_handler.State = MapMissionState.Active;
			}
		}
	}


	public void SelectNewHero(int heroID)
	{
		if (currentlySelectedHero != null && currentlySelectedHero.ID == heroID)
		{
			return;
		}
		currentlySelectedHero = heroDictionary[heroID];
		heroSelectedEvent.Invoke(heroID);
	}
	public void AddListenerForMIssionComplition(UnityAction<int> action)
	{
		missionCompletedEvent.AddListener(action);
	}
	public void AddListenerForHeroSelect(UnityAction<int> action)
	{
		heroSelectedEvent.AddListener(action);
	}
	List<UIMissionHandler> GetMissionHandlerByID(List<int> missionIDs)
	{
		List<UIMissionHandler> _handlers = new List<UIMissionHandler>();
		foreach (int _iD in missionIDs)
		{
			_handlers.Add(handlerDictionary[_iD]);
		}
		return _handlers;
	}

	void HideAllUI()
	{
		briefingHandler.gameObject.SetActive(false);
		alternativeBriefingHandler.gameObject.SetActive(false);
		debriefingHandler.gameObject.SetActive(false);
		heroCardsParent.gameObject.SetActive(false);
	}
	#endregion
}

