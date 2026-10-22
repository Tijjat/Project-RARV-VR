
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VRAudioManager : MonoBehaviour
{
    public static VRAudioManager Instance { get; private set; }

    [Header("Sound Effects")]
    [SerializeField] private AudioClip snapSound;
    [SerializeField] private AudioClip deleteSound;
    [SerializeField] private AudioClip saveSound;
    [SerializeField] private AudioClip grabSound;

    [Header("Volumes")]
    [Range(0f, 1f)]
    [SerializeField] private float snapVolume = 0.6f;

    [Range(0f, 1f)]
    [SerializeField] private float deleteVolume = 0.6f;

    [Range(0f, 1f)]
    [SerializeField] private float saveVolume = 0.7f;

    [Range(0f, 1f)]
    [SerializeField] private float grabVolume = 0.4f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate VRAudioManager detected.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Play(AudioClip clip, float volume)
    {
        if (clip == null || audioSource == null)
            return;

        audioSource.PlayOneShot(clip, volume);
    }

    public void PlaySnap()
    {
        Play(snapSound, snapVolume);
    }

    public void PlayDelete()
    {
        Play(deleteSound, deleteVolume);
    }

    public void PlaySave()
    {
        Play(saveSound, saveVolume);
    }

    public void PlayGrab()
    {
        Play(grabSound, grabVolume);
    }
}
