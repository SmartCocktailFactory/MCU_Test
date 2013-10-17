using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCFUdpCommandSender
{
    public class Command
    {
        public Command(int commandNumber, string name, int payloadLength)
        {
            Name = name;
            PayloadLength = payloadLength;
            CommandNumber = new byte[4];
            CommandNumber[0] = (byte)(commandNumber / 1000);
            CommandNumber[1] = (byte)(commandNumber / 100);
            CommandNumber[2] = (byte)(commandNumber / 10);
            CommandNumber[3] = (byte)commandNumber;
        }

        public byte[] CommandNumber { get; private set; }
        public string Name { get; private set; }
        public int PayloadLength { get; private set; }
        public bool HasPayload
        {
            get { return PayloadLength > 0; }
        }

        public override string ToString()
        {
            return Name + " = 0x" + ConvertToString(CommandNumber);
        }

        public static string ConvertToString(byte[] bytearray)
        {
            String result = String.Empty;
            for (int i = 0; i < bytearray.Length; i++)
            {
                result += String.Format("{0, 2}", bytearray[i]);
                result = result.Replace(' ', '0');
            }
            return result;
        }

        public static byte[] ConvertFromString(String text)
        {
            if (text.Length == 0)
                return new byte[] { };

            int number = 0;
            if (!Int32.TryParse(text, out number))
            {
                throw new InvalidOperationException("passed text is not a number");
            }

            byte[] bytearray = new byte[text.Length/2];
            for (int i = 0; i < text.Length; i+=2)
            {
                bytearray[i/2] = (byte)Int32.Parse(text.Substring(i, 2));
            }
            return bytearray;
        }
    }
}
