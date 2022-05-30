using UnityEngine;

[CreateAssetMenu(fileName ="New Container", menuName ="Data Container")]
public class DataContainer : ScriptableObject
{
    public int score;
    public int hiScore;
    public int coin;
    public int totalCoin;

    public float volume;
    public bool autopilot;

    public void updateHiScore()
    {
        if(score > hiScore)
        {
            hiScore = score;
        }
    }
}
