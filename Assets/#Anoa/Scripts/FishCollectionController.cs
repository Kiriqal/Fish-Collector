using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Anoa
{
    public class FishCollectionController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] protected GameObject gameObjectCollectionPanel;
        [SerializeField] protected Button buttonCollectionButton;
        [SerializeField] protected Button buttonCloseCollection;

        [Header("Fish Data References")]
        [SerializeField] protected List<FishData> listCommonFishData;
        [SerializeField] protected List<FishData> listEpicFishData;
        [SerializeField] protected List<FishData> listMythicFishData;

        [Header("Fish Slots")]
        [SerializeField] protected List<Image> listCommonFishSlots;
        [SerializeField] protected List<Image> listEpicFishSlots;
        [SerializeField] protected List<Image> listMythicFishSlots;

        [Header("Tooltip Reference")]
        [SerializeField] protected TooltipManager tooltipManager;

        protected static List<string> listCaughtFishNames = new List<string>();

        protected void Start()
        {
            buttonCollectionButton.onClick.AddListener(ShowCollectionPanel);
            if (buttonCloseCollection != null)
                buttonCloseCollection.onClick.AddListener(CloseCollectionPanel);

            gameObjectCollectionPanel.SetActive(false);
            InitializeFishSlots();
        }

        protected void InitializeFishSlots()
        {
            SetupFishSlotsWithTooltip(listCommonFishSlots, listCommonFishData);
            SetupFishSlotsWithTooltip(listEpicFishSlots, listEpicFishData);
            SetupFishSlotsWithTooltip(listMythicFishSlots, listMythicFishData);
        }

        protected void SetupFishSlotsWithTooltip(List<Image> fishSlots, List<FishData> fishDataList)
        {
            for (int i = 0; i < fishSlots.Count; i++)
            {
                if (i < fishDataList.Count)
                {
                    FishData fish = fishDataList[i];
                    fishSlots[i].sprite = fish.spriteFish;
                    SetFishSlotState(fishSlots[i], false);
                    AddTooltipToSlot(fishSlots[i], fish);
                }
            }
        }

        protected void AddTooltipToSlot(Image fishSlot, FishData fishData)
        {
            EventTrigger trigger = fishSlot.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = fishSlot.gameObject.AddComponent<EventTrigger>();
            }

            // Pointer Enter Event
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => {
                bool isUnlocked = listCaughtFishNames.Contains(fishData.strFishName);
                tooltipManager.ShowTooltip(fishData.strFishName, fishSlot.transform.position, fishData.intRarity, isUnlocked);
            });

            // Pointer Exit Event
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => {
                tooltipManager.HideTooltip();
            });

            trigger.triggers.Add(entryEnter);
            trigger.triggers.Add(entryExit);
        }

        protected void SetFishSlotState(Image fishSlot, bool isUnlocked)
        {
            if (isUnlocked)
            {
                fishSlot.color = Color.white; // Gambar asli
            }
            else
            {
                // +++ BUAT LEBIH GELAP +++
                fishSlot.color = new Color(0.1f, 0.1f, 0.1f, 0.8f); // Hampir hitam
            }
        }

        public void ShowCollectionPanel()
        {
            gameObjectCollectionPanel.SetActive(true);
            UpdateCollectionDisplay();
        }

        public void CloseCollectionPanel()
        {
            gameObjectCollectionPanel.SetActive(false);
        }

        protected void UpdateCollectionDisplay()
        {
            UpdatePanelSlots(listCommonFishSlots, listCommonFishData);
            UpdatePanelSlots(listEpicFishSlots, listEpicFishData);
            UpdatePanelSlots(listMythicFishSlots, listMythicFishData);
        }

        protected void UpdatePanelSlots(List<Image> fishSlots, List<FishData> fishDataList)
        {
            for (int i = 0; i < fishSlots.Count; i++)
            {
                if (i < fishDataList.Count)
                {
                    FishData fish = fishDataList[i];
                    bool isUnlocked = listCaughtFishNames.Contains(fish.strFishName);
                    SetFishSlotState(fishSlots[i], isUnlocked);
                }
            }
        }

        public static void UnlockFish(FishData fishData)
        {
            if (!listCaughtFishNames.Contains(fishData.strFishName))
            {
                listCaughtFishNames.Add(fishData.strFishName);
                Debug.Log($"Fish unlocked: {fishData.strFishName}");
            }
        }

        public static void ResetCollection()
        {
            listCaughtFishNames.Clear();
        }


    }
}