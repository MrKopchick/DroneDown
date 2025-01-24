using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

namespace ResearchSystem
{
    public class ResearchNodeUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nodeName;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private Button researchButton;
        [SerializeField] private RectTransform progressFillImage;

        [SerializeField] private ResearchNode assignedResearch;
        [SerializeField] private float holdTime = 2f;

        private bool isHolding;
        private float buttonWidth;
        private bool isProgressUpdating;

        private void Start()
        {
            if (assignedResearch == null) return;

            InitializeUI();
            AddEventTrigger(researchButton.gameObject, EventTriggerType.PointerDown, OnPointerDown);
            AddEventTrigger(researchButton.gameObject, EventTriggerType.PointerUp, OnPointerUp);

            if (ResearchManager.Instance.IsNodeActive(assignedResearch))
            {
                StartCoroutine(UpdateResearchProgress());
            }
        }

        private void InitializeUI()
        {
            nodeName.text = assignedResearch.NodeName;
            //description.text = assignedResearch.Description;

            buttonWidth = researchButton.GetComponent<RectTransform>().sizeDelta.x;
            UpdateProgressUI(assignedResearch.Progress);

            RefreshUI();
        }

        public void RefreshUI()
        {
            if (assignedResearch.IsUnlocked)
            {
                SetUIState("Complete", Color.white, false);
                UpdateProgressUI(1f);
            }
            else if (assignedResearch.ArePrerequisitesMet())
            {
                SetUIState("Available", Color.white, true);

                if (ResearchManager.Instance.IsNodeActive(assignedResearch) && !isProgressUpdating)
                {
                    StartCoroutine(UpdateResearchProgress());
                }
            }
            else
            {
                SetUIState("Locked", Color.gray, false);
                UpdateProgressUI(0f);
            }
        }

        private void SetUIState(string text, Color color, bool interactable)
        {
            progressText.text = text;
            progressText.color = color;
            researchButton.interactable = interactable;
        }

        private IEnumerator UpdateResearchProgress()
        {
            isProgressUpdating = true;

            while (!assignedResearch.IsUnlocked)
            {
                UpdateProgressUI(assignedResearch.Progress);
                yield return null;
            }

            isProgressUpdating = false;
            RefreshUI();
        }

        private void UpdateProgressUI(float progress)
        {
            if (progressFillImage != null)
            {
                progressFillImage.sizeDelta = new Vector2(buttonWidth * progress, progressFillImage.sizeDelta.y);
            }

            if (progressText != null)
            {
                progressText.text = $"{(progress * 100f):F1}%";
            }
        }

        private void OnPointerDown(BaseEventData _)
        {
            if (!researchButton.interactable) return;

            isHolding = true;
            AnimateProgressBar(true);
            DOVirtual.DelayedCall(holdTime, StartResearch).SetId(this);
        }

        private void OnPointerUp(BaseEventData _)
        {
            if (!isHolding) return;

            isHolding = false;
            AnimateProgressBar(false);
            DOTween.Kill(this);
        }

        private void StartResearch()
        {
            if (!isHolding) return;

            isHolding = false;

            if (ResearchManager.Instance.StartResearch(assignedResearch))
            {
                StartCoroutine(UpdateResearchProgress());
                RefreshUI();
            }
        }

        private void AnimateProgressBar(bool isFilling)
        {
            float targetWidth = isFilling ? buttonWidth : 0;
            progressFillImage.DOSizeDelta(new Vector2(targetWidth, progressFillImage.sizeDelta.y), holdTime).SetEase(Ease.OutQuad);
        }

        private void AddEventTrigger(GameObject obj, EventTriggerType eventType, System.Action<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = obj.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
            entry.callback.AddListener((data) => { action(data); });
            trigger.triggers.Add(entry);
        }
    }
}
