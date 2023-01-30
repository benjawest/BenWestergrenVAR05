using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




public class Exercise2 : MonoBehaviour
{

    public TMP_InputField inputFieldx;
    public TMP_InputField inputFieldy;
    public TMP_InputField inputFieldz;
    public List<GameObject> listOfObjects;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveFields()
    {
        //assign inputs from the 3 text fields to a single transform NewTransform 
        int x = int.Parse(inputFieldx.text);
        int y = int.Parse(inputFieldy.text);
        int z = int.Parse(inputFieldz.text);
        Vector3 newTransform = new Vector3(x, y, z);

        //assign the tranform for each Gameobject from NewTransform
        for (int i = 0; i < listOfObjects.Count; i++)
        {
            listOfObjects[i].transform.position = newTransform;
        }


    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
