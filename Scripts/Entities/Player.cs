using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUlilities;

public class Player : MonoBehaviour
{
	public Action OnDie, OnAddPoint, OnEnterBox, OnPaintWallEnter;
	public Action<BonusType, GameObject> OnBonusTake;

	public int speed = 2;
	public int rotationSpeed = 4;
	public GameObject trail;
	public GameObject explosion;

	public List<Mesh> ships = new List<Mesh>();
	public List<GameObject> trails = new List<GameObject>();

	bool isDie, colorBoostMode, autoRotate;
	Vector3 touchPos;
	HSBColor color;
	HSBColor remindColor;
	Animator animator;
	Stack remindStack;
	PlayerSkin skin;

	public void Init()
	{
		transform.position = new Vector2(0, -(ScreenHelper.screenHeight / 2) + (ScreenHelper.screenHeight / 3));
		transform.localScale = Vector3.one;
		animator = GetComponent<Animator>();
	}
	public void SetSkin(PlayerSkin _skin)
	{
		skin = _skin;
		GetComponent<MeshFilter>().sharedMesh = ships[(int)skin.ship];
		GetComponent<MeshCollider>().sharedMesh = ships[(int)skin.ship];

		trail = Instantiate(trails[(int)skin.trail]);
		trail.transform.SetParent(gameObject.transform);
		trail.transform.localPosition = new Vector3(0, -0.1f, 0);

		if (skin.ship == ShipType.Tristar)
			autoRotate = true;
	}
	public void SetColor(HSBColor _color)
	{
		animator.SetTrigger("OnColorChange");
		color = _color;
		GetComponent<Renderer>().material.SetColor("_Color", color.ToColor());
		trail.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", color.ToColor());
	}
	public void ColorBoostMode()
	{
		colorBoostMode ^= true;
		if (colorBoostMode)
			remindColor = color;
		else SetColor(remindColor);
	}

	private void Update()
	{
		if (isDie)
			return;

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Moved) 
			{
				touchPos = Camera.main.ScreenToWorldPoint(touch.position);
				if (touchPos.y > (ScreenHelper.screenHeight / 2) - (ScreenHelper.screenHeight / 7))
					touchPos = transform.position;
			}
		}
		else touchPos = transform.position;

		if (colorBoostMode)
			SetColor(HSBColor.GetRandomColor(new FloatRange(0.0f, 1.0f), new FloatRange(1.0f, 1.0f), new FloatRange(1.0f, 1.0f)));

#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (touchPos.y > (ScreenHelper.screenHeight / 2) - (ScreenHelper.screenHeight / 7))
				touchPos = transform.position;
		}
#endif
	}
	private void FixedUpdate()
	{
		if (isDie)
			return;

		float distance = 0;
		if (transform.position.x != touchPos.x)
		{
			transform.position = new Vector3(Mathf.Lerp(transform.position.x, touchPos.x, speed * Time.fixedDeltaTime), transform.position.y, transform.position.z);

			distance = transform.position.x - touchPos.x;
		}

		if (autoRotate)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + rotationSpeed * 10 * Time.fixedDeltaTime);
		}
		else
		{
			Quaternion newRot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + map(Mathf.Abs(distance), 0f, 1.0f, 0f, 40.0f) * (distance > 0 ? 1 : -1));
			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, rotationSpeed * Time.fixedDeltaTime); 
		}
	}
	private float map(float x, float in_min, float in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Box"))
		{
			if (!colorBoostMode)
			{
				if (other.gameObject.GetComponent<Box>()?.GetColor != color)
				{
					animator.enabled = false;
					trail.GetComponent<ParticleSystem>().Stop();
					explosion = Instantiate(explosion, transform.position, Quaternion.identity);
					var main = explosion.GetComponent<ParticleSystem>().main;
					main.startColor = color.ToColor();
					var shape = explosion.GetComponent<ParticleSystem>().shape;
					shape.mesh = ships[(int)skin.ship];
					explosion.GetComponent<ParticleSystem>().Play();
					OnDie();
					isDie = true;
					Destroy(gameObject);
				}
				else
				{
					OnEnterBox();
				}
			}
		}
		else if (other.CompareTag("PaintWall"))
		{
			OnPaintWallEnter();
			SetColor(other.GetComponent<PaintWall>().GetColor);
		}
		else if (other.CompareTag("Bonus"))
		{
			OnBonusTake(other.GetComponent<Bonus>().type, other.gameObject);
			Destroy(other.gameObject);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		Box box = other.gameObject.GetComponent<Box>();
		if (box)
		{
			if (!colorBoostMode)
			{
				if (box.GetColor == color)
				{
					AddPoint(box);
				}
			}
			else 
			{
				AddPoint(box);
			}
		}
	}

	private void AddPoint(Box box)
	{
		if (box.GetParentStack != remindStack)
		{
			OnAddPoint();
			remindStack = box.GetParentStack;
		}
	}

}
