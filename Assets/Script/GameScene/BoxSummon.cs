using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSummon : MonoBehaviour
{ 
    public void Summon_ST()
    {
        GameAreaEdit.Instance.SummonRandomPerfab_ST();
    }
    public void Summon_ND()
    {
        GameAreaEdit.Instance.SummonRandomPerfab_ND();
    }
}
