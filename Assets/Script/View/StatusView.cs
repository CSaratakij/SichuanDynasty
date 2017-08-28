using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField]
        GameObject plus;

        [SerializeField]
        GameObject minus;

        [SerializeField]
        Image[] digits;

        [SerializeField]
        Sprite[] spriteOneToNine;

        [SerializeField]
        Color colorPlus;

        [SerializeField]
        Color colorMinus;


        public StatusView()
        {
            digits = new Image[2];
            spriteOneToNine = new Sprite[10];
        }


        public void ShowStatus(string sign, int totalPoint)
        {
            HideAllDigits();
            HideAllSign();

            var isPlus = (sign == "+");
            var isMinus = (sign == "-");

            plus.SetActive(isPlus);
            minus.SetActive(isMinus);

            var digitArry = totalPoint.ToString().ToArray();

            for (int i = 0; i < digitArry.Length; i++) {

                var value = Convert.ToInt32(digitArry[i]);
                value -= 48;

                digits[i].color = isPlus ? colorPlus : colorMinus;
                digits[i].sprite = spriteOneToNine[value];
                digits[i].gameObject.SetActive(true);
            }

            StartCoroutine("_ShowStatusCallBack");
        }

        public void HideAllDigits()
        {
            for (int i = 0; i < digits.Length; i++) {
                digits[i].gameObject.SetActive(false);
            }
        }

        public void HideAllSign()
        {
            plus.SetActive(false);
            minus.SetActive(false);
        }


        IEnumerator _ShowStatusCallBack()
        {
            yield return new WaitForSeconds(0.6f);
            HideAllDigits();
            HideAllSign();
        }
    }
}
