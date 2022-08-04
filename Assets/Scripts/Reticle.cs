using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : Character
{
    public struct SavedAction
    {
        public int act;
        public CharSwitchManager.MainCharacter characterPerforming;
        public bool facingRight;
    }



    SpriteRenderer sr;


    public List<SavedAction> actionsToSend;
    public List<GameObject> actMarkers;

    public bool movable;
    public Action TeleportAction;
    public CharSwitchManager.MainCharacter retRole;

    public Sprite[] charReticles;
    public GameObject[] moveRepresentationPrefabs;
    protected List<GameObject> allMoveReps;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        movable = true;
        sr.sprite = charReticles[(int)CharSwitchManager.instance.charInPlay];
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.I) && actionsToSend.Count > 0)
        {

            actionsToSend.RemoveAt(actionsToSend.Count - 1);
            if (actionsToSend[actionsToSend.Count-1].act == -1)
            {
                actionsToSend.RemoveAt(actionsToSend.Count - 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            RotateCharacters();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (movable)
            {
                movable = false;
                rb.isKinematic = true;
            }
            else
            {
                movable = true;
                rb.isKinematic = false;
            }
        }


    }


    public void SendActions()
    {
        for (int i = 0; i < actionsToSend.Count; i++)
        {
            PlayableCharacter pc = CharSwitchManager.instance.MainCharacterReferences[(int)actionsToSend[i].characterPerforming].GetComponent<PlayableCharacter>();
            pc.TryAddPerform(pc.moves[actionsToSend[i].act], actionsToSend[i].act); 
        }
    }

    private void RotateCharacters()
    {
        int numSelect = 0;
        for (int i = 0; i < CharSwitchManager.instance.selectable.Length; i++)
        {
            if (CharSwitchManager.instance.selectable[i])
            {
                numSelect += 1;
            }
        }

        if (numSelect < 2)
        {
            return;
        }


        bool validChar = false;
        int a = (int)CharSwitchManager.instance.charInPlay;
        while (!validChar)
        {
            a += 1;
            if (a >= CharSwitchManager.instance.MainCharacterReferences.Length)
            {
                a = 0;
            }
            validChar = CharSwitchManager.instance.selectable[a];
        }

        retRole = (CharSwitchManager.MainCharacter)a;
        sr.sprite = charReticles[a];



    }
}
