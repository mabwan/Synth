using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace Synth
{
    public partial class Synth : Form
    {
        private const int SampleRate = 44100;
        private const short BitsPerSample = 16;
        public Synth()
        {
            InitializeComponent();
        }

        private void Synth_KeyDown(object sender, KeyEventArgs e)
        {
            Random random = new Random();
            short[] wave = new short[SampleRate];
            byte[] binaryWave = new byte[SampleRate * sizeof(short)];
            float frequency;
            switch(e.KeyCode)
            {
                case Keys.Z:
                    frequency = 65.4f;
                    break;
                case Keys.X:
                    frequency = 138.59f;
                    break;
                case Keys.C:
                    frequency = 261.62f;
                    break;
                case Keys.V:
                    frequency = 523.25f;
                    break;
                case Keys.B:
                    frequency = 1046.5f;
                    break;
                case Keys.N:
                    frequency = 2093f;
                    break;
                case Keys.M:
                    frequency = 4186.01f;
                    break;
                default:
                    return;
            }
            foreach (Oscillator oscillator in this.Controls.OfType<Oscillator>())
            {
                int samplesPerWaveLength = (int)(SampleRate / frequency);
                short ampStep = (short)((short.MaxValue * 2) / samplesPerWaveLength);
                short tempSample;
                switch (oscillator.WaveForm)
                {
                    case WaveForm.Sine:
                        for (int i = 0; i < SampleRate; i++)
                        {
                            wave[i] = Convert.ToInt16(short.MaxValue * Math.Sin(((Math.PI * 2 * frequency) / SampleRate) * i));
                        }
                        break;
                    case WaveForm.Saw:
                        for (int i = 0; i < SampleRate; i++)
                        {
                            tempSample = -short.MaxValue;
                            for (int j = 0; j < samplesPerWaveLength && i < SampleRate; j++)
                            {
                                tempSample += ampStep;
                                wave[i++] = Convert.ToInt16(tempSample);
                            }
                            i--;
                        }
                        break;
                    case WaveForm.Square:
                        for (int i = 0; i < SampleRate; i++)
                        {
                            wave[i] = Convert.ToInt16(short.MaxValue * Math.Sign(Math.Sin((Math.PI * 2 * frequency) / SampleRate * i)));
                        }
                        break;
                    case WaveForm.Triangle:             
                            tempSample = -short.MaxValue;
                            for (int i = 0; i < SampleRate; i++)
                            {
                                if (Math.Abs(tempSample + ampStep) > short.MaxValue)
                                {
                                    ampStep = (short)-ampStep;
                                }
                                tempSample += ampStep;
                                wave[i] = Convert.ToInt16(tempSample);
                        }
                        
                        break;

                    case WaveForm.Noise:
                        for (int i = 0; i < SampleRate; i++)
                        {
                            wave[i] = (short)random.Next(-short.MaxValue, short.MaxValue);
                        }
                        break;
                }
                Buffer.BlockCopy(wave, 0, binaryWave, 0, wave.Length * sizeof(short));
                using (MemoryStream memoryStream = new MemoryStream())
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    short blockAlign = BitsPerSample / 8;
                    int subChunkTwoSize = SampleRate * blockAlign;
                    binaryWriter.Write(new[] { 'R', 'I', 'F', 'F' });
                    binaryWriter.Write(36 + subChunkTwoSize);
                    binaryWriter.Write(new[] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
                    binaryWriter.Write(16);
                    binaryWriter.Write((short)1);
                    binaryWriter.Write((short)1);
                    binaryWriter.Write(SampleRate);
                    binaryWriter.Write(SampleRate * blockAlign);
                    binaryWriter.Write(blockAlign);
                    binaryWriter.Write(BitsPerSample);
                    binaryWriter.Write(new[] { 'd', 'a', 't', 'a' });
                    binaryWriter.Write(subChunkTwoSize);
                    binaryWriter.Write(binaryWave);
                    memoryStream.Position = 0;
                    new SoundPlayer(memoryStream).Play();
                }
            }
        }

    }
    public enum WaveForm
    {
        Sine, Saw, Square, Triangle, Noise
    }
}
