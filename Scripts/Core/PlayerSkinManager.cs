using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkinManager : MonoBehaviour
{
    PlayerSkin skin = new PlayerSkin();

    public void SetSkin(ShipType _ship, TrailType _trail)
    {
        skin.ship = _ship;
        skin.trail = _trail;
    }

    public PlayerSkin GetSkin() { return skin; }
}
public struct PlayerSkin
{
    public ShipType ship;
    public TrailType trail;
}

public enum ShipType
{
    Base,
    Romb,
    Heart,
    Star,
    Tristar
}

public enum TrailType
{
    Bubble,
    Line,
    Curve,
    Stars,
    Random
}
