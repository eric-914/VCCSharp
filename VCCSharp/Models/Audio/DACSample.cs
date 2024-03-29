﻿namespace VCCSharp.Models.Audio;

public class DACSample
{
    public class Channel
    {
        public int Out { get; set; }
        public int Last { get; set; }

        public void Sample(int pakSample, byte audioSample, byte singleBitSample)
        {
            var sample = pakSample + audioSample + singleBitSample;

            sample <<= 6;   //Convert to 16 bit values, for Max volume

            //Simulate a slow high pass filter
            if (sample == Last)
            {
                if (Out != 0)
                {
                    Out--;
                }
            }
            else
            {
                Out = Last = sample;
            }
        }
    }

    public Channel Left { get; } = new();
    public Channel Right { get; } = new();

    public int Sample => (Left.Out << 16) + Right.Out;
}
