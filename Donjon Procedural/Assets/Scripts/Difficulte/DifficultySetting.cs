using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDifficultySetting", menuName = "New Difficulty Setting")]
public class DifficultySetting : ScriptableObject
{
    public AnimationCurve courbeDeDifficulte;
    public AnimationCurve courbeEasy;
    public AnimationCurve courbeIntermediate;
    public AnimationCurve courbeDificult;
}
