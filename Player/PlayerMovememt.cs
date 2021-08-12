using System;
using _Scripts.Collectables;
using UnityEngine;

namespace _Scripts.Player
{
    /**
     * Class should be assigned to Player 'SucerSum' as component responsible for moving player
     * this methods are called directly from the controller (Keyboard/Mouse/Touch)
     */
    public class PlayerMovememt : MonoBehaviour, IPlayerMovement, IPlayerMovementParameters
    {
        #region enums

        public enum PlayerDirectionEnum { Hold, Up, Down, Left, Right }

        #endregion

        #region unity fields

        [SerializeField] private Transform RespawnPosition;
        [SerializeField]
        private float _forwardSpeed = 1f;
        [SerializeField]
        private float _rotationSpeed = 1f;

        private bool NeedsTurn = false;
        private bool IsTurning = false;

        public float forwardSpeed => _forwardSpeed;
        public float rotationSpeed => _rotationSpeed;
        
        public bool IsPoweredUp => modifier!=null && modifiedParams.IsPoweredUp;

        private bool _hasDied = false;
        public bool HasDied { get; }
        public float TimeToDismissPowerUp => modifier?.TimeToExpire ?? 0;
        public int TimeToDismissPowerUpInSec => (int) Math.Round(TimeToDismissPowerUp, 0);
        public int LTimeToDismissPowerUpInSec = 0;

        public event Action<bool, float> OnIsPoweredUpStarted;
        public event Action<bool, float> OnIsPoweredUpTimeout;
        public event Action<bool, float> OnIsPoweredUpChanged;
        public event Action<bool, int> OnIsPoweredUpSecondsChanged;
        public event Action<Enemy.Enemy> OnDiedBy;
        public event Action<Enemy.Enemy> OnConsumedEnemy;
        public event Action<ICollectable> OnConsumedCollectable;
        public event Action OnChangedDirection;

        public IPlayerMovementParameters modifiedParams => modifier?.Apply(this) ?? this;
        

        #endregion

        #region private fields

        

        #endregion

        private PlayerDirectionEnum currentDirection = PlayerDirectionEnum.Hold;
        [SerializeField] PlayerDirectionEnum wantedDirection = PlayerDirectionEnum.Hold;
        private PlayerDirectionEnum lastWantedDirection = PlayerDirectionEnum.Hold;
        private IPropertyModifier<IPlayerMovementParameters> modifier;


        public PlayerDirectionEnum GetCurrentDirection()
        {
            return wantedDirection;
        }

        private void Start()
        {
            Respawn();
        }

        public void Respawn()
        {
            if (RespawnPosition)
                transform.position = RespawnPosition.position;
            
            _hasDied = false;
            IsTurning = false;
            currentDirection = PlayerDirectionEnum.Hold;
            wantedDirection = PlayerDirectionEnum.Hold;
        }

        public void MoveUp()
        {
            wantedDirection = PlayerDirectionEnum.Up;
        }

        public void MoveDown()
        {
            wantedDirection = PlayerDirectionEnum.Down;
        }

        public void MoveLeft()
        {
            wantedDirection = PlayerDirectionEnum.Left;
        }

        public void MoveRight()
        {
            wantedDirection = PlayerDirectionEnum.Right;
        }

        public Vector3 Position => gameObject.transform.position;
        public bool IsAtRespawnPosition => transform.position == RespawnPosition.position;

        private void FixedUpdate()
        {
            if (_hasDied) return;
            
            ExpireModifierIfNeeded();
            if (wantedDirection == PlayerDirectionEnum.Hold) return;
            if (wantedDirection == currentDirection) return;
            RaiseEventIfWantedDirectionHasChanged();
            RaiseEventIfPowerUpTimeChanged();
            CalcIfNeedsTurn();
            RotateToTargetDirection();
            MovePlayer();
        }

        private void RaiseEventIfPowerUpTimeChanged()
        {
            if (LTimeToDismissPowerUpInSec != TimeToDismissPowerUpInSec)
                OnIsPoweredUpSecondsChanged?.Invoke(IsPoweredUp, TimeToDismissPowerUpInSec);

            LTimeToDismissPowerUpInSec = TimeToDismissPowerUpInSec;
        }

        private void RaiseEventIfWantedDirectionHasChanged()
        {
            if (lastWantedDirection == wantedDirection) return;
            OnChangedDirection?.Invoke();
            lastWantedDirection = wantedDirection;
        }

        private void ExpireModifierIfNeeded()
        {
            if (modifier == null) return;
            if (modifier.IsExpired)
            {
                modifier = null;
                OnIsPoweredUpChanged?.Invoke(IsPoweredUp, TimeToDismissPowerUp);
                OnIsPoweredUpTimeout?.Invoke(IsPoweredUp, TimeToDismissPowerUp);
                return;
            }
            
            OnIsPoweredUpChanged?.Invoke(IsPoweredUp, TimeToDismissPowerUp);
        }


        private void CalcIfNeedsTurn()
        {
            if (IsTurning) 
                NeedsTurn = false;
            
            if (currentDirection == wantedDirection) 
                NeedsTurn = false;

            NeedsTurn = true;
        }

        private void RotateToTargetDirection()
        {
            if (!NeedsTurn &&  !IsTurning) return;

            var currentAngle = transform.eulerAngles;
            var targetAngle = 0;

            switch (wantedDirection)
            {
                case PlayerDirectionEnum.Up:
                    targetAngle = 180+90;
                    break;
                case PlayerDirectionEnum.Down:
                    targetAngle = 90; 
                    break;
                case PlayerDirectionEnum.Left:
                    targetAngle = 0;
                    break;
                case PlayerDirectionEnum.Right:
                    targetAngle = 180;
                    break;
            }

            var angleDistance = Mathf.LerpAngle(currentAngle.z, targetAngle, Time.deltaTime * modifiedParams.rotationSpeed);
            if (Math.Abs(angleDistance - targetAngle) < Mathf.Epsilon )
            {
                IsTurning = false;
                return;
            }
            IsTurning = true;

            currentAngle.z = angleDistance;
            transform.eulerAngles = currentAngle;
            //transform.RotateAround(transform.position, Vector3.forward, Math.Min(angleDistance, angleDistance));



        }

        /// <summary>
        /// Calculate player new position based on wanted Direction and current heading
        /// </summary>
        private void MovePlayer()
        {
            //if (IsTurning) return;
            
            var currentPos = transform.position;
            var positionIncrement = Vector3.zero;
            
            switch (wantedDirection)
            {
                case PlayerDirectionEnum.Up:
                    positionIncrement.y = Time.deltaTime * modifiedParams.forwardSpeed;
                    break;
                case PlayerDirectionEnum.Down:
                    positionIncrement.y = -Time.deltaTime * modifiedParams.forwardSpeed;
                    break;
                case PlayerDirectionEnum.Left:
                    positionIncrement.x = -Time.deltaTime * modifiedParams.forwardSpeed;
                    break;
                case PlayerDirectionEnum.Right:
                    positionIncrement.x = Time.deltaTime * modifiedParams.forwardSpeed;
                    break;
            }

            transform.position = currentPos + positionIncrement;
        }

        public void ApplyModifier(IPropertyModifier<IPlayerMovementParameters> modifier)
        {
            this.modifier = modifier;
            this.modifier.Initialize(this);
            OnIsPoweredUpStarted?.Invoke(IsPoweredUp, TimeToDismissPowerUp);
            OnIsPoweredUpChanged?.Invoke(IsPoweredUp, TimeToDismissPowerUp);
        }

        public void KilledBy(Enemy.Enemy enemy)
        {
            _hasDied = true;
            OnDiedBy?.Invoke(enemy);
        }

        public void ConsumedEnemy(Enemy.Enemy enemy)
        {
            OnConsumedEnemy?.Invoke(enemy);
        }

        public void RaiseConsumedCollectable(ICollectable collectableItem)
        {
            OnConsumedCollectable?.Invoke(collectableItem);
        }
        
    }
}