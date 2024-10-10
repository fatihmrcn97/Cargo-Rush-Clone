using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : IMover
{
    CharacterController _characterController;
    PlayerSO _playerSO;
    float upgrade_speed = 0;
    public PlayerMoveController(CharacterController characterController,PlayerSO playerSo)
    {
        _characterController = characterController;
        _playerSO = playerSo;
        if (!PlayerPrefs.HasKey("speedUpgrade")) PlayerPrefs.SetFloat("speedUpgrade", 0);
        else upgrade_speed = PlayerPrefs.GetFloat("speedUpgrade");
    }

    public void FixedTick(Vector3 moveDirection)
    {
        moveDirection.y = 0; 
        _characterController.Move((_playerSO.speed + upgrade_speed) * Time.deltaTime * moveDirection);
    }

    public void SpeedUpgraded()
    {
        upgrade_speed = PlayerPrefs.GetFloat("speedUpgrade");
    }
}

public interface IMover
{
    void FixedTick(Vector3 moveDirection);
    void SpeedUpgraded();
}
