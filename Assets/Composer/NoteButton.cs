using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Prefab for single note UI.
// Handles playback of single notes on demand
// References Composer, to add this objects' note to notesInComposition<>
public class NoteButton : MonoBehaviour
{
    public AudioSource audioSource;
    public Button playButton;
    public Button addButton;
    private Composer composer;

    private void Start()
    {
        // Get a reference to the Composer script
        composer = GameObject.Find("Composer").GetComponent<Composer>();
    }

    // Play this objects' note
    public void PlayNote()
    {
        audioSource.Play();
    }

    // Add this objects' note to notesInComposition<>
    public void AddNoteToComposition()
    {
        composer.AddNoteToComposition(audioSource);
    }
}
