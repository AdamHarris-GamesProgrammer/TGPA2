using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverController : MonoBehaviour
{
    Transform[] _children = new Transform[2];

    int _capacity = 2;
    int _currentPopulation = 0;

    public bool IsFull
    {
        //If the current population is the same as the capacity then the cover is full.
        get { return _currentPopulation >= _capacity; }
    }

    private void Awake()
    {
        //TODO: Improve this for more complex covers which could have more points
        //TODO: Dynamic cover points?
        _children[0] = transform.GetChild(0);    
        _children[1] = transform.GetChild(1);    
    }

    public Transform[] GetCoverPoints()
    {
        return _children;
    }

    //No need to keep track of the actual enemy using the cover, just need the total amount using the cover;
    public void AddUser()
    {
        _currentPopulation++;
        //Debug.Log("Cover Population: " + _currentPopulation);
    }

    public void RemoveUser()
    {
        _currentPopulation =  (int)Mathf.Clamp(_currentPopulation--, 0.0f, _capacity);
    }
}
