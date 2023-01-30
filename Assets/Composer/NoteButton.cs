using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteButton : MonoBehaviour
{
    public AudioSource audioSource;
    public Button playButton;
    public Button addButton;
    public Button removeButton;
    private Composer composer;

    private void Start()
    {
        // Get a reference to the Composer script
        composer = GameObject.Find("Composer").GetComponent<Composer>();
        // Add the PlayNote method as a listener for the play button's onClick event
        playButton.onClick.AddListener(PlayNote);
        // Add the AddNoteToComposition method as a listener for the add button's onClick event
        addButton.onClick.AddListener(AddNoteToComposition);
        // set remove as not active
        removeButton.gameObject.SetActive(false);
    }

    // Play the audio source attached to the prefab
    private void PlayNote()
    {
        audioSource.Play();
    }

    // Add the audio source attached to the prefab to the notesInComposition list
    private void AddNoteToComposition()
    {
        composer.AddNoteToComposition(audioSource);
    }

    public void RemoveNoteFromComposition()
    {
        composer.RemoveNoteFromComposition(audioSource, this.gameObject);
      
    }




}
