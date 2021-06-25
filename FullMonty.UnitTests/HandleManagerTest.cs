using System;
using FullMonty.AddIn;
using NUnit.Framework;

namespace FullMonty.UnitTests
{
    public class HandleManagerTest
    {
        [Test]
        public void CanRegisterAndRetrieveHandle()
        {
            var manager = new HandleManager();

            var payload = new object();
            var createdHandle = manager.Register(payload);

            Assert.IsNotNull(createdHandle.Name);
            Assert.AreSame(payload, createdHandle.Payload);


            var retrievedHandle = manager[createdHandle.Name];
            Assert.AreSame(createdHandle, retrievedHandle);
        }

        [Test]
        public void ShouldThrowIfRetrievingUnregisteredHandle()
        {
            Assert.Throws<HandleManager.UnregisteredHandleException>(() =>
            {
                var x = new HandleManager()["myHandle"];
            });
        }

        [Test]
        public void CanRegisterWithSpecificName()
        {
            var manager = new HandleManager();
            var payload = new object();
            const string name = "MyHandleName";

            var createdHandle = manager.Register(payload, name);

            Assert.AreEqual(name, createdHandle.Name);
            Assert.AreSame(payload, createdHandle.Payload);


            var retrievedHandle = manager[name];
            Assert.AreSame(createdHandle, retrievedHandle);
        }

        [Test]
        public void RegisterShouldThrowIfNameIsNull()
        {
            var manager = new HandleManager();
            Assert.Throws<ArgumentNullException>(() => manager.Register(new object(), null));
        }
    }
}