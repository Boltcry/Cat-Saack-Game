using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class DialogueVariableObserver
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    // compile ink story file into a Story object
    public DialogueVariableObserver(TextAsset aInkJSON)
    {
        Story globalVariablesStory = new Story(aInkJSON.text);

        // initialize dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            //Debug.Log("Initialized global dialogue variable: " +name+ " = " +value);
        }
    }

    public void StartListening(Story aStory)
    {
        // VariablesToStory should be called before assigning listener
        VariablesToStory(aStory);
        aStory.variablesState.variableChangedEvent += OnVariableChanged;
    }

    public void StopListening(Story aStory)
    {
        aStory.variablesState.variableChangedEvent -=OnVariableChanged;
    }

    void OnVariableChanged(string aName, Ink.Runtime.Object aValue)
    {
        //Debug.Log("Variable changed: " +aName+ " = " +aValue);
        if (variables.ContainsKey(aName))
        {
            variables.Remove(aName);
            variables.Add(aName,aValue);
        }
    }

    void VariablesToStory(Story aStory)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            aStory.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
