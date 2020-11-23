using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textArea = null;
    [SerializeField] private Button backButton = null;
    [SerializeField] private Button forwardButton = null;
    [Tooltip("Type \"[!]\" to indicate that section has a task. " +
        "The task itself must be hard-coded, at the moment.")]
    [TextArea] [SerializeField] private string[] sections = null;
    private int cIndex;

    private class TutorialTask
    {
        public TutorialTask(bool completed, int originalIndex)
        {
            this.completed = completed;
            this.originalIndex = originalIndex;
        }

        public bool completed;
        public int originalIndex;
    }
    private List<TutorialTask> taskList = null;

    private int numIngredients = 0;

    public int CurrentIndex
    {
        get { return cIndex; }
        //When setting the current index,
        set
        {
            if (sections != null)
            {
                cIndex = Mathf.Clamp(value, 0, sections.Length - 1);

                //Set the tutorial window's text to the section at the current index.
                textArea.text = sections[cIndex];

                //Find the task that originated from CurrentIndex, if there is one,
                TutorialTask taskAtCIndex = taskList.Find(task => task.originalIndex == cIndex);
                //and if it doesn't exist or is complete, AND we're not at the last index, make the forward
                //button clickable.
                forwardButton.interactable = (taskAtCIndex == null || taskAtCIndex.completed)
                    && cIndex < sections.Length - 1;

                //Make the back button unclickable if cIndex is 0.
                backButton.interactable = cIndex > 0;
            }
        }
    }

    private void Start()
    {
        InitSectionsAndTasks();

        MixingBowl.IngredientAddedToBowl += UpdateIngredientTask;
        MixingBowl.MixingComplete += CompleteStirringTask;
        MixingBowl.ContentsSubmitted += CompleteSubmitTask;
        CustomerManager.OnDequeueCustomer += CompleteCookingTask;
    }
    private void OnDestroy()
    {
        //MixingBowl.IngredientAddedToBowl = null;
        //MixingBowl.MixingComplete = null;
        //MixingBowl.ContentsSubmitted = null;
        //CustomerManager.OnDequeueCustomer = null;
        MixingBowl.IngredientAddedToBowl += UpdateIngredientTask;
        MixingBowl.MixingComplete += CompleteStirringTask;
        MixingBowl.ContentsSubmitted += CompleteSubmitTask;
        CustomerManager.OnDequeueCustomer += CompleteCookingTask;
    }

    private void InitSectionsAndTasks()
    {
        if (sections != null)
        {
            taskList = new List<TutorialTask>();

            //For each section in the tutorial,
            for (int i = 0; i < sections.Length; i++)
            {
                //If it contains a task marker,
                if (sections[i].Contains("[!]"))
                {
                    //Remove it. Set that section's task to incomplete, and note its original index.
                    sections[i] = sections[i].Replace("[!]", "");
                    taskList.Add(new TutorialTask(false, i));
                }
            }

            CurrentIndex = 0;
        }
    }

    private void UpdateIngredientTask(Ingredient _)
    {
        //Keep track of how many ingredients are added, and once 8 are added,
        numIngredients++;
        if (numIngredients >= 8)
        {
            //Note this task is completed.
            taskList[0].completed = true;

            AutoAdvanceOnComplete(0);
            MixingBowl.IngredientAddedToBowl -= UpdateIngredientTask;
        }
    }
    private void CompleteStirringTask(Dictionary<IngredientAttribute, int> _)
    {
        taskList[1].completed = true;
        AutoAdvanceOnComplete(1);
        MixingBowl.MixingComplete -= CompleteStirringTask;
    }
    private void CompleteSubmitTask(Dictionary<IngredientAttribute, int> _)
    {
        taskList[2].completed = true;
        AutoAdvanceOnComplete(2);
        MixingBowl.ContentsSubmitted -= CompleteSubmitTask;
    }
    private void CompleteCookingTask(Customer _)
    {
        taskList[3].completed = true;
        AutoAdvanceOnComplete(3);
        CustomerManager.OnDequeueCustomer -= CompleteCookingTask;
    }

    public void GoBackward() { CurrentIndex--; }
    public void GoForward() { CurrentIndex++; }

    public void AutoAdvanceOnComplete(int completedTaskIndex)
    {
        if (CurrentIndex == taskList[completedTaskIndex].originalIndex)
        {
            CurrentIndex++;
        }
    }
}