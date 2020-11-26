using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUlilities;

public class EntitiesManager : MonoBehaviour
{
	public GameObject box;
	public GameObject stack;
	public GameObject paintWall;
	public GameObject player;
	public GameObject bonus;
	public GameObject stars;
	public GameObject indicator;

	List<GameObject> stacks = new List<GameObject>();

	GameManager game;
	GameViewManager viewManager;
	DifficultyManager difficulties;
	PlayerSkinManager skinManager;
	AudioManager audioManager;
	SettingsManager settingsManager;
	bool isFirst = true;

	bool bwMode = true;
	HSBColor color, newColor;
	HSBColor remindColor;
	FloatRange bonusSpeed = new FloatRange(2.0f, 3.0f);

	public void Init()
	{
		game = GetComponent<GameManager>();
		viewManager = GetComponent<GameViewManager>();
		difficulties = GetComponent<DifficultyManager>();
		skinManager = FindObjectOfType<PlayerSkinManager>();
		viewManager.OnInvertColors = OnInvertColors;
		audioManager = FindObjectOfType<AudioManager>();
		settingsManager = FindObjectOfType<SettingsManager>();
	}
	public int StacksCount => stacks.Count;
	public void SetColor(HSBColor _color) { color = _color; }
	public void SpawnPlayer()
	{
		player = Instantiate(player);
		Player pl = player.GetComponent<Player>();
		pl.Init();
		pl.SetSkin(skinManager.GetSkin());
		pl.OnDie += OnPlayerDie;
		pl.OnAddPoint += OnAddPoint;
		pl.OnBonusTake += OnBonusTake;
		pl.OnEnterBox += OnPlayerEnterToBox;
		pl.OnPaintWallEnter += OnPlayerEnterToPaintWall;
	}
	public void SpawnStack()
	{
		stacks.Add(Instantiate(stack));
		stacks[stacks.Count - 1].GetComponent<Stack>().Init(box, color, difficulties.StackSpeed, difficulties.ColorGap, difficulties.AmountOfBoxes);
		
		if (isFirst)
		{
			stacks[0].GetComponent<Stack>().onDestroy += OnStackDestroy;
			isFirst = false;
		}
	}
	public bool SpawnPaintWall(HSBColor _color)
	{
		newColor = _color;
		if (stacks.Count == 1)
		{
			Stack otherColorStack = stacks[0].GetComponent<Stack>();
			if (otherColorStack.GetLowestPosition().y - player.transform.position.y > 3.0f)
			{
				Instantiate(paintWall).GetComponent<PaintWall>().Init(CalculatePaintWallSpeed(otherColorStack), newColor);
				SetColor(newColor);
				return false;
			}
		}
		else if (stacks.Count > 1)
		{
			Stack colorStack = stacks[stacks.Count - 2].GetComponent<Stack>();
			Stack otherColorStack = stacks[stacks.Count - 1].GetComponent<Stack>();

			if (colorStack.GetHighestPosition().y < player.transform.position.y)
				Instantiate(paintWall).GetComponent<PaintWall>().Init(CalculatePaintWallSpeed(otherColorStack), newColor);
			else
				Instantiate(paintWall).GetComponent<PaintWall>().Init(CalculatePaintWallSpeed(colorStack, otherColorStack), newColor);
			SetColor(newColor);
			return false;
		}
		return true;
	}
	private float CalculatePaintWallSpeed(Stack otherColorStack)
	{
		float distanceFromStackToPlayer = otherColorStack.GetLowestPosition().y - player.transform.position.y;
		float timeFromStackToPlayer = distanceFromStackToPlayer / otherColorStack.GetSpeed;
		float distanceFromSpawnToPlayer = ((ScreenHelper.screenHeight / 2) + 0.5f) - player.transform.position.y;
		float paintWallSpeed = distanceFromSpawnToPlayer / (timeFromStackToPlayer / 1.5f);
		return paintWallSpeed;
	}
	private float CalculatePaintWallSpeed(Stack colorStack, Stack otherColorStack)
	{
		float distanceFromColorStackToPlayer = colorStack.GetLowestPosition().y - player.transform.position.y;
		float timeFromColorStackToPlayer = distanceFromColorStackToPlayer / colorStack.GetSpeed;

		float distanceFromOtherColorStackToPlayer = otherColorStack.GetLowestPosition().y - player.transform.position.y;
		float timeFromOtherColorStackToPlayer = distanceFromOtherColorStackToPlayer / otherColorStack.GetSpeed;

		float distanceFromSpawnToPlayer = (ScreenHelper.screenHeight / 2) - player.transform.position.y;
		float paintWallTime = timeFromOtherColorStackToPlayer - (timeFromOtherColorStackToPlayer - timeFromColorStackToPlayer) / 2;
		float paintWallSpeed = distanceFromSpawnToPlayer / paintWallTime;
		return paintWallSpeed;
	}

	public void SpawnBonus()
	{
		Instantiate(bonus).GetComponent<Bonus>().Init(bonusSpeed.Random, (BonusType)new IntRange(0, 2).Random); 
	}
	public void DestroyAllGOs()
	{
		while (stacks.Count > 0)
		{
			stacks[0].GetComponent<Stack>().DestroyStack();
		}
		Destroy(player);
	}
	public void BlackAndWhiteModeInit()
	{
		audioManager.ChangeMusicPitch(0.5f);
		remindColor = color;
		Camera.main.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1f);
		stars.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Color.black);
		player.GetComponent<Player>().SetColor(HSBColor.FromRGB(Color.black));
		SetColor(HSBColor.FromRGB(Color.black));

		foreach (GameObject go in stacks)
			go.GetComponent<Stack>().PaintToBlackAndWhite();
		stacks[0].GetComponent<Stack>().DestroyStack();
	}
	public bool BlackAndWhiteModeEnd()
	{
		if(stacks.Count == 0)
		{
			audioManager.ChangeMusicPitch(1.0f);
			Camera.main.backgroundColor = new Color(0.14f, 0.14f, 0.14f, 1f);
			stars.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Color.white);
			player.GetComponent<Player>().SetColor(remindColor);
			SetColor(remindColor);
			return true;
		}
		return false;
	}
	public void ColorBoostModeInit()
	{
		player.GetComponent<Player>().ColorBoostMode();
		audioManager.ChangeMusicPitch(3f);
	}
	public bool ColorBoostModeEnd()
	{
		if(stacks.Count == 0)
		{
			player.GetComponent<Player>().ColorBoostMode();
			audioManager.ChangeMusicPitch(1.0f);
			return true;
		}
		return false;
	}

	private void OnPlayerEnterToPaintWall()
	{
		audioManager.Play(ClipType.PaintWall);
		if(settingsManager.vibrationFlag)
			Vibration.Vibrate(new long[] { 100, 60}, -1);
	}
	private void OnInvertColors()
	{
		if (bwMode)
		{
			BlackAndWhiteModeInit();
			bwMode = false;
		}
		else
		{
			bwMode = true;
		}
	}
	private void OnBonusTake(BonusType type, GameObject bonus)
	{
		if (settingsManager.vibrationFlag)
			Vibration.VibratePeek();
		if (type == BonusType.BlackAndWhite)
		{
			game.ChangeGameMode(GameModes.BlackAndWhite);
		}
		else if (type == BonusType.ColorBoost)
		{
			game.ChangeGameMode(GameModes.ColorBoost);
		}
		else if(type == BonusType.AddPoints)
		{
			audioManager.Play(ClipType.Buy);
			game.OnAddPoints(Bonus.BonusPoints);
		}

	}
	private void OnPlayerDie()
	{
		if (settingsManager.vibrationFlag)
			Vibration.VibratePeek();
		audioManager.Play(ClipType.Death);
		game.OnGameOver();
		stars.GetComponent<ParticleSystem>().Clear();
		Instantiate(indicator, player.transform.position, Quaternion.identity).GetComponent<Indicator>().InitError();
	}
	private void OnAddPoint()
	{
		game.OnAddPoint();
	}
	private void OnPlayerEnterToBox()
	{
		if(game.GameMode == GameModes.Normal){
			audioManager.Play(ClipType.Stack);
			Instantiate(indicator, player.transform.position, Quaternion.identity).GetComponent<Indicator>().InitWhite(stacks[0].GetComponent<Stack>().GetSpeed);
			if (settingsManager.vibrationFlag)
				Vibration.Vibrate(new long[] {100, 30 }, -1);
		}
		else if(game.GameMode == GameModes.BlackAndWhite){
			audioManager.Play(ClipType.StackReverb);
			Instantiate(indicator, player.transform.position, Quaternion.identity).GetComponent<Indicator>().InitBlack(stacks[0].GetComponent<Stack>().GetSpeed);
			if (settingsManager.vibrationFlag)
				Vibration.Vibrate(new long[] { 100, 30 }, -1);
		}
		else if(game.GameMode == GameModes.ColorBoost){
			audioManager.Play(ClipType.Stack);
			Instantiate(indicator, player.transform.position, Quaternion.identity).GetComponent<Indicator>().InitBlack(stacks[0].GetComponent<Stack>().GetSpeed);
		}
	}
	private void OnStackDestroy()
	{
		stacks[0].GetComponent<Stack>().onDestroy -= OnStackDestroy;

		stacks.RemoveAt(0);

		if (stacks.Count > 0)
		{
			stacks[0].GetComponent<Stack>().onDestroy += OnStackDestroy;
		}
		else isFirst = true;
	}
}
