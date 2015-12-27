using System;
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
    private static EWaveManagerState m_State = EWaveManagerState.InProcess;
    private static List<Wave> m_WavesToProcess = new List<Wave>();

    public static void InitManager(LevelData aLevelData, EnemyGoal aEnemyGoal, EnemySpawnPoint aEnemySpawnPoint)
    {
        m_State = EWaveManagerState.InProcess;
        CoroutineManager.StartCoroutine(UpdateWaveManager());
        GenerateWave(aLevelData.m_WavesInfo, aEnemyGoal, aEnemySpawnPoint);
    }

    private static void GenerateWave(List<WaveInfo> aWavesInfo, EnemyGoal aEnemyGoal, EnemySpawnPoint aEnemySpawnPoint)
    {
        foreach (WaveInfo waveinfo in aWavesInfo)
        {
            Wave newWave = new Wave(waveinfo, aEnemyGoal, aEnemySpawnPoint);
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

        m_State = EWaveManagerState.Finish;
    }

    private static Wave GetCurrentWaveToProcess()
    {
        return m_WavesToProcess[0];
    }
}