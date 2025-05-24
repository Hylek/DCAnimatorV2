#if UNITY_EDITOR
using System;
using DC.Animator.Data;
using DC.Animator.Utils;
using UnityEditor;
using UnityEngine;

namespace DC.Animator.Editor
{
    [CustomEditor(typeof(AnimationCollection))]
    public class AnimationCollectionEditor : UnityEditor.Editor
    {
        private SerializedProperty _animationsProperty;
        private AnimationCollection _collection;

        private void OnEnable()
        {
            _animationsProperty = serializedObject.FindProperty("animations");
            _collection = (AnimationCollection)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Animation Presets", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Add Animation", GUILayout.Height(30))) AddNewAnimation();

            EditorGUILayout.Space();
            
            for (var i = 0; i < _animationsProperty.arraySize; i++)
            {
                var animationProperty = _animationsProperty.GetArrayElementAtIndex(i);
                var nameProperty = animationProperty.FindPropertyRelative("name");
                var typeProperty = animationProperty.FindPropertyRelative("type");

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(nameProperty, GUIContent.none);
                EditorGUILayout.PropertyField(typeProperty, GUIContent.none, GUILayout.Width(100));
                
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    _animationsProperty.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }

                EditorGUILayout.EndHorizontal();
                
                var foldoutName = nameProperty.stringValue;
                if (string.IsNullOrEmpty(foldoutName))
                    foldoutName = "Animation " + i;

                animationProperty.isExpanded = EditorGUILayout.Foldout(animationProperty.isExpanded, foldoutName, true);

                if (animationProperty.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    
                    EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("duration"));
                    EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("delay"));
                    EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("easingType"));

                    EditorGUILayout.Space();
                    
                    var type = (AnimationCollection.AnimationType)typeProperty.enumValueIndex;

                    switch (type)
                    {
                        case AnimationCollection.AnimationType.Fade:
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("fromAlpha"));
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("toAlpha"));
                            break;

                        case AnimationCollection.AnimationType.Move:
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("fromPosition"));
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("toPosition"));
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("useLocalPosition"));
                            EditorGUILayout.PropertyField(
                                animationProperty.FindPropertyRelative("useAnchoredPosition"));
                            break;

                        case AnimationCollection.AnimationType.Scale:
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("fromScale"));
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("toScale"));
                            break;

                        case AnimationCollection.AnimationType.Rotate:
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("fromEulerAngles"));
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("toEulerAngles"));
                            break;

                        case AnimationCollection.AnimationType.Color:
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("fromColor"));
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("toColor"));
                            EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("isTextColor"));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddNewAnimation()
        {
            var index = _animationsProperty.arraySize;
            _animationsProperty.InsertArrayElementAtIndex(index);

            var element = _animationsProperty.GetArrayElementAtIndex(index);

            element.FindPropertyRelative("name").stringValue = "New Animation";
            element.FindPropertyRelative("type").enumValueIndex = 0;
            element.FindPropertyRelative("duration").floatValue = 0.5f;
            element.FindPropertyRelative("delay").floatValue = 0f;
            element.FindPropertyRelative("easingType").enumValueIndex = (int)Easing.Type.EaseOutQuad;

            element.FindPropertyRelative("fromAlpha").floatValue = 0f;
            element.FindPropertyRelative("toAlpha").floatValue = 1f;
            
            element.FindPropertyRelative("fromScale").vector3Value = Vector3.zero;
            element.FindPropertyRelative("toScale").vector3Value = Vector3.one;
            
            element.FindPropertyRelative("fromColor").colorValue = Color.white;
            element.FindPropertyRelative("toColor").colorValue = Color.white;

            element.isExpanded = true;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif