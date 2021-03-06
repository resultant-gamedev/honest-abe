﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{

    public static bool cutsceneActive; // Whether or not we're currently playing a cutscene
    public int index; // The "index" of the cutscene, i.e. which part of the cutscene we are currently in if it's multi-part

    // The list of our cutscenes in the game (there's three)
    public enum Cutscenes
    {
        NULL,
        INTRO,
        BEAR,
        MID,
        END
    }
    public Cutscenes currentCutscene; // The cutscene that we're currently playing


    private string[] _introText = {
        "Abe awakes, a chilling memory gradually coming back to him... A romantic carriage ride with Mary Todd… A confederate ambush...",
        "General Robert E Lee himself had stepped out of the shadows and aimed a pistol at Abe’s face...",
        "The last thing Abe can remember is the sound of that gunshot."
    };

    private string[] _bearText =
    {
        "Finally, Abe lumbers out of the forest they had tried to bury him in.",
        "It seems this battle has only just begun."
    };

    private string[] _endText = {
        "With a sliver of hope that Mary Todd may yet be alive, Abe wanders off in search of his beloved, his bloody axe hungering for the next battle."
    };

    private bool _cutsceneOver, _allowSkip;
    private float timeToAllowSkip = 2f, timer = 0f;

    private GameObject _cutsceneCanvas; // The canvas object that is used for all the cutscenes
    private GameObject _introStoryPanel, _bearStoryPanel, _midStoryPanel, _endStoryPanel, _skipText;
    private Text _introStoryText, _bearStoryText, _endStoryText;
    private Image _midStoryImage;

    private Letterbox _letterbox;

    private GameObject _player, _ui;

    void Start()
    {
        _cutsceneCanvas = GameObject.Find("CutsceneCanvas");

        _introStoryPanel = GameObject.Find("IntroCutscenePanel");
        _introStoryText = _introStoryPanel.transform.Find("Text").GetComponent<Text>();
        _introStoryText.text = _introText[0];
        _introStoryPanel.SetActive(false);

        _bearStoryPanel = GameObject.Find("BearCutscenePanel");
        _bearStoryText = _bearStoryPanel.transform.Find("Text").GetComponent<Text>();
        _bearStoryText.text = _bearText[0];
        _bearStoryPanel.SetActive(false);

        _midStoryPanel = GameObject.Find("MidCutscenePanel");
        _midStoryImage = _midStoryPanel.transform.Find("Image").GetComponent<Image>();
        _midStoryPanel.SetActive(false);

        _endStoryPanel = GameObject.Find("EndCutscenePanel");
        _endStoryText = _endStoryPanel.transform.Find("Text").GetComponent<Text>();
        _endStoryText.text = _endText[0];
        _endStoryPanel.SetActive(false);

        _skipText = GameObject.Find("SkipText");
        _skipText.SetActive(false);
        _allowSkip = false;

        _cutsceneOver = false;
        cutsceneActive = false;
        index = 0;

        _letterbox = Camera.main.GetComponent<Letterbox>();

        _player = GameObject.Find("Player");
        _ui = GameObject.Find("UI");

        ChangeCutscene(Cutscenes.NULL);
    }

    void Update()
    {
        if (currentCutscene == Cutscenes.NULL)
            return;

        timer += Time.deltaTime;

        if (timer >= timeToAllowSkip)
        {
            _skipText.SetActive(true);
            _allowSkip = true;
        }

        if ((Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape)) && Time.timeScale > 0 && _allowSkip)
        {
            index++;
            timer = 0;
            _skipText.SetActive(false);
            _allowSkip = false;

            if (currentCutscene == Cutscenes.INTRO)
            {
                if (index >= _introText.Length)
                {
                    _cutsceneOver = true;
                    Invoke("ShowHUD", 1);
                    Invoke("ShowPickupText", 1);
                }
                else if (index < _introText.Length)
                {
                    _introStoryText.text = _introText[index];
                }
            }
            else if (currentCutscene == Cutscenes.BEAR)
            {
                if (index >= _bearText.Length)
                {
                    Invoke("ShowHUD", 1);
                    Invoke("NextLevel", 1);
                    _cutsceneOver = true;
                }
                else if (index < _bearText.Length)
                {
                    _bearStoryText.text = _bearText[index];
                }
            }
            else if (currentCutscene == Cutscenes.MID)
            {
                Invoke("ShowHUD", 1);
                Invoke("NextLevel", 1);
                _cutsceneOver = true;
            }
            else if (currentCutscene == Cutscenes.END)
            {
                if (index >= _endText.Length)
                {
                    Invoke("ShowWinScreen", 1);
                    _cutsceneOver = true;
                }
                else if (index < _endText.Length)
                {
                    _endStoryText.text = _endText[index];
                }
            }
        }

        if (_cutsceneOver)
            EndCutscene();
    }

    public void ChangeCutscene(Cutscenes cutscene)
    {
        ResetCutscenes();
        currentCutscene = cutscene;
        _cutsceneCanvas.SetActive(true);

        switch (currentCutscene)
        {
            case Cutscenes.INTRO:
                cutsceneActive = true;
                _player.GetComponent<Cinematic>().cinematic = "Abe Rises";
                _player.GetComponent<Cinematic>().enabled = true;
                _player.GetComponent<PlayerHealth>().RefillForCutscene();
                _ui.GetComponent<UIManager>().hudCanvas.SetActive(false);
                _letterbox.TweenTo(0.15f, 1);
				Invoke("ShowIntroPanel", 1);
                break;
            case Cutscenes.BEAR:
                cutsceneActive = true;
                GameObject.Find("Blood Drip").SetActive(false);
                _player.GetComponent<PlayerHealth>().RefillForCutscene();
                _ui.GetComponent<UIManager>().hudCanvas.SetActive(false);
                _ui.GetComponent<UIManager>().bossHealthUI.enabled = false;
                _letterbox.TweenTo(0.15f, 1);
                Invoke("ShowBearPanel", 1);
                break;
            case Cutscenes.MID:
                cutsceneActive = true;
                GameObject.Find("Blood Drip").SetActive(false);
                _player.GetComponent<PlayerHealth>().RefillForCutscene();
                _ui.GetComponent<UIManager>().hudCanvas.SetActive(false);
                _ui.GetComponent<UIManager>().bossHealthUI.enabled = false;
                _letterbox.TweenTo(0.15f, 1);
                Invoke("ShowMidPanel", 1);
                break;
            case Cutscenes.END:
                cutsceneActive = true;
                GameObject.Find("Blood Drip").SetActive(false);
                _player.GetComponent<Player>().PlayEnding();
                _player.GetComponent<PlayerHealth>().RefillForCutscene();
                _ui.GetComponent<UIManager>().hudCanvas.SetActive(false);
                _ui.GetComponent<UIManager>().bossHealthUI.enabled = false;
                _letterbox.TweenTo(0.15f, 1);
                Invoke("ShowEndPanel", 1);
                break;
            case Cutscenes.NULL:
                cutsceneActive = false;
                _cutsceneCanvas.SetActive(false);
                break;
        }
    }

    private void EndCutscene()
    {
        ChangeCutscene(Cutscenes.NULL);
        _letterbox.TweenTo(0, 1);
        _cutsceneOver = false;
    }

    private void ResetCutscenes()
    {
        index = 0;
        timer = 0;
        _allowSkip = false;

        _introStoryPanel.SetActive(false);
        _introStoryText.text = _introText[index];

        _bearStoryPanel.SetActive(false);
        _bearStoryText.text = _bearText[index];

        _midStoryPanel.SetActive(false);

        _endStoryPanel.SetActive(false);
        _endStoryText.text = _endText[index];
    }

    private void ShowPickupText()
    {
        GameObject.Find("GameManager").GetComponent<PerkManager>().showInstructions = true;
    }

    private void ShowHUD()
    {
        _ui.GetComponent<UIManager>().hudCanvas.SetActive(true);
        _letterbox.Amount = 0;
    }

    private void NextLevel()
    {
        EventHandler.SendEvent(EventHandler.Events.LEVEL_NEXT);
    }

    private void ShowWinScreen()
    {
        _ui.GetComponent<UIManager>().WinUI.SetActive(true);
    }

    private void ShowIntroPanel()
    {
        _introStoryPanel.SetActive(true);
    }

    private void ShowBearPanel()
    {
        _bearStoryPanel.SetActive(true);
    }

    private void ShowMidPanel()
    {
        _midStoryPanel.SetActive(true);
    }

    private void ShowEndPanel()
    {
        _endStoryPanel.SetActive(true);
    }
}
