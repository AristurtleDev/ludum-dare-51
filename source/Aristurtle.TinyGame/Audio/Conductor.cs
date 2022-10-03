/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System;
using System.IO;
using Aristurtle.TinyEngine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using NAudio.Wave;
namespace Aristurtle.TinyGame;


public class Conductor
{
    //  The output device used by NAudio to play the song
    private WaveOutEvent _outputDevice;

    //  The audio file used by NAudio to play the song
    private AudioFileReader _audioFile;

    private SoundEffect _badEffect;
    private SoundEffect _goodEffect;

    //  The time fo the last beat detected.
    private float _lastBeat;

    public int Volume { get; set; } = 50;
    private float _actualVolume;

    /// <summary>
    ///     Event handler for when a beat occurs.
    /// </summary>
    public EventHandler<BeatEventArgs> OnBeat;

    public EventHandler<EventArgs> OnMusicStopped;

    /// <summary>
    ///     Data about the song that is being played such as Bpm, Repet, and
    ///     offset.
    /// </summary>
    public SongModel SongModel { get; set; }

    /// <summary>
    ///     Time duration of a beat, calculated from the Bpm
    /// </summary>
    public float Crochet { get; set; }

    /// <summary>
    ///     The position of the song.
    /// </summary>
    public float SongPosition { get; set; }

    /// <summary>
    ///     The total number of beats that have occured for the currently
    ///     playing song.
    /// </summary>
    public int BeatCount { get; set; }

    /// <summary>
    ///     Creates a new Conductor instance
    /// </summary>
    public Conductor()
    {
        SetActualVolume();
    }

    public void LoadEffects()
    {
        _badEffect = Engine.GlobalContent.Load<SoundEffect>("audio/bad");
        _goodEffect = Engine.GlobalContent.Load<SoundEffect>("audio/good");
    }

    public void PlaySong(SongTitle title)
    {
        SongModel = title.GetSongModel();

        //  Initialize the output device if its null
        if (_outputDevice == null)
        {
            _outputDevice = new WaveOutEvent();
        }

        //  Iniitlize the audio file if its null
        if (_audioFile == null)
        {
            _audioFile = new AudioFileReader(Path.Combine(FileUtilities.AssemblyDirectory, "Content", SongModel.Location));
        }

        LoopStream ls = new(_audioFile);

        _outputDevice.Volume = (float)Math.Pow(Volume / 100.0f, Math.E);

        //  Initilize the output device with the audio file
        _outputDevice.Init(ls);

        //  Calculate the crochet (duration of time for a quater note)
        Crochet = (60 / SongModel.Bpm) * 1.0f;

        //  Start the song
        _outputDevice.Play();
    }

    public void Update()
    {
        //  Check if playback has stopped, if so, execute event
        if (_outputDevice.PlaybackState == PlaybackState.Stopped)
        {
            OnMusicStopped?.Invoke(this, EventArgs.Empty);
        }

        //  Update the song position
        SongPosition = (float)(_outputDevice.GetPosition() * 1d / _outputDevice.OutputWaveFormat.AverageBytesPerSecond);
        //  Check if a new beat has occured
        if (SongPosition > _lastBeat + Crochet)
        {
            //  Increment the time the last beat occured
            _lastBeat += Crochet;

            //  Increment the beat count
            BeatCount++;

            //  Trigger event
            Beat(new BeatEventArgs() { SongPosition = SongPosition, BeatCount = BeatCount });
        }

        if (_audioFile.Position >= _audioFile.Length)
        {
            _audioFile.Position = 0;
        }
    }

    private void Beat(BeatEventArgs e)
    {
        OnBeat?.Invoke(this, e);
    }

    public void StopMusic()
    {
        _outputDevice.Stop();
        _outputDevice.Dispose();
        _audioFile.Dispose();
        _outputDevice = null;
        _audioFile = null;
    }

    public float TimeLeft()
    {
        return SongPosition / (float)_audioFile.TotalTime.TotalSeconds;
    }

    public void IncreaseVolume()
    {
        Volume = Math.Min(Volume + 10, 100);

        SetActualVolume();
        _outputDevice.Volume = _actualVolume;
        MediaPlayer.Volume = _actualVolume;
    }

    public void DecreaseVolume()
    {
        Volume = Math.Max(Volume - 10, 0);
        SetActualVolume();
        _outputDevice.Volume = _actualVolume;
        MediaPlayer.Volume = _actualVolume;
    }

    private void SetActualVolume()
    {
        _actualVolume = Math.Max((float)Math.Pow(Volume / 100.0f, Math.E), 0.0f);
    }

    public void PlaySoundEffect(SoundEffectName effect)
    {
        if (effect == SoundEffectName.Bad)
        {
            _badEffect.Play(Volume / 100.0f, 0.0f, 0.0f);
        }
        else
        {
            _goodEffect.Play(Volume / 100.0f, 0.0f, 0.0f);
        }
    }
}