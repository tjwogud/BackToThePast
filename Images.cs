using System.Reflection;
using UnityEngine;

namespace BackToThePast
{
    public static class Images
    {
        public static Sprite swirl_cw;
        public static Sprite swirl_ccw;
        public static Sprite swirl_cw_outline;
        public static Sprite swirl_ccw_outline;
        public static Sprite arrow_cw;
        public static Sprite arrow_ccw;
        public static Sprite arrow_cw_outline;
        public static Sprite arrow_ccw_outline;

        public static Sprite editor_button_fill;

        public static void Load(AssetBundle bundle)
        {
            foreach (FieldInfo field in typeof(Images).GetFields())
                field.SetValue(null, bundle.LoadAsset<Sprite>(field.Name));
        }
    }
}
