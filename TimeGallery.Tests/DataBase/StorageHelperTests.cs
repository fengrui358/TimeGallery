using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using TimeGallery.DataBase;
using TimeGallery.Helper;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using StorageHelper = TimeGallery.DataBase.StorageHelper;

namespace TimeGallery.Tests.DataBase
{
    [TestFixture]
    public class StorageHelperTests
    {
        [TestCase()]
        public void GetConnectionTest()
        {
            var x = StorageHelper.GetConnection();
            Assert.Fail();
        }
    }
}