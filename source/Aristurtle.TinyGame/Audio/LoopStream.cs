using NAudio.Wave;

namespace Aristurtle.TinyGame;

/// <summary>
///     A <see cref="WaveStream"/> implementation that loops the playback of
///     the audio file.
/// </summary>
public class LoopStream : WaveStream
{
    //  The underlying source stream.
    private WaveStream _source;

    /// <summary>
    ///     Whether looping is enabled or not.
    /// </summary>
    /// <value></value>
    public bool EnableLooping { get; set; }

    /// <summary>
    ///     The <see cref="WaveFormat"/> of the underlying source stream.
    /// </summary>
    public override WaveFormat WaveFormat => _source.WaveFormat;

    /// <summary>
    ///     The length of the underlying source stream, in bytes.
    /// </summary>
    public override long Length => _source.Length;

    /// <summary>
    ///     THe current position in the underlying source stream.
    /// </summary>
    public override long Position 
    {
        get => _source.Position;
        set => _source.Position = value;
    }

    /// <summary>
    ///     Creates a new <see cref="LoopStream"/> class instance.
    /// </summary>
    /// <param name="source">
    ///     The source stream to use as the underlying stream.
    /// </param>
    public LoopStream(WaveStream source)
    {
        _source = source;
        EnableLooping = true;
    }

    /// <summary>
    ///     Reads the specified <paramref name="count"/> of bytes from the
    ///     underlying source into the <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">
    ///     The buffer to read the data into.
    /// </param>
    /// <param name="offset">
    ///     The offset fromt he current position of the underlying stream to
    ///     start reading the data.
    /// </param>
    /// <param name="count">
    ///     The total number of bytes to read.
    /// </param>
    /// <returns>
    ///     The total number of bytes read.
    /// </returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        int totalRead = 0;

        while (totalRead < count)
        {
            int bytesRead = _source.Read(buffer, offset + totalRead, count - totalRead);

            if (bytesRead == 0)
            {
                if (_source.Position == 0 || !EnableLooping)
                {
                    break;
                }

                _source.Position = 0;
            }
            totalRead += bytesRead;
        }

        return totalRead;
    }
}