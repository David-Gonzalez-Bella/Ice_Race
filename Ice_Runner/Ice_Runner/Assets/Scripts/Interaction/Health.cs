using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //Variables
    public int maxLifes;
    private int currentLifes;
    public int CurrentLifes
    {
        get
        {
            return currentLifes;
        }

        set
        {
            if (value > maxLifes)
            {
                currentLifes = maxLifes;
            }
            else if (value < 0)
            {
                currentLifes = 0;
            }
            else
            {
                currentLifes = value;
            }
            UpdateLifesText(); //Every time lifes are updated the UI will be updated as well
        }
    }

    void Start()
    {
        CurrentLifes += maxLifes;
    }

    private void UpdateLifesText() //Call the lifes update method in the UI
    {
        UI_Manager.sharedInstance.UpdateLifesText(this);
    }

    public void ModifyLifes(int quantity)
    {
        CurrentLifes += quantity;
    }
}
