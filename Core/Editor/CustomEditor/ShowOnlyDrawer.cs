using System;
using MotionAI.Core.Models;
using MotionAI.Core.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MotionAI.Core.Editor.CustomEditor {
    [CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
    public class ShowOnlyDrawer : PropertyDrawer {
        public static string CheckAllowedTyped(SerializedProperty prop) {
            Object o = prop.objectReferenceValue;

            if (o is Controller.MotionAIController mai) {
                return $"{mai.name} - {mai.DeviceId} - {mai.DeviceOrientation.ToString()}";
            }

            if (o is AbstractModelComponent model) {
                try {
                    return $"{model.chosenBuild.modelName}";
                }
                catch (Exception e) {
                    return "";
                }
            }

            return "Not Allowed";
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
            string valueStr;

            switch (prop.propertyType) {
                case SerializedPropertyType.Integer:
                    valueStr = prop.intValue.ToString();
                    break;
                case SerializedPropertyType.Boolean:
                    valueStr = prop.boolValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    valueStr = prop.floatValue.ToString("0.00000");
                    break;
                case SerializedPropertyType.String:
                    valueStr = prop.stringValue;
                    break;
                default:
                    valueStr = CheckAllowedTyped(prop);
                    break;
            }

            EditorGUI.LabelField(position, label.text, valueStr);
        }
    }
}