using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSequenceData", menuName = "ScriptableObjects/SequenceData")]
public class SequenceDataSO : ScriptableObject
{
    public Sequence sequenceToRun;
}
