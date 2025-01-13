using UnityEngine;

public interface IPlayerInfo
{
    bool IsSelf { get; }
    Color Color { get; }
}