using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// show reward and condition
/// </summary>
public class AchievementEmblem : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] Text conditionText;
    [SerializeField] Text rewardText;

    /// <summary>
    /// gets id
    /// </summary>
    public int Id
    {
        get { return id; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    ///  set the Condition text to message
    /// </summary>
    /// <param name="message">message</param>
    public void SetConditionText(string message)
    {
        conditionText.text = message;
    }

    /// <summary>
    /// set the Reward text to message
    /// </summary>
    /// <param name="message">message</param>
    public void SetRewardText(string message)
    {
        rewardText.text = message;
    }
}
