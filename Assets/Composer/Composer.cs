using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

// Add for Dropdown functionality
using System.Linq;

// VAR05BEN Composer Script

public class Composer : MonoBehaviour
{
    public Button playButton;
    private List<AudioSource> notesInComposition = new List<AudioSource>();
    public Button clearButton;
  
    //// A public list to store instances of the prefabs for the UI - PREFABS
    //public List<GameObject> notesList;
    //public GameObject noteButtonPrefab;
    //public Transform noteListParent;

    // Refernce UI for Dropdown features - DROPDOWN
    public TMP_Dropdown audioSourcesDropdown;
    public Button removeDropdownButton;

    private void Start()
    {
        RefreshDropdownOptions();
    }

    // Adds an AudioSource to the notesInComposition list
    public void AddNoteToComposition(AudioSource note)
    {
        notesInComposition.Add(note);
        //GameObject instance = Instantiate(noteButtonPrefab, noteListParent.transform);
        //notesList.Add(instance);
        //RectTransform rectTransform = instance.GetComponent<RectTransform>();

        //// Set the x position of each instance relative to the left edge of the panel
        //int elementCount = notesList.Count - 1;
        //rectTransform.localPosition = new Vector3(rectTransform.sizeDelta.x * elementCount, 0, 0);

        //// Access the remove and add buttons in the new instance and hide/show as necessary
        //Button addButton = instance.transform.Find("addButton").GetComponent<Button>();
        //Button removeButton = instance.transform.Find("removeButton").GetComponent<Button>();

        //addButton.gameObject.SetActive(false);
        //removeButton.gameObject.SetActive(true);

        // Temp for Dropdown
        RefreshDropdownOptions();

    }

    // Plays the notes in the notesInComposition list
    public void PlayNotesInComposition()
    {
        StartCoroutine(PlayNotes());
    }

    // A coroutine to play the notes in the notesInComposition list one by one
    IEnumerator PlayNotes()
    {
        foreach (AudioSource note in notesInComposition)
        {
            note.Play();
            yield return new WaitForSeconds(note.clip.length);
        }
    }

    // Clears the notesInComposition list
    public void ClearNotesInComposition()
    {
        notesInComposition.Clear();
        //foreach (GameObject instance in notesList)
        //{
        //    Destroy(instance);
        //}
        //notesList.Clear();
        RefreshDropdownOptions();
    }

    //// Remove selected note from notesInComposition - FOR PREFABS
    //public void RemoveNoteFromComposition(AudioSource note, GameObject instance)
    //{
    //    notesInComposition.Remove(note);
    //    notesList.Remove(instance);
    //    Destroy(instance);
    //}

    // Remove notes from composition - REMOVE WITH DROPDOWN
    public void RemoveSelectedAudioSource()
    {
        int selectedIndex = audioSourcesDropdown.value;
        notesInComposition.RemoveAt(selectedIndex);

        RefreshDropdownOptions();
    }

    // Update the dropdown options - REMOVE WITH DROPDOWN
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