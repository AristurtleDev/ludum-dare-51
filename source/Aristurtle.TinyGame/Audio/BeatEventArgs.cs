using System;

namespace Aristurtle.TinyGame;

/// <summary>
///     Arguments that are supplied when a Beat event is triggered.
/// </summary>
public class BeatEventArgs : EventArgs
{
    /// <summary>
    ///     The position within the current audio stream of the song that is
    ///     playing.
    /// </summary>
    public float SongPosition { get; set; }

    /// <summary>
    ///     The total number of beats that have ocured.
    /// </summary>
    /// <value></value>
    public int BeatCount { get; set; }

    /// <summary>
    ///     The crochet of the current song that is playing. 
    /// </summary>
    /// <remarks>
    ///     The crochet is the amount of time for a quater note to occur.
    /// </remarks>
    public float Crochet { get; set; }
}
