﻿using System.Text;

namespace VCCSharp.Models.Keyboard
{
    public interface IKeyboardScanCodes
    {
        string ConvertScanCodes(string text);
    }

    public class KeyboardScanCodes : IKeyboardScanCodes
    {
        public string ConvertScanCodes(string text)
        {
            var result = new StringBuilder();

            foreach (var letter in text)
            {
                char sc = GetScanCode(letter);
                // ReSharper disable once InconsistentNaming
                bool CSHIFT = GetCSHIFT(letter);
                // ReSharper disable once InconsistentNaming
                bool LCNTRL = GetLCNTRL(letter);

                if (CSHIFT) { result.Append((char)Define.DIK_LSHIFT); }
                if (LCNTRL) { result.Append((char)Define.DIK_LCONTROL); }

                result.Append(sc);
            }

            return result.ToString();
        }

        public char GetScanCode(char letter)
        {
            return letter switch
            {
                '@' => (char)0x03,
                'A' => (char)0x1E,
                'B' => (char)0x30,
                'C' => (char)0x2E,
                'D' => (char)0x20,
                'E' => (char)0x12,
                'F' => (char)0x21,
                'G' => (char)0x22,
                'H' => (char)0x23,
                'I' => (char)0x17,
                'J' => (char)0x24,
                'K' => (char)0x25,
                'L' => (char)0x26,
                'M' => (char)0x32,
                'N' => (char)0x31,
                'O' => (char)0x18,
                'P' => (char)0x19,
                'Q' => (char)0x10,
                'R' => (char)0x13,
                'S' => (char)0x1F,
                'T' => (char)0x14,
                'U' => (char)0x16,
                'V' => (char)0x2F,
                'W' => (char)0x11,
                'X' => (char)0x2D,
                'Y' => (char)0x15,
                'Z' => (char)0x2C,
                ' ' => (char)0x39,
                'a' => (char)0x1E,
                'b' => (char)0x30,
                'c' => (char)0x2E,
                'd' => (char)0x20,
                'e' => (char)0x12,
                'f' => (char)0x21,
                'g' => (char)0x22,
                'h' => (char)0x23,
                'i' => (char)0x17,
                'j' => (char)0x24,
                'k' => (char)0x25,
                'l' => (char)0x26,
                'm' => (char)0x32,
                'n' => (char)0x31,
                'o' => (char)0x18,
                'p' => (char)0x19,
                'q' => (char)0x10,
                'r' => (char)0x13,
                's' => (char)0x1F,
                't' => (char)0x14,
                'u' => (char)0x16,
                'v' => (char)0x2F,
                'w' => (char)0x11,
                'x' => (char)0x2D,
                'y' => (char)0x15,
                'z' => (char)0x2C,
                '0' => (char)0x0B,
                '1' => (char)0x02,
                '2' => (char)0x03,
                '3' => (char)0x04,
                '4' => (char)0x05,
                '5' => (char)0x06,
                '6' => (char)0x07,
                '7' => (char)0x08,
                '8' => (char)0x09,
                '9' => (char)0x0A,
                '!' => (char)0x02,
                '#' => (char)0x04,
                '$' => (char)0x05,
                '%' => (char)0x06,
                '^' => (char)0x07,
                '&' => (char)0x08,
                '*' => (char)0x09,
                '(' => (char)0x0A,
                ')' => (char)0x0B,
                '-' => (char)0x0C,
                '=' => (char)0x0D,
                ';' => (char)0x27,
                '\'' => (char)0x28,
                '/' => (char)0x35,
                '.' => (char)0x34,
                ',' => (char)0x33,
                '\n' => (char)0x1C,
                '+' => (char)0x0D,
                ':' => (char)0x27,
                '\"' => (char)0x28,
                '?' => (char)0x35,
                '<' => (char)0x33,
                '>' => (char)0x34,
                '[' => (char)0x1A,
                ']' => (char)0x1B,
                '{' => (char)0x1A,
                '}' => (char)0x1B,
                '\\' => (char)0x2B,
                '|' => (char)0x2B,
                '`' => (char)0x29,
                '~' => (char)0x29,
                '_' => (char)0x0C,
                '\t' => (char)0x39 // TAB
                ,
                _ => (char)0xFF
            };
        }

        // ReSharper disable once InconsistentNaming
        public bool GetCSHIFT(char letter)
        {
            return letter switch
            {
                '@' => true,
                'A' => true,
                'B' => true,
                'C' => true,
                'D' => true,
                'E' => true,
                'F' => true,
                'G' => true,
                'H' => true,
                'I' => true,
                'J' => true,
                'K' => true,
                'L' => true,
                'M' => true,
                'N' => true,
                'O' => true,
                'P' => true,
                'Q' => true,
                'R' => true,
                'S' => true,
                'T' => true,
                'U' => true,
                'V' => true,
                'W' => true,
                'X' => true,
                'Y' => true,
                'Z' => true,
                ' ' => false,
                'a' => false,
                'b' => false,
                'c' => false,
                'd' => false,
                'e' => false,
                'f' => false,
                'g' => false,
                'h' => false,
                'i' => false,
                'j' => false,
                'k' => false,
                'l' => false,
                'm' => false,
                'n' => false,
                'o' => false,
                'p' => false,
                'q' => false,
                'r' => false,
                's' => false,
                't' => false,
                'u' => false,
                'v' => false,
                'w' => false,
                'x' => false,
                'y' => false,
                'z' => false,
                '0' => false,
                '1' => false,
                '2' => false,
                '3' => false,
                '4' => false,
                '5' => false,
                '6' => false,
                '7' => false,
                '8' => false,
                '9' => false,
                '!' => true,
                '#' => true,
                '$' => true,
                '%' => true,
                '^' => true,
                '&' => true,
                '*' => true,
                '(' => true,
                ')' => true,
                '-' => false,
                '=' => false,
                ';' => false,
                '\'' => false,
                '/' => false,
                '.' => false,
                ',' => false,
                '\n' => false,
                '+' => true,
                ':' => true,
                '\"' => true,
                '?' => true,
                '<' => true,
                '>' => true,
                '[' => false,
                ']' => false,
                '{' => true,
                '}' => true,
                '\\' => false,
                '|' => true,
                '`' => false,
                '~' => true,
                '_' => true,
                '\t' => false, // TAB

                _ => false
            };
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <returns>TRUE when { [, ], \ }</returns>
        public bool GetLCNTRL(char letter)
        {
            return letter switch
            {
                '@' => false,
                'A' => false,
                'B' => false,
                'C' => false,
                'D' => false,
                'E' => false,
                'F' => false,
                'G' => false,
                'H' => false,
                'I' => false,
                'J' => false,
                'K' => false,
                'L' => false,
                'M' => false,
                'N' => false,
                'O' => false,
                'P' => false,
                'Q' => false,
                'R' => false,
                'S' => false,
                'T' => false,
                'U' => false,
                'V' => false,
                'W' => false,
                'X' => false,
                'Y' => false,
                'Z' => false,
                ' ' => false,
                'a' => false,
                'b' => false,
                'c' => false,
                'd' => false,
                'e' => false,
                'f' => false,
                'g' => false,
                'h' => false,
                'i' => false,
                'j' => false,
                'k' => false,
                'l' => false,
                'm' => false,
                'n' => false,
                'o' => false,
                'p' => false,
                'q' => false,
                'r' => false,
                's' => false,
                't' => false,
                'u' => false,
                'v' => false,
                'w' => false,
                'x' => false,
                'y' => false,
                'z' => false,
                '0' => false,
                '1' => false,
                '2' => false,
                '3' => false,
                '4' => false,
                '5' => false,
                '6' => false,
                '7' => false,
                '8' => false,
                '9' => false,
                '!' => false,
                '#' => false,
                '$' => false,
                '%' => false,
                '^' => false,
                '&' => false,
                '*' => false,
                '(' => false,
                ')' => false,
                '-' => false,
                '=' => false,
                ';' => false,
                '\'' => false,
                '/' => false,
                '.' => false,
                ',' => false,
                '\n' => false,
                '+' => false,
                ':' => false,
                '\"' => false,
                '?' => false,
                '<' => false,
                '>' => false,
                '[' => true,
                ']' => true,
                '{' => false,
                '}' => false,
                '\\' => true,
                '|' => false,
                '`' => false,
                '~' => false,
                '_' => false,
                '\t' => false, // TAB

                _ => false
            };
        }
    }
}
