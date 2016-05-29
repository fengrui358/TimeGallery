using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeGallery.Interfaces
{
    public interface IManagerDependency : IDependency
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
    }
}
