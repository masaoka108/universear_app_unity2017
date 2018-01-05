using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class LocalizeUITextArea : MonoBehaviour {

	[SerializeField] string strEn;
	[SerializeField] string strJp;
	[SerializeField] string strZh;
	[SerializeField] string strZhCn;
	[SerializeField] string strZhTw;
	[SerializeField] string strKo;

	void Awake() {
		Dictionary<SystemLanguage, string> dict = new Dictionary<SystemLanguage, string>() {
			{ SystemLanguage.English, strEn},
			{ SystemLanguage.Japanese, strJp},
			{ SystemLanguage.Chinese, strZh},
			{ SystemLanguage.ChineseSimplified, strZhCn},
			{ SystemLanguage.ChineseTraditional, strZhTw},
			{ SystemLanguage.Korean, strKo},
		};

		//Change
		SystemLanguage lang = Application.systemLanguage;
		Text mText = GetComponent<Text>();
		mText.text = dict.ContainsKey(lang) ? dict[lang] : dict[SystemLanguage.English];
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(LocalizeUITextArea))]
[CanEditMultipleObjects]
public class LocalizeEditorTextArea : Editor {

	SerializedProperty propEn;
	SerializedProperty propJp;
	SerializedProperty propZh;
	SerializedProperty propZhCn;
	SerializedProperty propZhTw;
	SerializedProperty propKo;

	void OnEnable() {
		propEn = serializedObject.FindProperty("strEn");
		propJp = serializedObject.FindProperty("strJp");
		propZh = serializedObject.FindProperty("strZh");
		propZhCn = serializedObject.FindProperty("strZhCn");
		propZhTw = serializedObject.FindProperty("strZhTw");
		propKo = serializedObject.FindProperty("strKo");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		CreateUI("English", propEn, true);
		CreateUI("Japanese", propJp, true);
		CreateUI("Chainese", propZh, true);
		CreateUI("Chainese(Simplified)", propZhCn, true);
		CreateUI("Chainese(Traditional)", propZhTw, true);
		CreateUI("Korean", propKo, false);

		serializedObject.ApplyModifiedProperties();
	}

	private void CreateUI(string label, SerializedProperty prop, bool addSpace) {
		EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
		prop.stringValue = EditorGUILayout.TextArea(prop.stringValue);
		if(addSpace) EditorGUILayout.Space();
	}
}
#endif