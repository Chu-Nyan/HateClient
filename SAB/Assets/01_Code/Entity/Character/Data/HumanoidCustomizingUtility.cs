using Chu.Core;
using UnityEngine;

namespace SAB.Unit
{
    public static class HumanoidCustomizingUtility
    {
        public const int EyeCount = 12;
        public const int EyebrowCount = 2;
        public const int HairCount = 13;
        public const int MouthCount = 12;

        public static string GetPath(CustomizingPart part, int number)
        {
            return part switch
            {
                CustomizingPart.Eye => GetEyePath(number),
                CustomizingPart.Eyebrow => GetEyebrowPath(number),
                CustomizingPart.Hair => GetHairPath(number),
                CustomizingPart.Mouth => GetMouthPath(number),
                _ => throw new System.Exception(),
            };
        }

        public static Mesh GetMesh(CustomizingPart part, int id)
        {
            string path = GetPath(part, id);
            return AssetManager.LoadAssetSync<Mesh>(path);
        }

        public static string GetEyePath(int number)
        {
            return GetPath("Eye", number, EyeCount);
        }

        public static string GetEyebrowPath(int number)
        {
            return GetPath("Eyebrow", number, EyebrowCount);
        }

        public static string GetHairPath(int number)
        {
            return GetPath("Hair", number, HairCount);
        }

        public static string GetMouthPath(int number)
        {
            return GetPath("Mouth", number, MouthCount);
        }

        private static string GetPath(string prefix, int number, int maxCount)
        {
            if (number < 1 || number > maxCount)
                number = 1;

            return $"{prefix}{number:D2}";
        }
    }
}
