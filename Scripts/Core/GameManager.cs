using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityUlilities;

public class GameManager : MonoBehaviour
{
	#region Public Variables
	public GameObject counterPoints;
	public GameObject highScoreView;
	public GameObject result;
	public int menuSceneIndex;
	public float bonusDuration = 8;
	#endregion

	#region Private Variables

	HSBColor color;
	public int difficulty;

	Dictionary<TimerType, Timer> timers = new Dictionary<TimerType, Timer>();

	bool readyToColorChange;

	int points;
	int highScore;
	int amountOfBonuses;
	int stackCounter;

	GameStates gameState = GameStates.Play;
	GameModes gameMode = GameModes.Normal;
	public GameModes GameMode => gameMode;
	bool gameModeChanged;
	bool modeEnd;
	bool difficultyChange;

	private delegate void UpdateDelegate();
	private UpdateDelegate[] UpdateDelegates;

	GameViewManager view;
	EntitiesManager entities;
	DifficultyManager difficulties;
	ScoreManager scoreManager;
	AudioManager audioManager;
	Ads ads;

	#endregion

	#region Private Properties
	private void Awake()
	{
		color = HSBColor.FromRGB(Color.white);
		difficulties = GetComponent<DifficultyManager>();
		difficulties.SetDifficulty(difficulty);
		amountOfBonuses = difficulties.AmountOfBonuses;
		Vibration.Init();

		scoreManager = FindObjectOfType<ScoreManager>();
		scoreManager.Load();
		GPLeaderboard.UpdateLeaderboardScore();
		if (scoreManager.GetLongestRide() > 0)
		{
			highScore = scoreManager.GetLongestRide();
			highScoreView.GetComponent<Text>().text = highScore.ToString();
		}

		ads = FindObjectOfType<Ads>();

		audioManager = FindObjectOfType<AudioManager>();

		InitTimers();

		InitGMDelegates();

		InitEntities();

		InitGameView();
	}
	private void InitTimers()
	{
		for (int i = 0; i < (int)TimerType.Amount; i++)
			timers.Add((TimerType)i, new Timer());
		timers[TimerType.BonusDuration].OnAction = BonusDurationAction;
		timers[TimerType.BonusGap].OnAction = BonusGapAction;
		timers[TimerType.ColorChangeGap].OnAction = ColorChangeGapAction;
		timers[TimerType.StackGap].OnAction = StackGapAction;
		timers[TimerType.BetweenDiffGap].OnAction = BetweenDiffGapAction;

		timers[TimerType.BonusGap].SetParameters(difficulties.BonusGap, true);
		timers[TimerType.StackGap].SetParameters(difficulties.StackGap, true);
	}
	private void InitGMDelegates()
	{
		UpdateDelegates = new UpdateDelegate[(int)GameModes.Amount];
		UpdateDelegates[(int)GameModes.Normal] = UpdateNormalGM;
		UpdateDelegates[(int)GameModes.BlackAndWhite] = UpdateBlackAndWhiteGM;
		UpdateDelegates[(int)GameModes.ColorBoost] = UpdateColorBoostGM;
	}
	private void InitEntities()
	{
		entities = GetComponent<EntitiesManager>();
		entities.Init();
		entities.SetColor(color);
		entities.SpawnPlayer();
	}
	private void InitGameView()
	{
		view = GetComponent<GameViewManager>();
		view.audioManager = audioManager;
		view.OnViewChange += OnViewChanged;
		view.ChangeGameView(GameStates.Play);
	}

	private void Update()
	{
		switch (gameState)
		{
			case GameStates.Play:
				UpdateDelegates[(int)gameMode]?.Invoke();
				break;
			case GameStates.Pause:
				break;
			case GameStates.GameOver:
				break;
		}

		if(points >= highScore)
		{
			highScore = points;
			highScoreView.GetComponent<Text>().text = "";
		}
		else if(points > 0)
		{
			highScoreView.GetComponent<Text>().text = highScore.ToString();
		}
	}
	private void UpdateNormalGM()
	{
		if (difficultyChange)
		{
			if (entities.StacksCount > 0)
				return;
			else difficultyChange = false;
		}
		timers[TimerType.BonusGap].Tick(Time.deltaTime);
		timers[TimerType.ColorChangeGap].Tick(Time.deltaTime);
		timers[TimerType.StackGap].Tick(Time.deltaTime);
		
		if(points == 5){

		}
		if (readyToColorChange)
			readyToColorChange = entities.SpawnPaintWall(color);
	}
	private void UpdateBlackAndWhiteGM()
	{
		if (gameModeChanged)
		{
			view.InvertColorUI();
			gameModeChanged = false;
			timers[TimerType.BonusDuration].SetParameters(bonusDuration, false);
		}
		if (!modeEnd)
		{
			timers[TimerType.StackGap].Tick(Time.deltaTime);
			timers[TimerType.BonusDuration].Tick(Time.deltaTime);
		}
		else
		{
			if (entities.BlackAndWhiteModeEnd())
			{
				view.InvertColorUI(); 
				ChangeGameMode(GameModes.Normal);
				modeEnd = false;
			}
		}
	}
	private void UpdateColorBoostGM()
	{
		if (gameModeChanged)
		{
			Time.timeScale = 5;
			gameModeChanged = false;
			entities.ColorBoostModeInit();
			timers[TimerType.BonusDuration].SetParameters(bonusDuration*2, false);
		}
		if (!modeEnd)
		{
			timers[TimerType.StackGap].Tick(Time.deltaTime);
			timers[TimerType.BonusDuration].Tick(Time.deltaTime);
		}
		else
		{
			if (entities.ColorBoostModeEnd())
			{
				Time.timeScale = 1;
				gameModeChanged = true;
				gameMode = GameModes.Normal;
				modeEnd = false;
			}
		}
	}

	private void BonusDurationAction()
	{
		modeEnd = true;
	}
	private void BonusGapAction()
	{
		if (amountOfBonuses == 0)
			return;

		amountOfBonuses--;
		entities.SpawnBonus();
		timers[TimerType.BonusGap].SetParameters(difficulties.BonusGap, true);
	}
	private void ColorChangeGapAction()
	{
		HSBColor newColor;
		do
		{
			newColor = HSBColor.GetRandomColor(new FloatRange(0.0f, 1.0f), new FloatRange(0.4f, 0.9f), new FloatRange(1.0f, 1.0f));
		}
		while (Mathf.Abs(newColor.h - color.h) < 0.05);

		color = newColor;
		readyToColorChange = true;

		timers[TimerType.ColorChangeGap].SetParameters(difficulties.ColorChangeGap, true);
		//entities.SetColor(color);
	}
	private void DifficultyChangeGapAction()
	{
		difficulties.SetDifficulty(++difficulty);
		difficultyChange = true;
		amountOfBonuses = difficulties.AmountOfBonuses;
		timers[TimerType.BonusGap].SetParameters(difficulties.BonusGap, true);
	}
	private void StackGapAction()
	{
		entities.SpawnStack();
		timers[TimerType.StackGap].SetParameters(difficulties.StackGap, true);
		stackCounter++;
		if(stackCounter == difficulties.DifficultyChangeGap)
		{
			DifficultyChangeGapAction();
			stackCounter = 0;
		}
	}
	private void BetweenDiffGapAction()
	{
		difficultyChange = false;
	}
	public void ChangeGameMode(GameModes mode)
	{
		gameMode = mode;
		gameModeChanged = true;
	}
	#endregion

	#region Private Events
	
	public void OnAddPoint()
	{
		points++;
		counterPoints.GetComponent<Text>().text = points.ToString();
	}
	public void OnAddPoints(int points)
	{
		this.points+=points;
		counterPoints.GetComponent<Text>().text = this.points.ToString();
	}
	private void OnViewChanged()
	{
		switch (gameState)
		{
			case GameStates.Play:
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				break;
			case GameStates.Pause:
				break;
			case GameStates.MainMenu:
				SceneManager.LoadScene(menuSceneIndex);
				break;
			case GameStates.GameOver:
				entities.DestroyAllGOs();
				break;
		}
	}
	#endregion

	#region Public Events
	public void OnNewGame()
	{
		gameState = GameStates.Play;
		view.ChangeGameView(GameStates.Play);
		Time.timeScale = 1;
		audioManager.Reload();
		audioManager.Play(ClipType.Music);
		audioManager.Play(ClipType.Click);
        audioManager.Play(ClipType.Whoosh);

		ads.DestroyBannerAd();
	}
	public void OnGameOver()
	{
		gameState = GameStates.GameOver;
		view.ChangeGameView(GameStates.GameOver);
		result.GetComponent<Text>().text = points.ToString();
		scoreManager.SetScore(scoreManager.GetScore() + points);
		if(scoreManager.GetLongestRide() < highScore){
			scoreManager.SetLongestRide(highScore);
			scoreManager.Save();
			GPLeaderboard.UpdateLeaderboardScore();
		}

		Time.timeScale = 0;

		audioManager.StopMusic();
		audioManager.Reload();
        audioManager.Play(ClipType.Whoosh);

		ads.RequestBanner();
	}
	public void OnPause()
	{
		audioManager.Play(ClipType.Click);
		gameState = GameStates.Pause;
		Time.timeScale = 0;
	}
	public void OnResume()
	{
		audioManager.Play(ClipType.Click);
		gameState = GameStates.Play;
		Time.timeScale = 1;
	}
	public void OnMainMenu()
	{
		audioManager.Reload();
		audioManager.Play(ClipType.Music);
		audioManager.Play(ClipType.Click);
        audioManager.Play(ClipType.Whoosh);
		gameState = GameStates.MainMenu;
		view.ChangeGameView(GameStates.MainMenu);

		ads.DestroyBannerAd();
	}
	#endregion
}

public enum GameStates
{
	MainMenu,
	Play,
	Pause,
	GameOver
}

public enum GameModes 
{ 
	Normal, 
	BlackAndWhite, 
	ColorBoost, 
	Amount 
}
