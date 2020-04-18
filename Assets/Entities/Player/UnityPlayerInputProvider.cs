﻿using UnityEngine;

namespace Entities.Player
{
    public class UnityPlayerInputProvider : IPlayerInputProvider
    {
        public PlayerInput GetInput()
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");
            bool jumpInput = Input.GetButton("Jump");

            return new PlayerInput
            {
                XInput = xInput,
                YInput = yInput,
                JumpInput = jumpInput
            };
        }
    }
}