using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Rotation")]
public class RotationSettings : ScriptableObject
{
    public float minRotationSpeed;

    public float maxRotationSpeed;
}