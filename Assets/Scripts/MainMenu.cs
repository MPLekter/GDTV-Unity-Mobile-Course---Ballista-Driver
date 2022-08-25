using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;

    [SerializeField] private AndroidNotificationsHandler androidNotificationsHandler;

    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeMinutes;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";


    private void Start()
    {
        checkHighscore();
        loadEnergy();
        setEnergyText();
    }

    private void setEnergyText()
    {
        energyText.text = $"PLAY ({energy})";
    }

    private void loadEnergy()
    {
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        if(energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if(energyReadyString == string.Empty) { return; }

            DateTime energyReady = DateTime.Parse(energyReadyString);

            if(DateTime.Now > energyReady) // energyReady is a date when energy was restored. So if now is later, then your energy is maxed again.
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else //if it's not ready yet, just call the function again after the correct time. Works when app is still on.
            {
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        }
    }
    
    private void EnergyRecharged()
    {
        energy = maxEnergy;
        Start();
    }
    private void checkHighscore()
    {
        int highScore = PlayerPrefs.GetInt(ScoreHandler.HighScoreKey, 0);
        highScoreText.text = $"High Score: {highScore}"; //this is how you make a string + some value in it. works like high score + something
    }

    public void Play()
    {
        if (energy <= 0) {return;} //you can only play if there is energy left.
        DecreaseEnergy();
        SceneManager.LoadScene(1);
    }

    private void DecreaseEnergy()
    {
        energy -=1;
        PlayerPrefs.SetInt(EnergyKey, energy);
        if (energy <= 0) //if the energy is gone, it's time to setup a new date when it will be restored to the max value.
        {
            SetNewEnergyReadyTime(); 
        }

    }

    private void SetNewEnergyReadyTime()
    {
        DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeMinutes); //
        PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
        androidNotificationsHandler.ScheduleNotification(energyReady);
#endif
    }
}
