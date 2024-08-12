using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using static MapMagic.ObjectPool;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace UIFitter
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class IKennyUIFitter : BaseUnityPlugin
    {
        public const string GUID = "ikennyagain.com.uifitter";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "UI Fitter";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "0.0.1";

        // For accessing your BepInEx Logger from outside of this class (eg Plugin.Log.LogMessage("");)
        internal static ManualLogSource Logging;

        // If you need settings, define them like so:
        public static ConfigEntry<bool> ExampleConfig;

        public static IKennyUIFitter Instance;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Logging = this.Logger;
            Instance = this;
            Logging.LogMessage($"Hello world from {NAME} {VERSION}!");
            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            new Harmony(GUID).PatchAll();
        }

        public static void Log(object message)
        {
            IKennyUIFitter.Logging.LogMessage($"UIPluginFit :" + message);
        }

        public static void DelayDo(Action ToDo, float Delay = 2f)
        {
            IKennyUIFitter.Instance.StartCoroutine(DelayDoRoutine(ToDo, Delay));
        }

        public static IEnumerator DelayDoRoutine(Action ToDo, float Delay = 2f)
        {
            yield return new WaitForSeconds(Delay);
            ToDo?.Invoke();
            yield break;
        }
    }

    // This is an example of a Harmony patch.
    // If you're not using this, you should delete it.
    [HarmonyPatch(typeof(Character), nameof(Character.Start))]
    public class CharacterStartPatch
    {
        public const float ShopMenuXOffset = 900;
        public const float ShopMenuYOffset = 520;

        public const string PathToShopUI = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu";

        public const string PathToShopNameLabel = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu/TopPanel/Shop PanelTop/lblShopName";

        public const string PathToShopPlayerInvLabel = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu/TopPanel/Shop PanelTop/lblPlayerInventory";

        [HarmonyPostfix]
        public static void Postfix(Character __instance)
        {
            IKennyUIFitter.DelayDo(() =>
            {
                if (!__instance.IsAI && __instance.CharacterUI != null)
                {
                    IKennyUIFitter.Log($"AWAKE CALLED FOR {__instance.Name}");                   
                    ModifyRectSizeDeltaAtPath(__instance.CharacterUI, PathToShopUI, new Vector2(900, 520));


                    ///change the offset min and offset max of the Shop Name Label (lbl)
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToShopNameLabel, new Vector2(0, 6), new Vector2(500, 34));
                }

            }, 4f);

        }
        public static void ModifyRectSizeDeltaAtPath(CharacterUI CharacterUI, string RectTransformPath, Vector2 NewSize)
        {
            IKennyUIFitter.Log($"{CharacterUI.transform} root.");
            IKennyUIFitter.Log($"Attempting to find {CharacterUI.transform}/{RectTransformPath}.");
            RectTransform foundRectTransform = GetRectTransformAtPath(CharacterUI, RectTransformPath);
            if (foundRectTransform != null)
            {
                foundRectTransform.sizeDelta = NewSize;
            }
        }

        public static void ModifyRectOffsetAtPath(CharacterUI CharacterUI, string RectTransformPath, Vector2 Min, Vector2 Max)
        {
            IKennyUIFitter.Log($"{CharacterUI.transform} root.");
            IKennyUIFitter.Log($"Attempting to find {CharacterUI.transform}/{RectTransformPath}.");
            RectTransform foundRectTransform = GetRectTransformAtPath(CharacterUI, RectTransformPath);
            if (foundRectTransform != null)
            {
                foundRectTransform.offsetMin = Min;
                foundRectTransform.offsetMax = Max;
            }
        }

        public static RectTransform GetRectTransformAtPath(CharacterUI CharacterUI, string RectTransformPath)
        {
            RectTransform foundRectTransform = CharacterUI.transform.Find(RectTransformPath).GetComponent<RectTransform>();
            if (foundRectTransform != null)
            {
                IKennyUIFitter.Log($"Found RectTransform");
                return foundRectTransform;
            }
            else
            {
                IKennyUIFitter.Log($"Did not find RectTransform");
                return null;
            }
        }
    }



}
