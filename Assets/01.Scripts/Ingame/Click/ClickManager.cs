using _01.Scripts.Ingame.Hero;
using _01.Scripts.Interfaces;
using _01.Scripts.Outgame.Upgrade;
using UnityEngine;

namespace _01.Scripts.Ingame.Click
{
    public class ClickManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _clickableLayer;
        [SerializeField] private float _baseClickDamage = 1f;

        [Header("Critical")]
        [SerializeField] private float _criticalChance = 0.5f;
        [SerializeField] private float _criticalMultiplier = 2f;

        [Header("Dependencies")]
        [SerializeField] private UpgradeManager _upgradeManager;
        [SerializeField] private HeroAnimator _heroAnimator;

        private float ClickDamage => _baseClickDamage + (_upgradeManager != null ? _upgradeManager.TotalClickDamage : 0f);

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryClick();
            }
        }

        private void TryClick()
        {
            Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, _clickableLayer);

            if (hit.collider == null)
            {
                return;
            }

            var clickable = hit.collider.GetComponent<IClickable>();
            if (clickable == null || !clickable.IsClickable)
            {
                return;
            }

            clickable.OnClick();
            DealClickDamage(hit.collider, hit.point);

            // 플레이어 공격 애니메이션.
            if (_heroAnimator != null)
            {
                _heroAnimator.PlayAttack();
            }
        }

        private void DealClickDamage(Collider2D target, Vector2 hitPoint)
        {
            var damageable = target.GetComponent<IDamageable>();
            if (damageable == null)
            {
                return;
            }

            bool isCritical = Random.value < _criticalChance;
            float damage = isCritical ? ClickDamage * _criticalMultiplier : ClickDamage;

            var clickInfo = new ClickInfo(
                damage: damage,
                clickType: EClickType.Manual,
                position: hitPoint,
                isCritical: isCritical
            );

            damageable.TakeDamage(clickInfo);
        }
    }
}