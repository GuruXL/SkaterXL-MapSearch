using UnityEngine;
using GameManagement;
using System.Linq;
using Rewired;

namespace MapSearch
{
    public class MenuController : MonoBehaviour
    {
        private LevelSelectionController levelController;
        InputDialogManager InputManager;
        public GameObject PopUpInputGO;
        public MenuInputFieldDialog PopUpInput;
        public bool isResetMapsExecuted = false;

        //bool showMainMenu = false;

        public void Start()
        {
            InputManager = GameStateMachine.Instance.GetComponent<InputDialogManager>();
            levelController = GameStateMachine.Instance.LevelSelectionObject.GetComponent<LevelSelectionController>();
        }

        public void Update()
        {
            if (GameStateMachine.Instance?.CurrentState?.GetType() == typeof(LevelSelectionState))
            {
                CheckInput();
            }

            if (PopUpInputGO != null && PopUpInput != null)
            {
                UpdateSearchString();

                if (Main.settings.searchString == "" && !isResetMapsExecuted)
                {
                    ResetMaps();
                }
            }

            if (GameStateMachine.Instance?.LastState?.GetType() == typeof(LevelSelectionState) && !isResetMapsExecuted)
            {
                ResetMaps();
            }
        }

        void CheckInput()
        {
            Player player = PlayerController.Instance.inputController.player;
            //if (GameStateMachine.Instance.MainPlayer.input.GetButtonDown("X")) // 1.2.6.0
            if (player.GetButtonDown("X"))
            {
                Main.searchFilter.GetMaps();
                CreatePopUpInput();
                //OpenCursor();
            }
            //else if (GameStateMachine.Instance.MainPlayer.input.GetButtonDown("B") && !isResetMapsExecuted) // 1.2.6.0
            else if (player.GetButtonDown("B") && !isResetMapsExecuted)
            {
                ResetMaps();
            }
        }

        void UpdateSearchString()
        {
            if (PopUpInput.inputField.text != Main.settings.searchString)
            {
                isResetMapsExecuted = false;
                Main.settings.searchString = PopUpInput.inputField.text;
                Main.settings.SearchList = Main.searchFilter.FilterArray(Main.searchFilter.Mapnames, Main.settings.searchString);
                FilterMaps();
            }
        }

        public void CreatePopUpInput()
        {
            if (PopUpInputGO == null)
            {
                PopUpInputGO = Instantiate(InputManager.dialogPrefab.gameObject);

                // Scrolls from top of list to bottom (maps won't register until the whole map list is scrolled through) -- look into better fix
                levelController.listView.scrollRect.normalizedPosition = new Vector2(0f, 0f);
                levelController.listView.scrollRect.normalizedPosition = new Vector2(0f, 1f);

                isResetMapsExecuted = false;
            }
        }

        public void FilterMaps()
        {
            for (int i = 0; i < levelController.listView.ItemViews.Count; i++)
            {
                if (!Main.settings.SearchList.Contains(levelController.listView.ItemViews[i].Label.text))
                {
                    levelController.listView.ItemViews[i].gameObject.SetActive(false);
                }
                else
                {
                    levelController.listView.ItemViews[i].gameObject.SetActive(true);
                }
            }
            levelController.listView.UpdateList();
        }

        public void ResetMaps()
        {
            if (levelController != null)
            {

                levelController.UpdateList();

                /*
                for (int i = 0; i < levelController.listView.ItemViews.Count; i++)
                {
                    if (levelController.listView.ItemViews[i].gameObject.activeSelf == false)
                    {
                        levelController.listView.ItemViews[i].gameObject.SetActive(true);
                    }
                }
                */

                isResetMapsExecuted = true;
            }
        }
    }
}
