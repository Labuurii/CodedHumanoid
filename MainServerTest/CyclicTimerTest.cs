using System;
using System.Threading;
using MainServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainServerTest
{
    [TestClass]
    public class CyclicTimerTest
    {
        [TestMethod]
        public void Test1()
        {
            var timer = new CyclicTimer(1);
            timer.WaitUntilNextFrame();
            Assert.IsTrue(!timer.HasPassedLastFrame());
            Assert.IsFalse(timer.IsBehind());
            Thread.Sleep(1000);
            Assert.IsTrue(timer.HasPassedLastFrame());
            Assert.IsFalse(timer.IsBehind());

            var now = DateTime.Now;
            timer.WaitUntilNextFrame();
            var delta = DateTime.Now - now;
            Assert.IsTrue(delta.TotalSeconds > 1, delta.TotalSeconds.ToString());
            Assert.IsFalse(timer.IsBehind());
        }

        [TestMethod]
        public void TestBehindTrue()
        {
            var timer = new CyclicTimer(1);
            Thread.Sleep(2000);
            Assert.IsTrue(timer.IsBehind());
        }

        [TestMethod]
        public void TestBehindFalse()
        {
            var timer = new CyclicTimer(1);
            Thread.Sleep(1500);
            Assert.IsFalse(timer.IsBehind());
        }
    }
}
