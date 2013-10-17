using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCFUdpCommandSender;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConvertToString_Empty()
        {
            byte[] bytearray = {};
            string convertedString = Command.ConvertToString(bytearray);
            Assert.AreEqual("", convertedString);
        }

        [TestMethod]
        public void ConvertToString_1Byte()
        {
            byte[] bytearray = { 0 };
            string convertedString = Command.ConvertToString(bytearray); 
            Assert.AreEqual("00", convertedString);
        }

        [TestMethod]
        public void ConvertToString_1ByteLarger10()
        {
            byte[] bytearray = { 12 };
            string convertedString = Command.ConvertToString(bytearray);
            Assert.AreEqual("12", convertedString);
        }

        [TestMethod]
        public void ConvertToString_2Bytes()
        {
            byte[] bytearray = { 00, 00 };
            string convertedString = Command.ConvertToString(bytearray);
            Assert.AreEqual("0000", convertedString);
        }

        [TestMethod]
        public void ConvertToString_2BytesLarger10()
        {
            byte[] bytearray = { 12, 05 };
            string convertedString = Command.ConvertToString(bytearray);
            Assert.AreEqual("1205", convertedString);
        }

        [TestMethod]
        public void ConvertFromString_0Bytes()
        {
            byte[] expectedBytearray = {};
            string stringToConvert = "";
            byte[] convertedBytearray = Command.ConvertFromString(stringToConvert);
            Assert.IsTrue(AreByteArraysEqual(expectedBytearray, convertedBytearray));
        }

        [TestMethod]
        public void ConvertFromString_1Byte()
        {
            byte[] expectedBytearray = { 0 };
            string stringToConvert = "00";
            byte[] convertedBytearray = Command.ConvertFromString(stringToConvert);
            Assert.IsTrue(AreByteArraysEqual(expectedBytearray, convertedBytearray));
        }

        [TestMethod]
        public void ConvertFromString_1ByteLarger10()
        {
            byte[] expectedBytearray = { 15 };
            string stringToConvert = "15";
            byte[] convertedBytearray = Command.ConvertFromString(stringToConvert);
            Assert.IsTrue(AreByteArraysEqual(expectedBytearray, convertedBytearray));
        }

        [TestMethod]
        public void ConvertFromString_2Bytes()
        {
            byte[] expectedBytearray = { 0, 0 };
            string stringToConvert = "0000";
            byte[] convertedBytearray = Command.ConvertFromString(stringToConvert);
            Assert.IsTrue(AreByteArraysEqual(expectedBytearray, convertedBytearray));
        }

        [TestMethod]
        public void ConvertFromString_2BytesLarger10()
        {
            byte[] expectedBytearray = { 12, 34 };
            string stringToConvert = "1234";
            byte[] convertedBytearray = Command.ConvertFromString(stringToConvert);
            Assert.IsTrue(AreByteArraysEqual(expectedBytearray, convertedBytearray));
        }

        private bool AreByteArraysEqual(byte[] expectedBytearray, byte[] convertedBytearray)
        {
            if (expectedBytearray.Length != convertedBytearray.Length)
                return false;

            for (int i = 0; i < expectedBytearray.Length; i++)
            {
                if(expectedBytearray[i] != convertedBytearray[i])
                    return false;
            }
            return true;
        }
    }
}
