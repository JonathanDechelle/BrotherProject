﻿using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum EWaveManagerState
{
    InProcess,
    Finish
}

public static class WaveManager
{
    private static List<Wave> m_WavesToProcess = new List<Wave>();

    public static void InitManager(LevelData aLevelData)
    {
        CoroutineManager.StartCoroutine(UpdateWaveManager());
        GenerateWave(aLevelData);
    }

    private static void GenerateWave(LevelData aLevelData)
    {
        foreach (WaveInfo waveinfo in aLevelData.m_WavesInfo)
        {
            Wave newWave = new Wave(waveinfo, aLevelData.m_EnemyGoals, aLevelData.m_EnemySpawnPoint);
            m_WavesToProcess.Add(newWave);
        }
    }

    private static IEnumerator UpdateWaveManager()
    {
        while (m_WavesToProcess.Count > 0)
        {
            GetCurrentWaveToProcess().StartCoolDown();
            while (!GetCurrentWaveToProcess().LaunchAnotherWave())
            {
                yield return null;
            }

            m_WavesToProcess.RemoveAt(0);
        }
    }

    private static Wave GetCurrentWaveToProcess()
    {
        return m_WavesToProcess[0];
    }
}