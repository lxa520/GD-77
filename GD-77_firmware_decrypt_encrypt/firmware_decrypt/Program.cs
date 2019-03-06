/*
 * GD-77 firmware decrypter/encrypter by DG4KLU.
 *
 * Copyright (C)2019 Kai Ludwig, DG4KLU
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *   1. Redistributions of source code must retain the above copyright notice,
 *      this list of conditions and the following disclaimer.
 *   2. Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 *   3. The name of the author may not be used to endorse or promote products
 *      derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 * EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
 * OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.IO;

namespace firmware_decrypt
{
    class Program
    {
        static void decrypt(string[] args)
        {
            bool pause = true;

            int shift = Convert.ToInt32(args[2], 16);

            Console.WriteLine(args[0] + String.Format(" {0:X4}", shift));

            FileStream stream_fw_in = new FileStream(args[0] + ".raw", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream stream_fw_out = new FileStream(args[0] + ".dec", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            FileStream stream_data = new FileStream(args[1] + ".dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            int xor_idx = shift;
            while (true)
            {
                int data = stream_fw_in.ReadByte();

                if (data < 0)
                {
                    break;
                }

                stream_data.Seek(xor_idx, SeekOrigin.Begin);
                byte xor_value = (byte)stream_data.ReadByte();

                data = ~(((data << 3) & 0b11111000) | ((data >> 5) & 0b00000111));
                data = (byte)data ^ xor_value;

                stream_fw_out.WriteByte((byte)data);

                xor_idx++;
                if (xor_idx >= 0x7fff)
                {
                    xor_idx = 0;
                }
            }

            stream_fw_in.Close();
            stream_fw_out.Close();
            stream_data.Close();

            pause = false;

            if (pause)
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }

        static void encrypt(string[] args)
        {
            bool pause = true;

            int shift = Convert.ToInt32(args[2], 16);

            Console.WriteLine(args[0] + String.Format(" {0:X4}", shift));

            FileStream stream_fw_in = new FileStream(args[0] + ".dec", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream stream_fw_out = new FileStream(args[0] + ".enc", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            FileStream stream_data = new FileStream(args[1] + ".dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            int xor_idx = shift;
            while (true)
            {
                int data = stream_fw_in.ReadByte();

                if (data < 0)
                {
                    break;
                }

                stream_data.Seek(xor_idx, SeekOrigin.Begin);
                byte xor_value = (byte)stream_data.ReadByte();

                data = (byte)data ^ xor_value;
                data = ~(((data >> 3) & 0b00011111) | ((data << 5) & 0b11100000));

                stream_fw_out.WriteByte((byte)data);

                xor_idx++;
                if (xor_idx >= 0x7fff)
                {
                    xor_idx = 0;
                }
            }

            stream_fw_in.Close();
            stream_fw_out.Close();
            stream_data.Close();

            pause = false;

            if (pause)
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Parameters: <Firmwarefile> <Datafilename> <Shift> <Mode(decrypt/encrypt)>");
            }
            else
            {
                if (args[3] == "decrypt")
                {
                    decrypt(args);
                }
                else if (args[3] == "encrypt")
                {
                    encrypt(args);
                }
            }
        }
    }
}
