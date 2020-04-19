using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AudioClips", menuName = "Custom/Audio Clips")]
public class AudioClips : ScriptableObject
{
    private static AudioClips _instance;

    public static AudioClips Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<AudioClips>("AudioClips");
            return _instance;
        }
    }

    public AudioClip Step;
    public AudioClip Swing;
    public AudioClip HitEnemy;
    [FormerlySerializedAs("GetHut")] public AudioClip GetHurt;
    public AudioClip Land;
    public AudioClip Jump;
    public AudioClip Slam;
    public AudioClip SlimeJump;
    public AudioClip BatSeePlayer;
    public AudioClip BatFlap;
    public AudioClip VineRetrieve;
    public AudioClip PickFlower;
    public AudioClip PlantFlower;
    public AudioClip DestroyTile;
}