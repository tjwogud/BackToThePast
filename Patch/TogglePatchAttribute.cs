using HarmonyLib;
using System;
using System.Linq;

namespace BackToThePast.Patch
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TogglePatchAttribute : Attribute
    {
        public string Id { get; private set; }
        public bool Disabled { get; private set; } = false;

        public TogglePatchAttribute(string id)
        {
            Id = id;
        }

        public TogglePatchAttribute(string id, params string[] requireds)
        {
            Id = id;
            if (requireds.Select(AccessTools.TypeByName).Any(t => t == null))
                Disabled = true;
        }
    }
}
