using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace TimeGallery.Models
{
    public class SessionModel
    {
        private Timer _expiresTimer;
        private DateTime _lastRefreshTime;

        public Guid Id { get; private set; }

        public UserModel UserModel { get; private set; }

        /// <summary>
        /// 上次刷新时间
        /// </summary>
        public DateTime LastRefreshTime
        {
            get { return _lastRefreshTime; }
            set
            {
                
            }
        }

        /// <summary>
        /// 到期事件，不需要事件参数
        /// </summary>
        public event EventHandler ExpiresEvent;

        public SessionModel(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            Id = Guid.NewGuid();
            UserModel = userModel;

            _expiresTimer = new Timer(ExpiresTimerHandler, DateTime.Now, );

            LastRefreshTime = DateTime.Now;
        }

        private void OnExpiresEvent()
        {
            ExpiresEvent?.Invoke(this, null);
        }

        private void ExpiresTimerHandler(object state)
        {
            
        }
    }
}