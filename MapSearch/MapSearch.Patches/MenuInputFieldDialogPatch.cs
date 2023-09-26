using HarmonyLib;
using UnityEngine;
using GameManagement;

namespace MapSearch.Patches
{
    public static class MenuInputFieldDialogPatch
    {
        [HarmonyPatch(typeof(MenuInputFieldDialog), "Awake")]
        public static class AwakePatch
        {
            [HarmonyPriority(200)]
            private static void Postfix(MenuInputFieldDialog __instance)
            {
                if (GameStateMachine.Instance.CurrentState.GetType() != typeof(LevelSelectionState)) 
                    return;

                Main.menuctrl.PopUpInput = null;

                __instance.titleText.text = "Search for Map";
                __instance.maxLength = 200;
                var canvas = __instance.gameObject.GetComponent<Canvas>();
                var background = __instance.transform.Find("ModalBackground");
                background.gameObject.SetActive(false);
                canvas.overrideSorting = true;
                canvas.sortingOrder = 1;

                Main.menuctrl.PopUpInput = __instance;
            }
        }
    }
}
