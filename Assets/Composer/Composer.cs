using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
// Added for Dropdown functionality - Research more about what this is...
using System.Linq;

// VAR05BEN Composer Script
// Stores notes from NoteButtons to be played back in notesInComposition<>
// Handles adding, removing and clearing of notesInComposition<>
// Interacts with TMP_Dropdown to display composition sequence and removal of selected notes
public class Composer : MonoBehaviour
{
    private List<AudioSource> notesInComposition = new List<AudioSource>();
    public Button clearButton;
    public Button playButton;
    public Button removeDropdownButton;
    public TMP_Dropdown audioSourcesDropdown;

    private void Start()
    {
        RefreshDropdownOptions();
    }

    // Add note to notesInComposition<>
    public void AddNoteToComposition(AudioSource note)
    {
        notesInComposition.Add(note);
        RefreshDropdownOptions();
    }

    // Play all notes in notesInComposition<>
    public void PlayNotesInComposition()
    {
        StartCoroutine(PlayNotes());
    }

    // A coroutine, plays notesInComposition one note at a time, waiting for the length of each Audiosource
    IEnumerator PlayNotes()
    {
        foreach (AudioSource note in notesInComposition)
        {
            note.Play();
            yield return new WaitForSeconds(note.clip.length);
        }
    }

    // Clear notesInComposition<>
    public void ClearNotesInComposition()
    {
        notesInComposition.Clear();
        RefreshDropdownOptions();
    }

    // Remove selected note from notesInComposition<i>
    public void RemoveSelectedAudioSource()
    {
        int selectedIndex = audioSourcesDropdown.value;
        notesInComposition.RemoveAt(selectedIndex);
        RefreshDropdownOptions();
    }

    // Update the dropdown options
    private void RefreshDropdownOptions()
    {
        audioSourcesDropdown.ClearOptions();
        List<string> options = notesInComposition.Select(source => source.name).ToList();
        audioSourcesDropdown.AddOptions(options);
    }

    void Update()
    {

    }
}