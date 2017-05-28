using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPDisplay : MonoBehaviour
{

    public static HPDisplay instance;

    private int HPForDisplay;
    public Unit thisUnit = instance.transform.parent.transform.GetComponent<Unit>();

    [SerializeField]
    private Text txtRef;

    private void Awake()
    {
        txtRef = GetComponent<Text>();//or provide from somewhere else (e.g. if you want via find GameObject.Find("CountText").GetComponent<Text>();)
        //unitRef = GetComponent<Unit>();
    }
    //then where you need:
    private void Start()
    {
        HPForDisplay = 10;
        txtRef.text = "";
    }

    private void Update()
    {
        UpdateHPIndicator();
    }

    public void UpdateHPForDisplay(int i)
    {
        instance.HPForDisplay = i;
    }

    public void UpdateHPIndicator()
    {
        if (thisUnit is HQ)
        {
            // there is a more efficient way of doing this. please do it
            if (thisUnit.HP < 100 && thisUnit.HP > 0)
            {
                if (thisUnit.HP == 1)
                    txtRef.text = "1";
                else if (thisUnit.HP == 2)
                    txtRef.text = "2";
                else if (thisUnit.HP == 3)
                    txtRef.text = "3";
                else if (thisUnit.HP == 4)
                    txtRef.text = "4";
                else if (thisUnit.HP == 5)
                    txtRef.text = "5";
                else if (thisUnit.HP == 6)
                    txtRef.text = "6";
                else if (thisUnit.HP == 7)
                    txtRef.text = "7";
                else if (thisUnit.HP == 8)
                    txtRef.text = "8";
                else if (thisUnit.HP == 9)
                    txtRef.text = "9";

                else if (thisUnit.HP == 10)
                    txtRef.text = "10";
                else if (thisUnit.HP == 11)
                    txtRef.text = "11";
                else if (thisUnit.HP == 12)
                    txtRef.text = "12";
                else if (thisUnit.HP == 13)
                    txtRef.text = "13";
                else if (thisUnit.HP == 14)
                    txtRef.text = "14";
                else if (thisUnit.HP == 15)
                    txtRef.text = "15";
                else if (thisUnit.HP == 16)
                    txtRef.text = "16";
                else if (thisUnit.HP == 17)
                    txtRef.text = "17";
                else if (thisUnit.HP == 18)
                    txtRef.text = "18";

                else if (thisUnit.HP == 19)
                    txtRef.text = "19";


                else if (thisUnit.HP == 20)
                    txtRef.text = "20";
                else if (thisUnit.HP == 21)
                    txtRef.text = "21";
                else if (thisUnit.HP == 22)
                    txtRef.text = "22";
                else if (thisUnit.HP == 23)
                    txtRef.text = "23";
                else if (thisUnit.HP == 24)
                    txtRef.text = "24";
                else if (thisUnit.HP == 25)
                    txtRef.text = "25";
                else if (thisUnit.HP == 26)
                    txtRef.text = "26";
                else if (thisUnit.HP == 27)
                    txtRef.text = "27";
                else if (thisUnit.HP == 28)
                    txtRef.text = "28";
                else if (thisUnit.HP == 29)
                    txtRef.text = "29";

                else if (thisUnit.HP == 30)
                    txtRef.text = "30";
                else if (thisUnit.HP == 31)
                    txtRef.text = "31";
                else if (thisUnit.HP == 32)
                    txtRef.text = "32";
                else if (thisUnit.HP == 33)
                    txtRef.text = "33";
                else if (thisUnit.HP == 34)
                    txtRef.text = "34";
                else if (thisUnit.HP == 35)
                    txtRef.text = "35";
                else if (thisUnit.HP == 36)
                    txtRef.text = "36";
                else if (thisUnit.HP == 37)
                    txtRef.text = "37";
                else if (thisUnit.HP == 38)
                    txtRef.text = "38";
                else if (thisUnit.HP == 39)
                    txtRef.text = "39";
                else if (thisUnit.HP == 40)
                    txtRef.text = "40";
                else if (thisUnit.HP == 41)
                    txtRef.text = "41";
                else if (thisUnit.HP == 42)
                    txtRef.text = "42";
                else if (thisUnit.HP == 43)
                    txtRef.text = "43";
                else if (thisUnit.HP == 44)
                    txtRef.text = "44";
                else if (thisUnit.HP == 45)
                    txtRef.text = "45";
                else if (thisUnit.HP == 46)
                    txtRef.text = "46";
                else if (thisUnit.HP == 47)
                    txtRef.text = "47";
                else if (thisUnit.HP == 48)
                    txtRef.text = "48";
                else if (thisUnit.HP == 49)
                    txtRef.text = "49";
                else if (thisUnit.HP == 50)
                    txtRef.text = "50";
                else if (thisUnit.HP == 51)
                    txtRef.text = "51";
                else if (thisUnit.HP == 52)
                    txtRef.text = "52";
                else if (thisUnit.HP == 53)
                    txtRef.text = "53";
                else if (thisUnit.HP == 54)
                    txtRef.text = "54";
                else if (thisUnit.HP == 55)
                    txtRef.text = "55";
                else if (thisUnit.HP == 56)
                    txtRef.text = "56";
                else if (thisUnit.HP == 57)
                    txtRef.text = "57";
                else if (thisUnit.HP == 58)
                    txtRef.text = "58";
                else if (thisUnit.HP == 59)
                    txtRef.text = "59";
                else if (thisUnit.HP == 60)
                    txtRef.text = "60";
                else if (thisUnit.HP == 61)
                    txtRef.text = "61";
                else if (thisUnit.HP == 62)
                    txtRef.text = "62";
                else if (thisUnit.HP == 63)
                    txtRef.text = "63";
                else if (thisUnit.HP == 64)
                    txtRef.text = "64";
                else if (thisUnit.HP == 65)
                    txtRef.text = "65";
                else if (thisUnit.HP == 66)
                    txtRef.text = "66";
                else if (thisUnit.HP == 67)
                    txtRef.text = "67";
                else if (thisUnit.HP == 68)
                    txtRef.text = "68";
                else if (thisUnit.HP == 69)
                    txtRef.text = "69";
                else if (thisUnit.HP == 70)
                    txtRef.text = "70";
                else if (thisUnit.HP == 71)
                    txtRef.text = "71";
                else if (thisUnit.HP == 72)
                    txtRef.text = "72";
                else if (thisUnit.HP == 73)
                    txtRef.text = "73";
                else if (thisUnit.HP == 74)
                    txtRef.text = "74";
                else if (thisUnit.HP == 75)
                    txtRef.text = "75";
                else if (thisUnit.HP == 76)
                    txtRef.text = "76";
                else if (thisUnit.HP == 77)
                    txtRef.text = "77";
                else if (thisUnit.HP == 54)
                    txtRef.text = "78";
                else if (thisUnit.HP == 78)
                    txtRef.text = "79";
                else if (thisUnit.HP == 79)
                    txtRef.text = "79";
                else if (thisUnit.HP == 80)
                    txtRef.text = "80";
                else if (thisUnit.HP == 81)
                    txtRef.text = "81";
                else if (thisUnit.HP == 82)
                    txtRef.text = "82";
                else if (thisUnit.HP == 83)
                    txtRef.text = "83";
                else if (thisUnit.HP == 84)
                    txtRef.text = "84";
                else if (thisUnit.HP == 85)
                    txtRef.text = "85";
                else if (thisUnit.HP == 86)
                    txtRef.text = "86";
                else if (thisUnit.HP == 87)
                    txtRef.text = "87";
                else if (thisUnit.HP == 88)
                    txtRef.text = "88";
                else if (thisUnit.HP == 89)
                    txtRef.text = "89";
                else if (thisUnit.HP == 90)
                    txtRef.text = "90";
                else if (thisUnit.HP == 91)
                    txtRef.text = "91";
                else if (thisUnit.HP == 92)
                    txtRef.text = "92";
                else if (thisUnit.HP == 93)
                    txtRef.text = "93";
                else if (thisUnit.HP == 94)
                    txtRef.text = "94";
                else if (thisUnit.HP == 95)
                    txtRef.text = "95";
                else if (thisUnit.HP == 96)
                    txtRef.text = "96";
                else if (thisUnit.HP == 97)
                    txtRef.text = "97";
                else if (thisUnit.HP == 98)
                    txtRef.text = "98";
                else if (thisUnit.HP == 99)
                    txtRef.text = "99";
            }
        }
        else
        if (thisUnit.HP < 10 && thisUnit.HP > 0)
        {
            if (thisUnit.HP == 1)
                txtRef.text = "1";
            else if (thisUnit.HP == 2)
                txtRef.text = "2";
            else if (thisUnit.HP == 3)
                txtRef.text = "3";
            else if (thisUnit.HP == 4)
                txtRef.text = "4";
            else if (thisUnit.HP == 5)
                txtRef.text = "5";
            else if (thisUnit.HP == 6)
                txtRef.text = "6";
            else if (thisUnit.HP == 7)
                txtRef.text = "7";
            else if (thisUnit.HP == 8)
                txtRef.text = "8";
            else if (thisUnit.HP == 9)
                txtRef.text = "9";
        }
    }
    /*
    if (HPForDisplay < 10)
    {
        txtRef.text = "8";
    }    */
}