using System.Collections.Generic;
using UnityEngine; 

namespace Packs.Tutorial.Scripts
{
    public class TutorialController : MonoBehaviour
    {
        private static TutorialController Instance;
        
        public GameObject playerArrow;

        private List<ITutorialStep> steps = new(); 
        
        public bool isComplated = false;

        public int currentStep = 0;

        private const string CompleteKey = "TutorialIsCompleted";
        private const string TutorialCurrentStepKey = "TutorialStep";

        public delegate void VoidAction();
 
        private void Awake()
        {
            SingletonPrepare();
            FillDatas();
        }

        private void SingletonPrepare()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            steps.ForEach((step => step.Init(currentStep)));
        }

        private void Update()
        {
            if (isComplated) return;
            Execute();
            SetPlayerArrowDirection();
        }

        private void SetPlayerArrowDirection()
        {
            if (currentStep >= steps.Count || playerArrow == null) return;
            var arrowY = playerArrow.transform.position.y;
            GameObject go = steps[currentStep].Destination();
            Vector3 destinationPoint = go.transform.position;
            destinationPoint.y = arrowY;
            if (go == null) return;
            playerArrow.transform.LookAt(destinationPoint);
        }

        private void FillDatas()
        {
            isComplated = bool.Parse(PlayerPrefs.GetString(CompleteKey, "false"));
            currentStep = PlayerPrefs.GetInt(TutorialCurrentStepKey, 0);

            List<ITutorialStep> childs = new();
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                if (transform.GetChild(0).GetChild(i).TryGetComponent(out ITutorialStep tutorialStep))
                    childs.Add(tutorialStep);
            }

            childs.ForEach((step => steps.Add(null)));
            childs.ForEach((step => steps[step.Id] = step));
            if (playerArrow != null) playerArrow.SetActive(!isComplated);
        }


        private void NextStep()
        {
            currentStep++;
            PlayerPrefs.SetInt(TutorialCurrentStepKey, currentStep); 
            if (currentStep >= steps.Count)
                TutorialCompleted();

            else
                NextStepTransection();
        }

        private void NextStepTransection()
        {
            steps[currentStep - 1].ExitStep();
            steps[currentStep].Init(currentStep);
        }

        private void TutorialCompleted()
        { 
            isComplated = true;
            PlayerPrefs.SetString(CompleteKey, isComplated.ToString());
            playerArrow.SetActive(false);
            #if UNITY_EDITOR
            Debug.Log("Tutorial Completed..");
            #endif
        }

        public void Execute()
        {
            if (isComplated) return;
            var currentStep = steps[this.currentStep];
            var isValid = currentStep.Execute();
            if (isValid) NextStep();
        }

    }
}