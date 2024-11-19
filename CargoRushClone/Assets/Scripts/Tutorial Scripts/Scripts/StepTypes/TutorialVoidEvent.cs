using EventScripts.GameAssets.GameEvents;
using UnityEngine;

namespace Packs.Tutorial.Scripts
{
    

    public class TutorialVoidEvent : TutorialStep, ITutorialStep
    {
    
        public bool condition = false;
        public int id;
        public int Id { get=>id; }

        [SerializeField] private VoidEvent followEvent;
        [SerializeField] private bool isWait;

        private TutorialController _tutorialController;
        private void OnDisable()
        {
            if(followEvent!=null)
                followEvent.RemoveListener(EventActivated);
        }

        private void Awake()
        {
            _tutorialController = GetComponentInParent<TutorialController>();
        }

        private void EventActivated()
        {
            // Next Stebe burda gececek
            condition = true;
        }

        public void Init(int currentStepId)
        {
            if (id == currentStepId) CurrentStepStart();
            else NotCurrentStep();
        }

        public void NotCurrentStep()
        {
            arrow.SetActive(false); 
        }

        public void CurrentStepStart()
        { 
            followEvent.AddListener(EventActivated);
            arrow.SetActive(true);
            if (!isWait) return;
            _tutorialController.playerArrow.SetActive(false);
            arrow.SetActive(false);
        }

        public bool Execute()
        {
            var conditionResult = Condition();
            if (conditionResult) ConditionTrue();
            else ConditionFalse();
            return conditionResult;
        }

        public void ConditionTrue()
        {
            arrow.SetActive(false);
        }

        public void ConditionFalse()
        {
        
        }

        public bool Condition()
        { 
            return condition;
        }

        public void ExitStep()
        {
            // destinationObj.GetComponent<Renderer>().sharedMaterial.color = Color.red;
            _tutorialController.playerArrow.SetActive(true);
        }

        public GameObject Destination()
        {
            return destinationObj;
        }
    }}