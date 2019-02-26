﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using SeedQuest.Level;

[System.Serializable]
public class LevelNameProps {
    public Sprite background;
    public float opacity = 1.0f;
}

public enum LevelNameBackground { Clear, SolidBlue, TransparentBlack }

public class LevelNameUI : MonoBehaviour
{
    public LevelNameBackground background = LevelNameBackground.Clear;
    public LevelNameProps[] backgrounds;

    private Image backgroundImage;
    private TextMeshProUGUI levelNameUI;
    private TextMeshProUGUI zoneNameui;
    private Transform player;

    void Start() {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player = playerObject.transform;
        else
            Debug.LogError("Error: Player is missing from Scene.");

        backgroundImage = GetComponentInChildren<Image>();
        TextMeshProUGUI[] textmesh = GetComponentsInChildren<TextMeshProUGUI>();
        levelNameUI = textmesh[0];
        zoneNameui = textmesh[1];

        UpdateText();
        UpdateBackground();
    }

    private void Update() {
        UpdateText();
        UpdateBackground();
    }

    void UpdateText() {
        levelNameUI.text = LevelManager.LevelName;

        BoundingBox bb = LevelManager.GetBoundingBoxPlayerIsIn();
        if (bb != null)
            zoneNameui.text = bb.name;
        else
            zoneNameui.text = "";
    } 

    void UpdateBackground() {
        int index = 0;
        if (background == LevelNameBackground.Clear)
            index = 0;
        else if (background == LevelNameBackground.SolidBlue)
            index = 1;
        else if (background == LevelNameBackground.TransparentBlack)
            index = 2;

        backgroundImage.sprite = backgrounds[index].background;
        var color = backgroundImage.color;
        color.a = backgrounds[index].opacity;
        backgroundImage.color = color;
    }
} 