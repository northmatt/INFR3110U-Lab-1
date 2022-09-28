using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public PlayerAction playerInput;
    public GameObject player;
    public bool gamePaused = false;
    public bool CursorLocked = false;
    public byte collectables = 0;

    private void Awake() {
        playerInput = new PlayerAction();
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    private void Start() {
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindGameObjectWithTag("Player");

        CursorHidden(true);
    }

    public void DoPause(bool isPaused) {
        gamePaused = isPaused;
        CursorHidden(!gamePaused);
    }

    void CursorHidden(bool isHidden) {
        CursorLocked = isHidden;
        Cursor.lockState = CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !CursorLocked;
    }
}
