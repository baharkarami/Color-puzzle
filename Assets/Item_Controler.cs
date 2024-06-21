using UnityEngine;
using TMPro;

public class Item_Controler : MonoBehaviour
{
    public string AssignedColorName;
    public Game_Server Game_Server;
    public string derction;

    void Update()
    {
        // بررسی سیستم عامل
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            
         CheckInput();
        }

        

    }

    // تابع برای بررسی ورودی‌های کیبورد
    void CheckInput()
    {
        if (derction == "U" && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Game_Server.VerifyColorChoice(AssignedColorName);
        }
        else if (derction == "D" && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Game_Server.VerifyColorChoice(AssignedColorName);
        }
        else if (derction == "L" && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Game_Server.VerifyColorChoice(AssignedColorName);
        }
        else if (derction == "R" && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Game_Server.VerifyColorChoice(AssignedColorName);
        }
    }


    public void Check_Color_In_Android()
    {
        if (Application.platform == RuntimePlatform.Android )
        {
            Game_Server.VerifyColorChoice(AssignedColorName);
        }
    }
   
}
