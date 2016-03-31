using UnityEngine;

public class PreferenceManager : MonoBehaviour
{

	void Awake()
	{
		// Axe DT Vampirism
		if (!PlayerPrefs.HasKey(PerkManager.axe_dtVampirism_name))
			PlayerPrefs.SetInt(PerkManager.axe_dtVampirism_name, 0);
		PerkManager.axe_dtVampirism_unlocked = PlayerPrefs.GetInt(PerkManager.axe_dtVampirism_name) == 1;

        // Axe BFA
        if (!PlayerPrefs.HasKey(PerkManager.axe_bfa_name))
            PlayerPrefs.SetInt(PerkManager.axe_bfa_name, 0);
        PerkManager.axe_bfa_unlocked = PlayerPrefs.GetInt(PerkManager.axe_bfa_name) == 1;

        // Trinket Aggression Buddy 
        if (!PlayerPrefs.HasKey(PerkManager.trinket_agressionBuddy_name))
			PlayerPrefs.SetInt(PerkManager.trinket_agressionBuddy_name, 0);
		PerkManager.trinket_agressionBuddy_unlocked = PlayerPrefs.GetInt(PerkManager.trinket_agressionBuddy_name) == 1;

        // Bear Hands
        if (!PlayerPrefs.HasKey(PerkManager.hat_bearHands_name))
            PlayerPrefs.SetInt(PerkManager.hat_bearHands_name, 0);
        PerkManager.hat_bearHands_unlocked = PlayerPrefs.GetInt(PerkManager.hat_bearHands_name) == 1;

		// Trinket Mary's Todds Lockette
		if (!PlayerPrefs.HasKey(PerkManager.trinket_maryToddsLockette_name))
			PlayerPrefs.SetInt(PerkManager.trinket_maryToddsLockette_name, 0);
		PerkManager.trinket_maryToddsLockette_unlocked = PlayerPrefs.GetInt(PerkManager.trinket_maryToddsLockette_name) == 1;
    }

}