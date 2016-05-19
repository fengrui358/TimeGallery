using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using TimeGallery.DataBase;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Managers;
using TimeGallery.Tests.Fakes;
using TimeGallery.Tests.Helper;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using StorageHelper = TimeGallery.DataBase.StorageHelper;

namespace TimeGallery.Tests.DataBase
{
    [TestFixture]
    public class StorageHelperTests
    {
        [SetUp]
        public void SetUp()
        {
            var stubBuilder = new ContainerBuilder();
            stubBuilder.RegisterType<StubWebConfigConfigurationManager>().As<IConfigurationManager>();

            IocHelper.Container = stubBuilder.Build();

            //创建数据库
            DbTestHelper.CreateFakeDataBase();
        }

        [TearDown]
        public void TearDown()
        {
            DbTestHelper.DropFakeDataBase();
        }

        [Test]
        public void GetConnectionTest()
        {
            //var x = StorageHelper.GetConnection();
            Assert.Fail();
        }
    }
}