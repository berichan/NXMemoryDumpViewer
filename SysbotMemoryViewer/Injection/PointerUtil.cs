﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NHSE.Injection
{
    public static class PointerUtil
    {
        public static ulong GetPointerAddressFromExpression(IRAMReadWriter rw, string pointerExpression)
        {
            // Regex pattern to get operators and offsets from pointer expression.
            const string pattern = @"(\+|\-)([A-Fa-f0-9]+)";
            Regex regex = new(pattern);
            Match match = regex.Match(pointerExpression);

            // Get first offset from pointer expression and read address at that offset from main start.
            var ofs = Convert.ToUInt64(match.Groups[2].Value, 16);
            var address = BitConverter.ToUInt64(rw.ReadBytes(ofs, 0x8, RWMethod.Main), 0);
            match = match.NextMatch();

            // Matches the rest of the operators and offsets in the pointer expression.
            while (match.Success)
            {
                // Get operator and offset from match.
                string opp = match.Groups[1].Value;
                ofs = Convert.ToUInt64(match.Groups[2].Value, 16);

                // Add or subtract the offset from the current stored address based on operator in front of offset.
                switch (opp)
                {
                    case "+":
                        address += ofs;
                        break;
                    case "-":
                        address -= ofs;
                        break;
                }

                // Attempt another match and if successful read bytes at address and store the new address.
                match = match.NextMatch();
                if (!match.Success)
                    continue;

                byte[] bytes = rw.ReadBytes(address, 0x8, RWMethod.Absolute);
                address = BitConverter.ToUInt64(bytes, 0);
            }

            return address;
        }
    }
}
