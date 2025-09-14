using UnityEngine;
using Cube;

public static class MergeSystem
{
    private const float MERGE_VELOCITY_THRESHOLD = 5f;

    public static bool TryMerge(CubeController cubeA, CubeController cubeB, Collision collision, out int score)
    {
        score = 0;

        if (cubeA.PO2Value != cubeB.PO2Value) return false;

        if (collision.relativeVelocity.magnitude < MERGE_VELOCITY_THRESHOLD) return false;

        CubeController survivor = cubeA.CurrentVelocity > cubeB.CurrentVelocity ? cubeA : cubeB;
        CubeController absorbed = survivor == cubeA ? cubeB : cubeA;

        int total = survivor.PO2Value + absorbed.PO2Value;
        score = total / 4;

        survivor.SetCubePo2Value(total);
        absorbed.DestroySelf();

        return true;
    }
}
