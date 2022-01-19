// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// The WAVEFORMATEX structure defines the format of waveform-audio data. Only format information common to all waveform-audio data formats is included in this structure. For formats that require additional information, this structure is included as the first member in another structure, along with the additional information.
    /// Formats that support more than two channels or sample sizes of more than 16 bits can be described in a WAVEFORMATEXTENSIBLE structure, which includes the WAVEFORMAT structure.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/mmeapi/ns-mmeapi-waveformatex"/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WAVEFORMATEX
    {
        /// <summary>
        /// Waveform-audio format type. Format tags are registered with Microsoft Corporation for many compression algorithms. A complete list of format tags can be found in the Mmreg.h header file. For one- or two-channel PCM data, this value should be WAVE_FORMAT_PCM. When this structure is included in a WAVEFORMATEXTENSIBLE structure, this value must be WAVE_FORMAT_EXTENSIBLE.
        /// </summary>
        public ushort wFormatTag;         /* format type */

        /// <summary>
        /// Number of channels in the waveform-audio data. Monaural data uses one channel and stereo data uses two channels.
        /// </summary>
        public ushort nChannels;          /* number of channels (i.e. mono, stereo...) */

        /// <summary>
        /// Sample rate, in samples per second (hertz). If wFormatTag is WAVE_FORMAT_PCM, then common values for nSamplesPerSec are 8.0 kHz, 11.025 kHz, 22.05 kHz, and 44.1 kHz. For non-PCM formats, this member must be computed according to the manufacturer's specification of the format tag.
        /// </summary>
        public uint nSamplesPerSec;       /* sample rate */

        /// <summary>
        /// Required average data-transfer rate, in bytes per second, for the format tag. If wFormatTag is WAVE_FORMAT_PCM, nAvgBytesPerSec should be equal to the product of nSamplesPerSec and nBlockAlign. For non-PCM formats, this member must be computed according to the manufacturer's specification of the format tag.
        /// </summary>
        public uint nAvgBytesPerSec;      /* for buffer estimation */

        /// <summary>
        /// Block alignment, in bytes. The block alignment is the minimum atomic unit of data for the wFormatTag format type. If wFormatTag is WAVE_FORMAT_PCM or WAVE_FORMAT_EXTENSIBLE, nBlockAlign must be equal to the product of nChannels and wBitsPerSample divided by 8 (bits per byte). For non-PCM formats, this member must be computed according to the manufacturer's specification of the format tag.
        /// </summary>
        public ushort nBlockAlign;        /* block size of data */

        /// <summary>
        /// Bits per sample for the wFormatTag format type. If wFormatTag is WAVE_FORMAT_PCM, then wBitsPerSample should be equal to 8 or 16. For non-PCM formats, this member must be set according to the manufacturer's specification of the format tag.
        /// </summary>
        public ushort wBitsPerSample;     /* number of bits per sample of mono data */

        /// <summary>
        /// Size, in bytes, of extra format information appended to the end of the WAVEFORMATEX structure. This information can be used by non-PCM formats to store extra attributes for the wFormatTag.
        /// </summary>
        public ushort cbSize;             /* the count in bytes of the size of extra information (after cbSize) */
    }
}
