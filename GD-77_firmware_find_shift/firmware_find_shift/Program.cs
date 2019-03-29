/*
 * GD-77 firmware find shift by DG4KLU.
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

namespace find_shift
{
    class Program
    {
        static void find_shift(string[] args)
        {
            FileStream stream_fw_in = new FileStream(args[0] + ".raw", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream stream_data = new FileStream(args[1] + ".dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            byte[] search_bytes = null;
            int start_pos = 0;

            if (args[2] == "search_startpattern")
            {
                start_pos = 0;
                search_bytes = new byte[] { 0xF0, 0xFE, 0x00, 0x20, 0x21, 0x51, 0x00, 0x00 };
            }
            else if (args[2] == "search_endpattern")
            {
                start_pos = (int)stream_fw_in.Length - 16;
                search_bytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }

            if (search_bytes!=null)
            {
                Console.Write(args[0]);

                for (long shift = 0x0000; shift < 0x07fff; shift++)
                {
                    stream_fw_in.Seek(start_pos, SeekOrigin.Begin);
                    int search_idx = 0;

                    int xor_idx = (int)shift;
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

                        if (data == search_bytes[search_idx])
                        {
                            if (search_idx == search_bytes.Length - 1)
                            {
                                break;
                            }
                            search_idx++;
                        }
                        else
                        {
                            break;
                        }

                        xor_idx++;
                        if (xor_idx >= 0x7fff)
                        {
                            xor_idx = 0;
                        }
                    }

                    if (search_idx == search_bytes.Length - 1)
                    {
                        shift = shift - (start_pos % 0x7fff);
                        if (shift<0)
                        {
                            shift = shift + 0x7fff;
                        }
                        Console.WriteLine(String.Format(" {0:X4}", shift));
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Wrong mode.");
            }

            stream_fw_in.Close();
            stream_data.Close();
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Parameters: <Firmwarefile> <Datafilename> <Mode(search_startpattern/search_endpattern)>");
            }
            else
            {
                find_shift(args);
            }
        }
    }
}
