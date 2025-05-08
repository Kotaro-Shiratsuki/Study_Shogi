using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KomaSO))]
public class EnumArrayScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        KomaSO scriptableObject = (KomaSO)target;

        EditorGUILayout.LabelField("Default Movement Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        for (int i = 0; i < scriptableObject.MovementData.DefaultMoving.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < scriptableObject.MovementData.DefaultMoving[i].MovingListRow.Count; j++)
            {
                scriptableObject.MovementData.DefaultMoving[i].MovingListRow[j] = (Movingtype)EditorGUILayout.EnumPopup(scriptableObject.MovementData.DefaultMoving[i].MovingListRow[j]);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Extra Movement Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        for (int i = 0; i < scriptableObject.MovementData.ExtraMoving.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < scriptableObject.MovementData.ExtraMoving[i].MovingListRow.Count; j++)
            {
                scriptableObject.MovementData.ExtraMoving[i].MovingListRow[j] = (Movingtype)EditorGUILayout.EnumPopup(scriptableObject.MovementData.ExtraMoving[i].MovingListRow[j]);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        DrawPropertiesExcluding(serializedObject, "m_Script", "<MovementData>");

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(scriptableObject);
        }
    }
}

