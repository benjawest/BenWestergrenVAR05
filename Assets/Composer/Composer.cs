using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// VAR05BEN Composer Script


public class Composer : MonoBehaviour
{
    // A public list of AudioSources to be displayed in the Unity Editor
    public AudioSource[] notesInScale;
    // The play button to be displayed in the Unity Editor
    public Button playButton;
    // A list to store the selected AudioSources
    private List<AudioSource> notesInComposition = new List<AudioSource>();
    // A public button to be displayed in the Unity Editor
    public Button clearButton;
    // A public list to store instances of the prefabs for the UI
    public List<GameObject> notesList;
    public GameObject noteButtonPrefab;
    public Transform noteListParent;
   

    private void Start()
    {
        // Add the PlayNotesInComposition method as a listener for the play button's onClick event
        playButton.onClick.AddListener(PlayNotesInComposition);
        // Add the ClearNotesInComposition method as a listener for the clear button's onClick event
        clearButton.onClick.AddListener(ClearNotesInComposition);
    }

    // Adds an AudioSource to the notesInComposition list
    public void AddNoteToComposition(AudioSource note)
    {
        notesInComposition.Add(note);
        GameObject instance = Instantiate(noteButtonPrefab, noteListParent.transform);
        notesList.Add(instance);
        RectTransform rectTransform = instance.GetComponent<RectTransform>();

        // Set the x position of each instance relative to the left edge of the panel
        int elementCount = notesList.Count - 1;
        rectTransform.localPosition = new Vector3(rectTransform.sizeDelta.x * elementCount, 0, 0);

        // Access the remove and add buttons in the new instance and hide/show as necessary
        Button addButton = instance.transform.Find("addButton").GetComponent<Button>();
        Button removeButton = instance.transform.Find("removeButton").GetComponent<Button>();

        addButton.gameObject.SetActive(false);
        removeButton.gameObject.SetActive(true);

    }

    // Plays the notes in the notesInComposition list
    private void PlayNotesInComposition()
    {
        StartCoroutine(PlayNotes());
    }

    // A coroutine to play the notes in the notesInComposition list one by one
    IEnumerator PlayNotes()
    {

        // Iterate through the notesInComposition list and play each note
        foreach (AudioSource note in notesInComposition)
        {
            note.Play();
            // Wait for the length of the clip before moving on to the next note
            yield return new WaitForSeconds(note.clip.length);
        }
    }


    // Clears the notesInComposition list
    private void ClearNotesInComposition()
    {
        notesInComposition.Clear();
        foreach (GameObject instance in notesList)
        {
            Destroy(instance);
        }
        notesList.Clear();
    }

    // Remove selected note from notesInComposition
    public void RemoveNoteFromComposition(AudioSource note, GameObject instance)
    {
        notesInComposition.Remove(note);
        notesList.Remove(instance);
        Destroy(instance);
    }



    void Update()
    {
       

    }
}