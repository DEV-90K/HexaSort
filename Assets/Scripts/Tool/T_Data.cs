using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Data : PersistentMonoSingleton<T_Data>
{
    public ChallengeData _challengeData = null;
    public ChallengePresenterData _presenterData = null;

    public Dictionary<string, T_HexaInBoardData> _hexasSelected = new Dictionary<string, T_HexaInBoardData>();
    public int colorNumber = 0;
    public int hexInEachHexaNumber = 0;
}
