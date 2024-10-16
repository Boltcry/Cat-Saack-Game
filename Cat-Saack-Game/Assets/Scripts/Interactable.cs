using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface Interactable
{
    event Action OnSelected;
    public void OnSelect();
}
