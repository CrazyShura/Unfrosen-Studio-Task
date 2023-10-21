using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Image backgorund;
	[SerializeField]
	TMP_Text heroName;
	[SerializeField]
	TMP_Text heroPower;
	[SerializeField]
	Button selectButton;

	[SerializeField]
	Color selectedColor;
	[SerializeField]
	Color normalColor;

	Hero host;
	bool selected = false;
	#endregion

	#region Properties
	public Hero Host
	{
		get => host;
		set
		{
			if (host == null)
			{
				host = value;
				heroName.text = host.HeroName;
				UIUpdate(0);
				selectButton.onClick.AddListener(HandleButtonclick);
				host.AddListenerForPowerChange(UIUpdate);
				CampaignManager.Instance.AddListenerForHeroSelect(HandleSelctedEvent);
			}
		}
	}
	public bool Selected { get => selected; set => selected = value; }
	#endregion

	#region Methods
	void UIUpdate(int unused)
	{
		heroPower.text = "Power: " + host.HeroPower.ToString();
	}
	void HandleSelctedEvent(int heroID)
	{
		if (heroID == host.ID)
		{
			backgorund.color = selectedColor;
		}
		else
		{
			backgorund.color = normalColor;
		}
	}
	void HandleButtonclick()
	{
		CampaignManager.Instance.SelectNewHero(host.ID);
	}
	#endregion
}

