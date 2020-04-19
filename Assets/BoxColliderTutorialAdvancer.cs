using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxColliderTutorialAdvancer : MonoBehaviour
{
    public TutorialManager.TutorialStep tutorialStep = TutorialManager.TutorialStep.Move;
    private BoxCollider2D _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.Instance.PastTutorial(tutorialStep))
            return;
        
        if(_collider.OverlapPoint(FindObjectOfType<PlayerController>().transform.position))
            TutorialManager.Instance.SetTutorialStep(tutorialStep);
    }
}
