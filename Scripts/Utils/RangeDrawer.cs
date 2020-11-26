using UnityEngine;
using UnityEditor;
using UnityUlilities;

[CustomPropertyDrawer(typeof(IntRange))]
public class IntRangeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		IntRange range = new IntRange(
			property.FindPropertyRelative("from").intValue,
			property.FindPropertyRelative("to").intValue);
		EditorGUI.BeginProperty(position, label, property);
		EditorGUI.BeginChangeCheck();
		position = EditorGUI.PrefixLabel(position, label);
		if(position.height > 16f)
		{
			position.height = 16f;
		}
		position = new Rect(position.x, position.y, position.width, position.height + 2);
		property.FindPropertyRelative("from").intValue = EditorGUI.IntField(position, range.from);
		position = new Rect(position.x, position.y + position.height + 2, position.width, position.height);
		property.FindPropertyRelative("to").intValue = EditorGUI.IntField(position, range.to);
		EditorGUI.EndChangeCheck();
		EditorGUI.EndProperty();
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 16f + 18f;
	}
}

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		FloatRange range = new FloatRange(
			property.FindPropertyRelative("from").floatValue,
			property.FindPropertyRelative("to").floatValue);
		EditorGUI.BeginProperty(position, label, property);
		EditorGUI.BeginChangeCheck();
		position = EditorGUI.PrefixLabel(position, label);
		if (position.height > 16f)
		{
			position.height = 16f;
		}
		position = new Rect(position.x, position.y, position.width, position.height + 2);
		property.FindPropertyRelative("from").floatValue = EditorGUI.FloatField(position, range.from);
		position = new Rect(position.x, position.y + position.height + 2, position.width, position.height);
		property.FindPropertyRelative("to").floatValue = EditorGUI.FloatField(position, range.to);
		EditorGUI.EndChangeCheck();
		EditorGUI.EndProperty();
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 16f + 18f;
	}
}
