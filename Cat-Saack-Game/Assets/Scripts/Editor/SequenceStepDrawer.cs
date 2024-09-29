using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(SequenceStep), true)]
public class SequenceStepDrawer : PropertyDrawer
{
    private static readonly List<Type> stepTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => type.IsSubclassOf(typeof(SequenceStep)) && !type.IsAbstract)
        .ToList();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Store original position so we can move UI elements down properly
        Rect originalPosition = position;

        // Determine the type of the current SequenceStep
        var targetObject = property.managedReferenceValue;
        var currentType = targetObject?.GetType();

        // Dropdown to select the type
        Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        string[] typeNames = stepTypes.Select(t => t.Name).ToArray();
        int currentIndex = Array.FindIndex(typeNames, name => name == currentType?.Name);

        // Draw the dropdown for selecting the type
        int newIndex = EditorGUI.Popup(dropdownRect, "Step Type", currentIndex, typeNames);

        // Update the type if the selection changes
        if (newIndex != currentIndex && newIndex >= 0)
        {
            Type newType = stepTypes[newIndex];
            property.managedReferenceValue = Activator.CreateInstance(newType);
        }

        // Adjust the position downwards after drawing the dropdown
        position.y += EditorGUIUtility.singleLineHeight + 2; // Add some space

        // Draw the foldout to show or hide the step's properties
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "Properties");

        // Adjust position downwards after the foldout
        position.y += EditorGUIUtility.singleLineHeight + 2; // Add space for foldout

        // Draw the properties of the selected type if expanded
        if (property.isExpanded && property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            var childProperty = property.Copy();
            var endProperty = property.GetEndProperty();
            childProperty.NextVisible(true); // Move to first child property

            // Iterate through all the fields of the SequenceStep type and draw them
            while (!SerializedProperty.EqualContents(childProperty, endProperty))
            {
                float propertyHeight = EditorGUI.GetPropertyHeight(childProperty, true);
                position.height = propertyHeight;

                // Draw each child property
                EditorGUI.PropertyField(position, childProperty, true);

                // Move the position down for the next property field
                position.y += propertyHeight + 2;
                
                childProperty.NextVisible(false); // Move to next property
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = EditorGUIUtility.singleLineHeight * 2; // Dropdown + foldout

        if (property.isExpanded)
        {
            var childProperty = property.Copy();
            var endProperty = property.GetEndProperty();
            childProperty.NextVisible(true); // Move to first child

            while (!SerializedProperty.EqualContents(childProperty, endProperty))
            {
                totalHeight += EditorGUI.GetPropertyHeight(childProperty, true) + 12;
                childProperty.NextVisible(false);
            }
        }

        return totalHeight;
    }
}
