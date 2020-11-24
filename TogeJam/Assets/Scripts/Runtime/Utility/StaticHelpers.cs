using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Utility
{
    public static class InputUtils
    {
        public static Vector2 GetMousePosition2D()
        { return Mouse.current.position.ReadValue(); }

        public static Vector3 GetMousePosition3D()
        { return Mouse.current.position.ReadValue(); }

        public static bool RaycastBeneathMouseCursor(Camera ViewingCamera, out RaycastHit Hit)
        {
            Vector3 NearPlane = GetMousePosition3D() + Vector3.forward * ViewingCamera.nearClipPlane;
            Vector3 FarPlane = GetMousePosition3D() + Vector3.forward * ViewingCamera.farClipPlane;

            Vector3 WorldMouseNear = ViewingCamera.ScreenToWorldPoint(NearPlane);
            Vector3 WorldMouseFar = ViewingCamera.ScreenToWorldPoint(FarPlane);

            if (Physics.Raycast(WorldMouseNear, WorldMouseFar - WorldMouseNear, out Hit))
                return true;

            return false;
        }

        public static Vector3Int GetCellWorldCoordinates(Grid InGrid, Vector3 WorldCoord)
        {
            return InGrid.WorldToCell(Vector3Int.RoundToInt(WorldCoord + InGrid.cellSize / 2.0f));
        }
    }

    public static class MathHelpers
    {
        public static bool Vector3Equals(Vector3 A, Vector3 B, float Tolerance = Vector3.kEpsilon)
        { return (A - B).sqrMagnitude < Tolerance; }

        public static float FloatRoundToNearestN(float In, float N)
        {
            float Mod = In % N;
            float Mid = N / 2.0f;
            
            if (Mod > Mid)
                return In + (N - Mod);
            else if (Mod < Mid)
                return In - Mod;

                return In;
        }

        public static Vector3 Vector3RoundToNearestN(Vector3 In, float N)
        {
            return new Vector3(FloatRoundToNearestN(In.x, N), FloatRoundToNearestN(In.y, N), FloatRoundToNearestN(In.z, N));
        }
    }

    public static class DebugUtils
    {
        public struct BuildWorldTextParams
        {
            public string _Text;
            public Transform _Parent;
            public Vector3 _LocalPosition;
            public Quaternion _Rotation;
            public int _FontSize;
            public TextAnchor _TextAnchor;
            public TextAlignment _TextAlignment;
            public int _SortingOrder;
            public Color _TextColor;

            public BuildWorldTextParams(Color InTextColor, Quaternion InRotation, string InText = "Hello", Transform InParent = null, Vector3 InLocalPosition = default(Vector3), int InFontSize = 40, TextAnchor InTextAnchor = TextAnchor.MiddleCenter, TextAlignment InTextAlignment = TextAlignment.Center, int InSortingOrder = 0)
            {
                _TextColor = InTextColor;
                _Text = InText;
                _Parent = InParent;
                _LocalPosition = InLocalPosition;
                _Rotation = InRotation;
                _FontSize = InFontSize;
                _TextAnchor = InTextAnchor;
                _TextAlignment = InTextAlignment;
                _SortingOrder = InSortingOrder;
            }
        }

        public static TextMesh CreateWorldText(BuildWorldTextParams InParams)
        {
            GameObject GO = new GameObject("WorldText", typeof(TextMesh));
            Transform T = GO.transform;
            T.SetParent(InParams._Parent, false);
            T.localPosition = InParams._LocalPosition;
            T.localRotation = InParams._Rotation;

            TextMesh Text = GO.GetComponent<TextMesh>();
            Text.anchor = InParams._TextAnchor;
            Text.alignment = InParams._TextAlignment;
            Text.text = InParams._Text;
            Text.fontSize = InParams._FontSize;
            Text.color = InParams._TextColor;
            Text.GetComponent<MeshRenderer>().sortingOrder = InParams._SortingOrder;

            return Text;
        }
    }

    public static class GenericHelpers
    {
        public static GameObject CreateManager(string ObjectName)
        {
            GameObject Prefab = Resources.Load("Prefabs/" + ObjectName) as GameObject;
            GameObject Instance  = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity);
            Instance.name = ObjectName;
            
            return (Prefab == null) ? null : Instance;
        }
    }
}