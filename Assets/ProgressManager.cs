using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    static ProgressManager _instance;
    public static ProgressManager Instance { get { return _instance; } }
    public CurrentStage stage = CurrentStage.overworld;
    public MusicManager musicManager;

    public GameObject lavaLight;
    public GameObject castleDeathPlane;
    public GameObject castlePlatformDeathPlane;

    private bool FirstLoad = true;

    private void Awake()
    {
        _instance = this;
    }

    public void NextStage()
    {
        switch(stage)
        {
            case CurrentStage.overworld:
                SetStage(CurrentStage.castle);
                break;
            case CurrentStage.castle:
                SetStage(CurrentStage.bossFight);
                break;
            case CurrentStage.bossFight:
                SetStage(CurrentStage.bossWin);
                break;
        }

    }

    public void SetStage(CurrentStage stage)
    {
        if(stage == this.stage)
        {
            return;
        }

        this.stage = stage;

        switch(stage)
        {
            case CurrentStage.overworld:
                OverworldStage();
                break;

            case CurrentStage.castle:
                CastleStage();
                break;

            case CurrentStage.bossFight:
                BossBattleStage();
                break;
            case CurrentStage.bossWin:
                BossWinStage();
                break;
        }
    }

    public void SetStage(CurrentStage stage, bool instant)
    {
        if (!FirstLoad && stage == this.stage)
        {
            return;
        }

        this.stage = stage;

        switch (stage)
        {
            case CurrentStage.overworld:
                OverworldStage(instant);
                break;

            case CurrentStage.castle:
                CastleStage(instant);
                break;

            case CurrentStage.bossFight:
                BossBattleStage(instant);
                break;
            case CurrentStage.bossWin:
                BossWinStage(instant);
                break;
        }
    }

    public void OverworldStage()
    {
        musicManager.SwapMusicSmooth(0);
    }

    public void CastleStage()
    {
        musicManager.SwapMusicSmooth(1);
        lavaLight.SetActive(true);
        castleDeathPlane.SetActive(true);
        if (castlePlatformDeathPlane.activeSelf == true)
        {
            castlePlatformDeathPlane.SetActive(false);
        }
    }

    public void BossBattleStage()
    {
        musicManager.SwapMusicSmooth(2);
    }

    public void BossWinStage()
    {
        musicManager.SwapMusicSmooth(3);
    }

    public void OverworldStage(bool instant)
    {
        musicManager.SwapMusicInstant(0);
    }

    public void CastleStage(bool instant)
    {
        musicManager.SwapMusicInstant(1);
        lavaLight.SetActive(true);
        castleDeathPlane.SetActive(true);
        if(castlePlatformDeathPlane.activeSelf == true)
        {
            castlePlatformDeathPlane.SetActive(false);
        }
    }

    public void BossBattleStage(bool instant)
    {
        musicManager.SwapMusicInstant(2);
    }

    public void BossWinStage(bool instant)
    {
        musicManager.SwapMusicInstant(3);
    }
}

public enum CurrentStage
{
    overworld,
    castle,
    bossFight,
    bossWin
}
