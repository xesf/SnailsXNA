using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Input
{
    public class ControllerRumble
    {
        private int _rumbleTimer;
        private bool _enableRumble;
        private float _leftMotor;
        private float _rightMotor;
        private GamePadInput _gamepad;

        public ControllerRumble(GamePadInput gamePad)
        {
            _enableRumble = false;
            _gamepad = gamePad;
        }

        public void Update(BrainGameTime gameTime)
        {
            if (_enableRumble)
            {
                if (_rumbleTimer > 0)
                {
                    _rumbleTimer -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    _rumbleTimer = 5;
                    _leftMotor -= 0.1f;
                    _rightMotor -= 0.1f;

                    if (_leftMotor < 0)
                        _leftMotor = 0;
                    if (_rightMotor < 0)
                        _rightMotor = 0;
                    if (_leftMotor == 0 && _rightMotor == 0)
                    {
                        _enableRumble = false;
                        _rumbleTimer = 0;
                    }
                    _gamepad.SetVibration(_leftMotor, _rightMotor);
                }
            }
        }

        // Add Rumble effect
        public void AddEffect(int duration, float leftMotor, float rightMotor)
        {
            _enableRumble = true;
            _leftMotor = leftMotor;
            _rightMotor = rightMotor;
            _rumbleTimer = duration;
            _gamepad.SetVibration(_leftMotor, _rightMotor);
        }
    }
}
