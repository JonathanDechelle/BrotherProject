using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// Keeps track of all running coroutines, and runs them till the end.
static class CoroutineManager
{
    public static List<IEnumerator> Coroutines = new List<IEnumerator>();
    public static void StartCoroutine(IEnumerator item)
    {
        Coroutines.Add(item);
    }

    public static void StopCoroutine(IEnumerator item)
    {
        Coroutines.Remove(item);
    }
}