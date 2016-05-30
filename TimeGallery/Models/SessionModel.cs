using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Autofac;
using TimeGallery.Helper;
using TimeGallery.Interfaces;

namespace TimeGallery.Models
{
    public class SessionModel
    {
        private Timer _expiresTimer;
        private DateTime _lastRefreshTime;

        /// <summary>
        /// Session的有效保留时间
        /// </summary>
        private int SessionAvailableTime
        {
            get
            {
                var sessionAvailableTime =
                    IocHelper.Container.Resolve<IConfigurationManager>().GetAppSetting("SessionAvailableTime");

                int outSessionAvailableTime;
                if (!string.IsNullOrEmpty(sessionAvailableTime) &&
                    int.TryParse(sessionAvailableTime, out outSessionAvailableTime))
                {
                    return outSessionAvailableTime;
                }
                else
                {
                    return 1800;
                }
            }
        }

        public Guid Id { get; private set; }

        public UserModel UserModel { get; private set; }

        /// <summary>
        /// 上次刷新时间
        /// </summary>
        public DateTime LastRefreshTime
        {
            get { return _lastRefreshTime; }
            private set
            {
                _lastRefreshTime = value;

                //比要求时间延后10秒钟校验
                _expiresTimer.Change(
                    TimeSpan.FromSeconds(SessionAvailableTime).Add(TimeSpan.FromSeconds(10)),
                    Timeout.InfiniteTimeSpan);
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

            _expiresTimer = new Timer(ExpiresTimerHandler);

            LastRefreshTime = DateTime.Now;
        }

        public void Refresh()
        {
            LastRefreshTime = DateTime.Now;
        }

        private void OnExpiresEvent()
        {
            ExpiresEvent?.Invoke(this, null);
        }

        private void ExpiresTimerHandler(object state)
        {
            if (LastRefreshTime.AddSeconds(SessionAvailableTime) < DateTime.Now)
            {
                //到期清理
                OnExpiresEvent();
            }
        }
    }
}