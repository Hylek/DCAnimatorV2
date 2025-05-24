#if UNITY_EDITOR
using System;
using DC.Animator.Adapters;
using DC.Animator.Core;
using DC.Animator.Data;
using DC.Animator.Utils;
using DCAnimator.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DC.Animator.Editor
{
    public class AnimationEditorWindow : EditorWindow
    {
        private readonly string[] _animationTypes = { "Fade", "Move", "Scale", "Rotate", "Color" };

        private readonly string[] _tabNames = { "Create Animation", "Test Animation", "Manage Presets" };
        private AnimationCollection _animationCollection;

        private AnimationData _currentAnimation = new();
        private bool _isAnimating;
        private float _lastUpdateTime;
        private string _presetName = "New Preset";
        private IAnimation _previewAnimation;
        private Vector2 _scrollPosition;
        private int _selectedAnimationType;

        private int _selectedTabIndex;
        
        private GameObject _targetObject;

        private void OnEnable()
        {
            ResetCurrentAnimation();
        }

        private void OnGUI()
        {
            _selectedTabIndex = GUILayout.Toolbar(_selectedTabIndex, _tabNames);

            EditorGUILayout.Space();

            switch (_selectedTabIndex)
            {
                case 0:
                    DrawCreateAnimationTab();
                    break;
                case 1:
                    DrawTestAnimationTab();
                    break;
                case 2:
                    DrawManagePresetsTab();
                    break;
            }

            if (!_isAnimating || _previewAnimation == null) return;

            var deltaTime = (float)(EditorApplication.timeSinceStartup - _lastUpdateTime);
            _lastUpdateTime = (float)EditorApplication.timeSinceStartup;

            _previewAnimation.Update(deltaTime);

            if (_previewAnimation.IsComplete) _isAnimating = false;

            Repaint();
        }

        [MenuItem("Window/Simple Animations/Animation Editor")]
        public static void ShowWindow()
        {
            GetWindow<AnimationEditorWindow>("Animation Editor");
        }

        private void ResetCurrentAnimation()
        {
            _currentAnimation = new AnimationData
            {
                name = "New Animation",
                type = AnimationCollection.AnimationType.Fade,
                duration = 0.5f,
                delay = 0f,
                easingType = Easing.Type.EaseOutQuad,
                fromAlpha = 0f,
                toAlpha = 1f,
                fromScale = Vector3.zero,
                toScale = Vector3.one,
                fromColor = Color.white,
                toColor = Color.white
            };
        }

        private void DrawCreateAnimationTab()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.LabelField("Create New Animation", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _currentAnimation.name = EditorGUILayout.TextField("Animation Name", _currentAnimation.name);

            _selectedAnimationType = EditorGUILayout.Popup("Animation Type", _selectedAnimationType, _animationTypes);
            _currentAnimation.type = (AnimationCollection.AnimationType)_selectedAnimationType;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Common Properties", EditorStyles.boldLabel);
            _currentAnimation.duration = EditorGUILayout.FloatField("Duration", _currentAnimation.duration);
            _currentAnimation.delay = EditorGUILayout.FloatField("Delay", _currentAnimation.delay);

            var easingNames = Enum.GetNames(typeof(Easing.Type));
            var easingIndex = (int)_currentAnimation.easingType;
            easingIndex = EditorGUILayout.Popup("Easing Type", easingIndex, easingNames);
            _currentAnimation.easingType = (Easing.Type)easingIndex;

            EditorGUILayout.Space();

            // Specific Properties
            EditorGUILayout.LabelField("Animation Properties", EditorStyles.boldLabel);

            switch (_currentAnimation.type)
            {
                case AnimationCollection.AnimationType.Fade:
                    _currentAnimation.fromAlpha =
                        EditorGUILayout.Slider("From Alpha", _currentAnimation.fromAlpha, 0f, 1f);
                    _currentAnimation.toAlpha = EditorGUILayout.Slider("To Alpha", _currentAnimation.toAlpha, 0f, 1f);
                    break;

                case AnimationCollection.AnimationType.Move:
                    _currentAnimation.fromPosition =
                        EditorGUILayout.Vector3Field("From Position", _currentAnimation.fromPosition);
                    _currentAnimation.toPosition =
                        EditorGUILayout.Vector3Field("To Position", _currentAnimation.toPosition);
                    _currentAnimation.useLocalPosition =
                        EditorGUILayout.Toggle("Use Local Position", _currentAnimation.useLocalPosition);
                    _currentAnimation.useAnchoredPosition = EditorGUILayout.Toggle("Use Anchored Position",
                        _currentAnimation.useAnchoredPosition);
                    break;

                case AnimationCollection.AnimationType.Scale:
                    _currentAnimation.fromScale =
                        EditorGUILayout.Vector3Field("From Scale", _currentAnimation.fromScale);
                    _currentAnimation.toScale = EditorGUILayout.Vector3Field("To Scale", _currentAnimation.toScale);
                    break;

                case AnimationCollection.AnimationType.Rotate:
                    _currentAnimation.fromEulerAngles =
                        EditorGUILayout.Vector3Field("From Rotation", _currentAnimation.fromEulerAngles);
                    _currentAnimation.toEulerAngles =
                        EditorGUILayout.Vector3Field("To Rotation", _currentAnimation.toEulerAngles);
                    break;

                case AnimationCollection.AnimationType.Color:
                    _currentAnimation.fromColor = EditorGUILayout.ColorField("From Color", _currentAnimation.fromColor);
                    _currentAnimation.toColor = EditorGUILayout.ColorField("To Color", _currentAnimation.toColor);
                    _currentAnimation.isTextColor =
                        EditorGUILayout.Toggle("Is Text Color", _currentAnimation.isTextColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();

            _animationCollection = (AnimationCollection)EditorGUILayout.ObjectField("Save To Preset", _animationCollection,
                typeof(AnimationCollection), false);

            if (_animationCollection && GUILayout.Button("Save")) SaveAnimationToPreset();

            EditorGUILayout.EndHorizontal();

            if (!_animationCollection)
            {
                EditorGUILayout.BeginHorizontal();
                _presetName = EditorGUILayout.TextField("New Preset Name", _presetName);

                if (GUILayout.Button("Create")) CreateNewPreset();

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawTestAnimationTab()
        {
            EditorGUILayout.LabelField("Test Animation", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _targetObject =
                (GameObject)EditorGUILayout.ObjectField("Target Object", _targetObject, typeof(GameObject), true);
            _animationCollection = (AnimationCollection)EditorGUILayout.ObjectField("Animation Preset", _animationCollection,
                typeof(AnimationCollection), false);

            EditorGUILayout.Space();

            if (_targetObject && _animationCollection)
            {
                EditorGUILayout.LabelField("Available Animations:", EditorStyles.boldLabel);

                if (_animationCollection.animations.Count == 0)
                    EditorGUILayout.HelpBox("No animations in preset", MessageType.Info);
                else
                    foreach (var anim in _animationCollection.animations)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(anim.name, EditorStyles.label);

                        if (GUILayout.Button("Play", GUILayout.Width(60))) PlayAnimation(anim.name);

                        EditorGUILayout.EndHorizontal();
                    }
            }
            else
            {
                EditorGUILayout.HelpBox("Select a target object and an animation preset to test animations",
                    MessageType.Info);
            }
        }

        private void DrawManagePresetsTab()
        {
            EditorGUILayout.LabelField("Manage Animation Presets", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _animationCollection = (AnimationCollection)EditorGUILayout.ObjectField("Animation Preset", _animationCollection,
                typeof(AnimationCollection), false);

            EditorGUILayout.Space();

            if (_animationCollection)
            {
                EditorGUILayout.LabelField("Animations in Preset:", EditorStyles.boldLabel);

                if (_animationCollection.animations.Count == 0)
                    EditorGUILayout.HelpBox("No animations in preset", MessageType.Info);
                else
                    for (var i = 0; i < _animationCollection.animations.Count; i++)
                    {
                        var anim = _animationCollection.animations[i];

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"{anim.name} ({anim.type})", EditorStyles.label);

                        if (GUILayout.Button("Edit", GUILayout.Width(60)))
                        {
                            _currentAnimation = anim;
                            _selectedAnimationType = (int)anim.type;
                            _selectedTabIndex = 0; // Switch to Create tab
                        }

                        if (GUILayout.Button("Delete", GUILayout.Width(60)))
                            if (EditorUtility.DisplayDialog("Delete Animation",
                                    $"Are you sure you want to delete animation '{anim.name}'?",
                                    "Delete", "Cancel"))
                            {
                                _animationCollection.animations.RemoveAt(i);
                                EditorUtility.SetDirty(_animationCollection);
                                AssetDatabase.SaveAssets();
                                break;
                            }

                        EditorGUILayout.EndHorizontal();
                    }

                EditorGUILayout.Space();

                if (GUILayout.Button("Create New Animation", GUILayout.Height(30)))
                {
                    ResetCurrentAnimation();
                    _selectedTabIndex = 0; // Switch to Create tab
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                _presetName = EditorGUILayout.TextField("New Preset Name", _presetName);

                if (GUILayout.Button("Create")) CreateNewPreset();

                EditorGUILayout.EndHorizontal();
            }
        }

        private void CreateNewPreset()
        {
            if (string.IsNullOrEmpty(_presetName))
            {
                EditorUtility.DisplayDialog("Error", "Please enter a name for the preset", "OK");
                return;
            }

            var path = EditorUtility.SaveFilePanelInProject(
                "Save Animation Preset",
                _presetName,
                "asset",
                "Save animation preset"
            );

            if (string.IsNullOrEmpty(path))
                return;

            _animationCollection = CreateInstance<AnimationCollection>();
            AssetDatabase.CreateAsset(_animationCollection, path);
            AssetDatabase.SaveAssets();

            SaveAnimationToPreset();
        }

        private void SaveAnimationToPreset()
        {
            if (!_animationCollection)
                return;
            
            for (var i = 0; i < _animationCollection.animations.Count; i++)
                if (_animationCollection.animations[i].name == _currentAnimation.name)
                {
                    if (!EditorUtility.DisplayDialog("Overwrite Animation",
                            $"Animation '{_currentAnimation.name}' already exists. Overwrite?",
                            "Overwrite", "Cancel")) return;
                    
                    _animationCollection.animations[i] = _currentAnimation;
                    EditorUtility.SetDirty(_animationCollection);
                    AssetDatabase.SaveAssets();

                    return;
                }
            
            _animationCollection.animations.Add(_currentAnimation);
            EditorUtility.SetDirty(_animationCollection);
            AssetDatabase.SaveAssets();
        }

        private void PlayAnimation(string animationName)
        {
            if (!_targetObject || !_animationCollection)
                return;
            
            IAnimatable adapter;
            
            var image = _targetObject.GetComponent<Image>();
            if (image)
                adapter = new ImageAdapter(image);

            var text = _targetObject.GetComponent<TextMeshProUGUI>();
            if (text)
                adapter = new TMPTextAdapter(text);

            var tmpro = _targetObject.GetComponent<TextMeshPro>();
            if (tmpro)
                adapter = new TMPTextAdapter(tmpro);

            var spriteRenderer = _targetObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
                adapter = new SpriteAdapter(spriteRenderer);

            var rectTransform = _targetObject.GetComponent<RectTransform>();
            if (rectTransform)
            {
                adapter = new RectTransformAdapter(rectTransform);
            }
            else
            {
                var transform = _targetObject.transform;
                adapter = new TransformAdapter(transform);
            }

            _previewAnimation = _animationCollection.CreateAnimation(adapter, animationName);

            if (_previewAnimation == null)
            {
                EditorUtility.DisplayDialog("Error", $"Animation '{animationName}' not found in preset", "OK");
                return;
            }

            _previewAnimation.Start();
            _isAnimating = true;
            _lastUpdateTime = (float)EditorApplication.timeSinceStartup;

            EditorApplication.update += EditorUpdate;
        }

        private void EditorUpdate()
        {
            if (_isAnimating && _previewAnimation != null)
            {
                Repaint();

                if (!_previewAnimation.IsComplete) return;

                _isAnimating = false;
            }

            EditorApplication.update -= EditorUpdate;
        }
    }
}
#endif