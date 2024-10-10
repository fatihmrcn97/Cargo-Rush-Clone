
using UnityEngine;

namespace Packs.Tutorial.Scripts
{
    public interface ITutorialStep
    {
        public int Id { get; }

        //Baslangicda tum steplerin Initi tetiklenir
        public void Init(int currentStepId);

        //If step is not currentStep 
        public void NotCurrentStep();

        //If step is  currentStep call one time
        public void CurrentStepStart();

        //If step is  currentStep call update time
        public bool Execute();

        public void ConditionTrue();

        public void ConditionFalse();

        public bool Condition();
        public void ExitStep();

        public GameObject Destination();
    }
}