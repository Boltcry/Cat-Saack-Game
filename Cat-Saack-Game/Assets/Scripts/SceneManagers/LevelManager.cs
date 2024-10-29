using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Sequence sequenceOnAwake = new Sequence();

    void Start()
    {
        if (sequenceOnAwake != null)
        {
            SequenceManager.StartSequence(sequenceOnAwake);
        }
    }
}
