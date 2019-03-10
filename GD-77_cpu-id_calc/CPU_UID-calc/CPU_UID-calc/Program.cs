/*
 * GD-77 CPU-ID calculator by DG4KLU.
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

namespace CPU_UID_calc
{
    class Program
    {
        static void Main(string[] args)
        {
            // check file exist
            if (File.Exists("cpu-uid.dat"))
            {
                byte[] buf_in1_16 = new byte[16];
                byte[] buf_in2_16 = new byte[16];

                // read input data
                FileStream stream_in = new FileStream("cpu-uid.dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                stream_in.Read(buf_in1_16, 0, buf_in1_16.Length);
                stream_in.Read(buf_in2_16, 0, buf_in2_16.Length);
                stream_in.Close();

                // convert CPU-UID
                for (int i=0; i<4; i++)
                {
                    // calculate data required by the bootloader (needs to get placed at 0x0007f800)
                    uint v1 = (uint)((buf_in1_16[i * 4] << 0) + (buf_in1_16[i * 4 + 1] << 8) + (buf_in1_16[i * 4 + 2] << 16) + (buf_in1_16[i * 4 + 3] << 24));

                    uint vtmp = v1 + 0xAAAA * (v1 + 0x5555);                                // ... the magic formula ...
                    v1 = vtmp + (vtmp << 27) + (vtmp << 13) + (vtmp >> 17) + (vtmp >> 23);  // ...
                    if (v1 == 0)                                                            // ...
                    {                                                                       // ...
                        v1 = 0x12345678;                                                    // ...
                    }                                                                       // ...

                    buf_in1_16[i * 4] = (byte)((v1 & 0x000000FF) >> 0);
                    buf_in1_16[i * 4 + 1] = (byte)((v1 & 0x0000FF00) >> 8);
                    buf_in1_16[i * 4 + 2] = (byte)((v1 & 0x00FF0000) >> 16);
                    buf_in1_16[i * 4 + 3] = (byte)((v1 & 0xFF000000) >> 24);

                    // calculate data required by the bootloader (needs to get placed at 0x0007f810)
                    int v2 = (buf_in2_16[i * 4] << 0) + (buf_in2_16[i * 4 + 1] << 8) + (buf_in2_16[i * 4 + 2] << 16) + (buf_in2_16[i * 4 + 3] << 24);

                    v2 = 0x1d / (v2 + 1) - v2 - 1;                                          // ... the magic formula ...

                    buf_in2_16[i * 4] = (byte)((v2 & 0x000000FF) >> 0);
                    buf_in2_16[i * 4 + 1] = (byte)((v2 & 0x0000FF00) >> 8);
                    buf_in2_16[i * 4 + 2] = (byte)((v2 & 0x00FF0000) >> 16);
                    buf_in2_16[i * 4 + 3] = (byte)((v2 & 0xFF000000) >> 24);
                }

                // write converted data
                FileStream stream_out = new FileStream("cpu-uid.out", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                stream_out.Write(buf_in1_16, 0, buf_in1_16.Length);
                stream_out.Write(buf_in2_16, 0, buf_in2_16.Length);
                stream_out.Close();
            }
            else
            {
                Console.WriteLine("ERROR: cpu-uid.dat file missing.");
            }
        }
    }
}
