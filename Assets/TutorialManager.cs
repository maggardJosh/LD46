using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;

    public static TutorialManager Instance
    {
        get
        {
            if(_instance == null)
                throw new Exception("No Tutorial Manager");
            return _instance;
        }
    }

    public TextMeshProUGUI tutorialLabel;
    private Animator _animator;
    void Awake()
    {
        _instance = this;
        _animator = GetComponent<Animator>();
    }

    public enum TutorialStep
    {
        Move,
        Jump,
        Attack,
        AttackDone,
        Pickup,
        ReturnPickup,
        DoubleJump,
        DoubleJumpDone,
        Slam,
        SlamDone,
        Vine,
        VineDone
    }

    private TutorialStep _currentStep = TutorialStep.Move;

    public TutorialStep CurrentStep => _currentStep;

    public void SetTutorialStep(TutorialStep newStep)
    {
        if(PastTutorial(newStep))
            return;
        _currentStep = newStep;
        _animator.SetTrigger(NextTutorial);
        _animator.SetBool(HasText, !String.IsNullOrWhiteSpace(GetCurrentTutorialText()));

    }

    public bool PastTutorial(TutorialStep step)
    {
        return (int) step <= (int) _currentStep;
    }
    public void UpdateText()
    {
        tutorialLabel.text = GetCurrentTutorialText();
    }

    public string GetCurrentTutorialText()
    {
        if (TutorialText.TryGetValue(_currentStep, out string result))
            return result;
        return "";
    }
    private static Dictionary<TutorialStep, string> TutorialText = new Dictionary<TutorialStep, string>
    {
        {TutorialStep.Move, "Move with WASD (or ZQSD)"},
        {TutorialStep.Jump, "Jump with K"},
        {TutorialStep.Attack, "Attack with J"},
        {TutorialStep.Pickup, "Pickup Flower with J"},
        {TutorialStep.ReturnPickup, "Return flower to garden, Keep It Alive!"},
        {TutorialStep.DoubleJump, "You are now able to jump midair!"},
        {TutorialStep.Slam, "You can slam now! Down + J in midair"},
        {TutorialStep.Vine, "You can now use vines! Up + J to get to even higher places!"}
    };

    private static readonly int NextTutorial = Animator.StringToHash("NextTutorial");
    private static readonly int HasText = Animator.StringToHash("HasText");
}
