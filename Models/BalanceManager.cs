using System;

namespace project.Models
{
    public static class BalanceManager
    {
        private static decimal _balance = 300;
        private static DateTime _lastIncrement = DateTime.UtcNow;
        private static readonly object _lock = new object();

        public static decimal GetBalance()
        {
            lock (_lock)
            {
                UpdateBalance();
                return _balance;
            }
        }

        public static bool TryDeduct(decimal amount)
        {
            lock (_lock)
            {
                UpdateBalance();
                if (_balance >= amount)
                {
                    _balance -= amount;
                    return true;
                }
                return false;
            }
        }

        private static void UpdateBalance()
        {
            var now = DateTime.UtcNow;
            var minutesPassed = (now - _lastIncrement).TotalMinutes;
            if (minutesPassed >= 1)
            {
                int increments = (int)(minutesPassed / 1);
                _balance += increments * 200;
                _lastIncrement = _lastIncrement.AddMinutes(increments * 1);
            }
        }
    }
} 