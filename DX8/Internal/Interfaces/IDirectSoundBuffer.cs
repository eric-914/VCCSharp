// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

using System.Runtime.InteropServices;
using LPVOID = System.IntPtr;

namespace DX8.Internal.Interfaces
{
    /// <summary>
    /// Applications use the methods of the IDirectSoundBuffer interface to create DirectSoundBuffer objects and set up the environment.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(DxGuid.DirectSoundBuffer)]
    internal interface IDirectSoundBuffer
    {
        #region unused
        public long GetCaps();
        #endregion

        /// <summary>
        /// This method retrieves the current position of the play and write cursors in the sound buffer.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/mt708925(v=vs.85)"/>
        /// <param name="playCursor">Address of a variable to contain the current play position in the DirectSoundBuffer object. This position is an offset within the sound buffer and is specified in bytes. This parameter can be NULL if the current play position is not wanted.</param>
        /// <param name="writeCursor">Address of a variable to contain the current write position in the DirectSoundBuffer object. This position is an offset within the sound buffer and is specified in bytes. This parameter can be NULL if the current write position is not wanted.</param>
        /// <returns>
        /// If the method succeeds, the return value is DS_OK.
        /// If the method fails, the return value may be one of the following error values:
        /// DSERR_INVALIDPARAM | DSERR_PRIOLEVELNEEDED
        /// </returns>
        /// <remarks>The write cursor indicates the position at which it is safe to write new data to the buffer. The write cursor always leads the play cursor, typically by about 15 milliseconds' worth of audio data.</remarks>
        public long GetCurrentPosition(/*DWORD*/ ref uint playCursor, /*DWORD*/ ref uint writeCursor);

        #region unused
        public long GetFormat();
        public long GetVolume();
        public long GetPan();
        public long GetFrequency();
        public long GetStatus();
        public long Initialize();
        #endregion

        /// <summary>
        /// This method obtains a valid write pointer to the sound buffer's audio data.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/mt708932(v=vs.85)"/>
        /// <param name="offset">Offset, in bytes, from the start of the buffer to where the lock begins. This parameter is ignored if DSBLOCK_FROMWRITECURSOR is specified in the dwFlags parameter.</param>
        /// <param name="length">Size, in bytes, of the portion of the buffer to lock. Note that the sound buffer is conceptually circular.</param>
        /// <param name="sndPointer1">Address of a pointer to contain the first block of the sound buffer to be locked.</param>
        /// <param name="sndLength1">Address of a variable to contain the number of bytes pointed to by the lplpvAudioPtr1 parameter. If this value is less than the dwWriteBytes parameter, lplpvAudioPtr2 will point to a second block of sound data.</param>
        /// <param name="sndPointer2">Address of a pointer to contain the second block of the sound buffer to be locked. If the value of this parameter is NULL, the lplpvAudioPtr1 parameter points to the entire locked portion of the sound buffer.</param>
        /// <param name="sndLength2">Address of a variable to contain the number of bytes pointed to by the lplpvAudioPtr2 parameter. If lplpvAudioPtr2 is NULL, this value will be 0.</param>
        /// <param name="dwFlags">Flags modifying the lock event.</param>
        /// <returns>
        /// If the method succeeds, the return value is DS_OK.
        /// If the method fails, the return value may be one of the following error values:
        /// DSERR_BUFFERLOST | DSERR_INVALIDCALL | DSERR_INVALIDPARAM |DSERR_PRIOLEVELNEEDED
        /// </returns>
        /// <remarks>This method accepts an offset and a byte count, and returns two write pointers and their associated sizes.</remarks>
        public long Lock(/*DWORD*/ uint offset, /*DWORD*/ uint length, ref LPVOID sndPointer1, ref /*DWORD*/ uint sndLength1, ref LPVOID sndPointer2, ref /*DWORD*/ uint sndLength2, /*DWORD*/ uint dwFlags);

        /// <summary>
        /// This method causes the sound buffer to play from the current position.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/mt708933(v=vs.85)"/>
        /// <param name="dwReserved1">This parameter is reserved. Its value must be 0.</param>
        /// <param name="dwPriority">This parameter is reserved. Its value must be 0.</param>
        /// <param name="dwFlags">Flags specifying how to play the buffer.</param>
        /// <returns>
        /// If the method succeeds, the return value is DS_OK.
        /// If the method fails, the return value may be one of the following error values:
        /// DSERR_BUFFERLOST | DSERR_INVALIDCALL | DSERR_INVALIDPARAM | DSERR_PRIOLEVELNEEDED
        /// </returns>
        /// <remarks>This method will cause a secondary sound buffer to be mixed into the primary buffer and sent to the sound device. If this is the first buffer to play, it will implicitly create a primary buffer and start playing that buffer; the application need not explicitly direct the primary buffer to play.</remarks>
        public long Play(/*DWORD*/ uint dwReserved1, /*DWORD*/ uint dwPriority, /*DWORD*/ uint dwFlags);

        /// <summary>
        /// This method moves the current play position for secondary sound buffers.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/mt708935(v=vs.85)"/>
        /// <param name="position">New position, in bytes, from the beginning of the buffer that will be used when the sound buffer is played.</param>
        /// <returns>
        /// If the method succeeds, the return value is DS_OK.
        /// If the method fails, the return value may be one of the following error values:
        /// DSERR_INVALIDCALL | DSERR_INVALIDPARAM | DSERR_PRIOLEVELNEEDED
        /// </returns>
        /// <remarks>This method cannot be called on primary sound buffers.</remarks>
        public long SetCurrentPosition(/*DWORD*/ uint position);

        #region unused
        public long SetFormat();
        public long SetVolume();
        public long SetPan();
        public long SetFrequency();
        #endregion

        /// <summary>
        /// This method causes the sound buffer to stop playing.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/mt708940(v=vs.85)"/>
        /// <returns>
        /// If the method succeeds, the return value is DS_OK.
        /// If the method fails, the return value may be one of the following error values:
        /// DSERR_INVALIDPARAM | DSERR_PRIOLEVELNEEDED
        /// </returns>
        /// <remarks>For secondary sound buffers, Stop will set the current position of the buffer to the sample that follows the last sample played. This means that if the Play method is called on the buffer, it will continue playing where it left off.</remarks>
        public long Stop();

        /// <summary>
        /// This method releases a locked sound buffer.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/mt708941(v=vs.85)"/>
        /// <param name="sndPointer1">Address of the value retrieved in the lplpvAudioPtr1 parameter of the Lock method.</param>
        /// <param name="sndLength1">Number of bytes actually written to the lpvAudioPtr1 parameter. It should not exceed the number of bytes returned by the Lock method.</param>
        /// <param name="sndPointer2">Address of the value retrieved in the lplpvAudioPtr2 parameter of the Lock method.</param>
        /// <param name="sndLength2">Number of bytes actually written to the lpvAudioPtr2 parameter. It should not exceed the number of bytes returned by the Lock method.</param>
        /// <returns>
        /// If the method succeeds, the return value is DS_OK.
        /// If the method fails, the return value may be one of the following error values:
        /// DSERR_INVALIDCALL | DSERR_INVALIDPARAM | DSERR_PRIOLEVELNEEDED
        /// </returns>
        /// <remarks>An application must pass both pointers, lpvAudioPtr1 and lpvAudioPtr2, returned by the Lock method to ensure the correct pairing of Lock and Unlock. The second pointer is needed even if 0 bytes were written to the second pointer.</remarks>
        public long Unlock(LPVOID sndPointer1, /*DWORD*/ uint sndLength1, LPVOID sndPointer2, /*DWORD*/ uint sndLength2);

        #region unused
        public long Restore();
        #endregion
    }
}
