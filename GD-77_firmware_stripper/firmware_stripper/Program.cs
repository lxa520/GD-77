/*
 * GD-77 firmware stripper by DG4KLU.
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
using System.Linq;

namespace firmware_stripper
{
    class Program
    {
        static void Main(string[] args)
        {
            bool pause = true;

            // check parameters
            if (args.Length != 1)
            {
                Console.WriteLine("firmware_stripper: Reads GD-77 sgl firmware file and extracts the raw encrypted firmware data");
                Console.WriteLine("                   and the encoding key converted to hex.");
                Console.WriteLine();
                Console.WriteLine("firmware_stripper <filename without .sgl extension>");
                Console.WriteLine();
                Console.WriteLine("=> .raw file of the same name (with the encoding key converted to hex added to the filename)");
                Console.WriteLine("    containing raw encrypted firmware data");
            }
            else
            {
                // check file exist
                if (File.Exists(args[0] + ".sgl"))
                {
                    FileStream stream_in = new FileStream(args[0] + ".sgl", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    // read header tag
                    byte[] buf_in_4 = new byte[4];
                    stream_in.Read(buf_in_4, 0, buf_in_4.Length);

                    // check header tag
                    byte[] header_tag = new byte[] { (byte)'S', (byte)'G', (byte)'L', (byte)'!' };
                    if (buf_in_4.SequenceEqual(header_tag))
                    {
                        // read and decode offset and xor tag
                        stream_in.Seek(0x000C, SeekOrigin.Begin);
                        stream_in.Read(buf_in_4, 0, buf_in_4.Length);
                        for (int i = 0; i < buf_in_4.Length; i++)
                        {
                            buf_in_4[i] = (byte)(buf_in_4[i] ^ header_tag[i]);
                        }
                        int offset = buf_in_4[0] + 256 * buf_in_4[1];
                        byte[] xor_data = new byte[] { buf_in_4[2] , buf_in_4[3] };

                        // read and decode part of the header
                        byte[] buf_in_512 = new byte[512];
                        stream_in.Seek(offset + 0x0006, SeekOrigin.Begin);
                        stream_in.Read(buf_in_512, 0, buf_in_512.Length);
                        int xor_idx = 0;
                        for (int i = 0; i < buf_in_512.Length; i++)
                        {
                            buf_in_512[i] = (byte)(buf_in_512[i] ^ xor_data[xor_idx]);
                            xor_idx++;
                            if (xor_idx==2)
                            {
                                xor_idx = 0;
                            }
                        }

                        // dump decoded part of the header
                        /*
                        Console.WriteLine(String.Format("Offset  : {0:X4}", offset));
                        Console.WriteLine(String.Format("XOR-Data: {0:X2}{1:X2}", xor_data[0], xor_data[1]));
                        int pos = 0;
                        int idx = 0;
                        string line1 = "";
                        string line2 = "";
                        for (int i = 0; i < buf_in_512.Length; i++)
                        {
                            if (line1 == "")
                            {
                                line1 = String.Format("{0:X6}: ", i);
                            }
                            line1 = line1 + String.Format(" {0:X2}", buf_in_512[idx]);
                            if ((buf_in_512[idx] >= 0x20) && (buf_in_512[idx] < 0x7f))
                            {
                                line2 = line2 + (char)buf_in_512[idx];
                            }
                            else
                            {
                                line2 = line2 + ".";
                            }
                            idx++;
                            pos++;

                            if (pos == 16)
                            {
                                Console.WriteLine(line1 + " " + line2);
                                line1 = "";
                                line2 = "";
                                pos = 0;
                            }
                        }
                        */

                        // extract encoding key
                        byte key1 = (byte)(buf_in_512[0x005D] - 'a');
                        byte key2 = (byte)(buf_in_512[0x005E] - 'a');
                        byte key3 = (byte)(buf_in_512[0x005F] - 'a');
                        byte key4 = (byte)(buf_in_512[0x0060] - 'a');
                        int encoding_key = (key1 << 12) + (key2 << 8) + (key3 << 4) + key4;

                        // extract length
                        byte length1 = (byte)buf_in_512[0x0000];
                        byte length2 = (byte)buf_in_512[0x0001];
                        byte length3 = (byte)buf_in_512[0x0002];
                        byte length4 = (byte)buf_in_512[0x0003];
                        int length = (length4 << 24) + (length3 << 16) + (length2 << 8) + length1;

                        // extract encoded raw firmware
                        FileStream stream_out = new FileStream(args[0] + "_" + String.Format("{0:X4}", encoding_key) + "_" + String.Format("{0:X6}", length) + ".raw", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                        stream_in.Seek(stream_in.Length - length, SeekOrigin.Begin);
                        int c;
                        while ((c = stream_in.ReadByte())>=0)
                        {
                            stream_out.WriteByte((byte)c);
                        }
                        stream_out.Close();

                        // disable pause on exit
                        pause = false;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: SGL! header missing.");
                    }

                    stream_in.Close();
                }
                else
                {
                    Console.WriteLine("ERROR: sgl file missing.");
                }
            }

            if (pause)
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }
    }
}
