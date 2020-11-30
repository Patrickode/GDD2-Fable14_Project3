using UnityEngine;

public class Doorbell : MonoBehaviour
{
    // Doorbell Pull with pull Store Bell 01 by maisonsonique at freesound.org
    // https://freesound.org/people/maisonsonique/sounds/196368/
    [SerializeField]
    private AudioClip doorBellSound = null;

    void Start()
    {
        FindObjectOfType<SoundEffectsManager>().PlaySound(doorBellSound);
    }
}
