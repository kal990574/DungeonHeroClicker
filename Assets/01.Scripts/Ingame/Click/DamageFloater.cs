using System.Collections;
using UnityEngine;
using TMPro;

namespace _01.Scripts.Ingame.Click
{
    public class DamageFloater : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _damageText;

        public void Show(int damage)
        {
            gameObject.SetActive(true);
            _damageText.text = damage.ToString();
            StartCoroutine(Show_Coroutine());
        }

        private IEnumerator Show_Coroutine()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }
    }
}