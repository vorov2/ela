using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{

    [TestClass]
    public class TestMyMath
    {
        [TestMethod]
        public void TestSum()
        {
            const int should = 5;
            int result = MyMath.Sum(2, 2);
            Assert.AreEqual(should, result);//, 
                //String.Format("Полученное значение '{0}' не соответствует ожидаемому '{1}'.", result, should));
        }

        [TestMethod]
        public void TestSubtract()
        {
            const int should = 3;
            int result = MyMath.Subtract(8, 5);
            Assert.AreEqual(should, result);
        }
    }



    static class MyMath
    {
        public static int Sum(int x, int y)
        {
            return x + y;
        }

        public static int Subtract(int x, int y)
        {
            return x - y;
        }
    }
}
