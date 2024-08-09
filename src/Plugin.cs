using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace UIFitter
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class UIPluginFit : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymod".
        public const string GUID = "ikennyagain.uifitter";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "UI Fitter";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "0.0.1";

        // For accessing your BepInEx Logger from outside of this class (eg Plugin.Log.LogMessage("");)
        internal static ManualLogSource Log;

        // If you need settings, define them like so:
        public static ConfigEntry<bool> ExampleConfig;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;
            Log.LogMessage($"Hello world from {NAME} {VERSION}!");

            // Any config settings you define should be set up like this:
            ExampleConfig = Config.Bind("ExampleCategory", "ExampleSetting", false, "This is an example setting.");

            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            new Harmony(GUID).PatchAll();
        }

        // Update is called once per frame. Use this only if needed.
        // You also have all other MonoBehaviour methods available (OnGUI, etc)
        internal void Update()
        {

        }

        // This is an example of a Harmony patch.
        // If you're not using this, you should delete it.
        [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
        public class CharacterAwakePatch
        {
            public const float ShopMenuXOffset = 900;
            public const float ShopMenuYOffset = 520;

            [HarmonyPostfix]
            public static void Postfix(Character __instance)
            {
                ///check this character is not an AI, they dont have a UI (well acutally they do its really messed up)
                if (!__instance.IsAI)
                {
                    //only affect this character this patch is running on if it is the world host
                    if (__instance.IsWorldHost)
                    {
                        RectTransform foundRectTransform = __instance.CharacterUI.transform.Find("GameplayPanels/Menus/ModalMenus/ShopMenu").GetComponent<RectTransform>();
                        if (foundRectTransform != null)
                        {
                            foundRectTransform.offsetMax = new Vector2(900, 520);
                        }
                        else
                        {
                            //log error we did not find the component at path
                        }
                    }
                }
            }
        }
    }
}
