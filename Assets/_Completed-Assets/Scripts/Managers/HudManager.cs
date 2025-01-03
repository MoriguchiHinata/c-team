﻿using UnityEngine;
using Complete;  // Complete名前空間を使用

namespace Complete  // HudManagerも同じ名前空間に入れる
{
    public class HudManager : MonoBehaviour
    {
        // SerializeFieldでInspectorから設定可能に
        [SerializeField] private GameManager m_GameManager;  // GameManagerへの参照
        
        [Header("Stock Areas")]
        [SerializeField] private PlayerStockArea m_Player1StockArea;  // プレイヤー1のストックエリア
        //[SerializeField] private PlayerStockArea m_Player2StockArea;  // プレイヤー2のストックエリア

        [Header("Hp Areas")]
        [SerializeField] private PlayerHpArea m_Player1HpArea;  // プレイヤー1のHPエリア
        [SerializeField] private PlayerHpArea m_Player2HpArea;  // プレイヤー2のHPエリア

        [Header("Win Areas")]
        [SerializeField] private PlayerWinArea m_Player1WinArea;  // プレイヤー1の勝利数エリア
        [SerializeField] private PlayerWinArea m_Player2WinArea;  // プレイヤー2の勝利数エリア

        private void Start()
        {
            if (m_GameManager != null)
            {
                // GameManagerのイベント購読
                m_GameManager.OnGameStateChanged += HandleGameStateChanged;
                m_GameManager.OnWinCountChanged += HandleWinCountChanged;

                // 各タンクのイベント購読
                foreach (var tank in m_GameManager.m_Tanks)
                {
                    tank.OnWeaponStockChanged += HandleWeaponStockChanged;
                    tank.OnHealthChanged += HandleHealthChanged;
                }
            }
            SetHudVisibility(false);
        }

        private void OnDestroy()
        {
            if (m_GameManager != null)
            {
                m_GameManager.OnGameStateChanged -= HandleGameStateChanged;
                m_GameManager.OnWinCountChanged -= HandleWinCountChanged;

                // イベント購読の解除
                foreach (var tank in m_GameManager.m_Tanks)
                {
                    tank.OnWeaponStockChanged -= HandleWeaponStockChanged;
                    tank.OnHealthChanged -= HandleHealthChanged;
                }
            }
        }

        private void HandleWeaponStockChanged(int playerNumber, int currentStock, string weaponName)
        {
            // プレイヤー番号に応じて対応するUIを更新
            // 砲弾のUIを更新
            if (playerNumber == 1 && m_Player1StockArea != null)
            {
                m_Player1StockArea.UpdatePlayerStockArea(currentStock, weaponName);
            }
            //else if (playerNumber == 2 && m_Player2StockArea != null)
            //{
            //    m_Player2StockArea.UpdatePlayerStockArea(currentStock, weaponName);
            //}
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            // プレイ中のみHUDを表示
            bool shouldShowHUD = (newState == GameManager.GameState.RoundPlaying);
            SetHudVisibility(shouldShowHUD);

            // ミニマップの表示/非表示を設定
            bool shouldShowMinimap = (newState == GameManager.GameState.RoundPlaying);
            SetMinimapVisibility(shouldShowMinimap);
        }

        private void SetHudVisibility(bool visible)
        {
            // 両プレイヤーのストックエリアの表示/非表示を設定
            if (m_Player1StockArea != null)
                m_Player1StockArea.gameObject.SetActive(visible);

            //if (m_Player2StockArea != null)
            //    m_Player2StockArea.gameObject.SetActive(visible);

            // 両プレイヤーのHPエリアの表示/非表示を設定
            if (m_Player1HpArea != null)
                m_Player1HpArea.gameObject.SetActive(visible);

            if (m_Player2HpArea != null)
                m_Player2HpArea.gameObject.SetActive(visible);

            // 両プレイヤーの勝利数エリアの表示/非表示を設定
            if (m_Player1WinArea != null)
                m_Player1WinArea.gameObject.SetActive(visible);

            if (m_Player2WinArea != null)
                m_Player2WinArea.gameObject.SetActive(visible);
        }

        private void SetMinimapVisibility(bool isVisible)
        {
            GameObject tank1 = m_GameManager.m_Tanks[0].m_Instance;
            Transform minimapCameraTransform = tank1.transform.Find("TankRenderers/TankTurret/MinimapCamera");
            GameObject minimapCamera = minimapCameraTransform.gameObject;
            minimapCamera.SetActive(isVisible);

            Camera cameraComponent = minimapCamera.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                cameraComponent.enabled = isVisible;
            }
            else
            {
                Debug.LogError("Camera component not found on MinimapCamera");
            }
        }

        private void HandleHealthChanged(int playerNumber, float currentHealth)
        {
            Debug.Log("HandleHealthChanged: " + playerNumber + ", " + currentHealth);
            // プレイヤー番号に応じて対応するUIを更新
            if (playerNumber == 1 && m_Player1StockArea != null)
            {
                m_Player1HpArea.UpdateHpArea(currentHealth);
            }
            else if (playerNumber == 2 && m_Player2HpArea != null)
            {
                m_Player2HpArea.UpdateHpArea(currentHealth);
            }
        }

        private void HandleWinCountChanged(int playerNumber, int winCount)
        {
            Debug.Log("HandleWinCountChanged: " + playerNumber + ", " + winCount);
            if (playerNumber == 1 && m_Player1WinArea != null)
            {
                m_Player1WinArea.UpdateWinCount(winCount);
            }
            else if (playerNumber == 2 && m_Player2WinArea != null)
            {
                m_Player2WinArea.UpdateWinCount(winCount);
            }
        }
    }
}