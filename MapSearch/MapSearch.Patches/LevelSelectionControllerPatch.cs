using HarmonyLib;
using UnityEngine;
using GameManagement;
using UnityEngine.EventSystems;

namespace MapSearch.Patches
{
    public static class LevelSelectionControllerPatch
    {
        [HarmonyPatch(typeof(LevelSelectionState), "OnEnter")]
        public static class LevelSelectionPatch
        {
            private static MenuButton searchButton;

            [HarmonyPriority(200)]
            private static void Postfix(ref LevelSelectionState __instance)
            {
                if (searchButton == null)
                {
                    GameObject originalButton = GetOGBackButton(__instance);
                    GameObject newButton = Object.Instantiate(originalButton, originalButton.transform.parent);
                    newButton.transform.SetSiblingIndex(1);
                    newButton.name = "SearchButton";

                    searchButton = newButton.GetComponent<MenuButton>();
                    MenuButton backbutton = originalButton.GetComponent<MenuButton>();
                    searchButton.gameObject.SetActive(true);
                    searchButton.GreyedOut = false;
                    searchButton.GreyedOutInfoText = "Map Search";
                    searchButton.Label.SetText("Search For Map");
                    searchButton.Label.alignment = TMPro.TextAlignmentOptions.Center;
                    searchButton.Label.characterSpacing = 2f;
                    var pos = searchButton.gameObject.GetComponent<RectTransform>();
                    pos.anchoredPosition = new Vector2(0, 12f);
                    pos.offsetMin.Set(10f, pos.offsetMin.y);

                    backbutton.onClick.RemoveAllListeners();
                    searchButton.onClick.RemoveAllListeners();  // Remove existing listeners
                    searchButton.onClick.AddListener(() => SearchButtonOnClick());  // Add new listener
                }
            }
            private static GameObject GetOGBackButton(LevelSelectionState __instance)
            {
                Transform button = __instance.gameObject.transform.FindChildRecursively("Back Button");

                if (button != null)
                {
                    return button.gameObject;
                }
                return null;
            }

            public static void SearchButtonOnClick()
            {
                //GameStateMachine.Instance.RequestLevelSelectionState();
                Main.searchFilter.GetMaps();
                Main.menuctrl.CreatePopUpInput();
            }
        }
    }
}
