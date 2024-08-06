using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TatehamaATS.Exceptions;
using static System.Net.Mime.MediaTypeNames;

namespace TatehamaATS
{
    internal class Retsuban
    {
        internal Label RetsubanText;
        internal Label CarText;
        internal int NowSelect;
        SoundPlayer set_trainnum = new SoundPlayer(Properties.Resources.set_trainnum);
        SoundPlayer set_trainsetlen = new SoundPlayer(Properties.Resources.set_trainsetlen);
        SoundPlayer set_complete = new SoundPlayer(Properties.Resources.set_complete);
        SoundPlayer beep1 = new SoundPlayer(Properties.Resources.beep1);
        SoundPlayer beep2 = new SoundPlayer(Properties.Resources.beep2);
        SoundPlayer beep3 = new SoundPlayer(Properties.Resources.beep3);

        public Retsuban(Label RetsubanText, Label CarText)
        {
            this.RetsubanText = RetsubanText;
            this.CarText = CarText;
            MainWindow.retsuban?.Init();
        }

        public void addText(string text)
        {
            PlaySound(beep1);
            try
            {
                if (NowSelect == 1)
                {
                    RetsubanText.Text += text;
                }
                if (NowSelect == 2)
                {
                    CarText.Text += text;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Enter()
        {
            try
            {
                PlaySound(beep2);
                if (NowSelect == 1)
                {
                    try
                    {
                        TrainState.TrainDiaName = RetsubanText.Text;
                        NowSelect++;
                        PlayLoopingSound(set_trainsetlen);
                    }
                    catch (Exception ex)
                    {
                        throw new RetsubanAbnormal(3, "Retsuban.cs@Enter", ex);
                    }
                }
                else if (NowSelect == 2)
                {
                    try
                    {
                        NowSelect++;
                        var car = int.Parse(CarText.Text);
                        if (2 <= car && car <= 10)
                        {
                            TrainState.TrainCar = car;
                            TrainState.TrainLength = (int)(car * 20.5 - 3 + 1);
                            PlaySound(set_complete);
                        }
                        else
                        {
                            throw new CarAbnormal(3, "2-10範囲外Retsuban.cs@Enter");
                        }
                        TrainState.chengeDiaName = true;
                        MainWindow.transfer.SetRetsuban();
                        MainWindow.inspectionRecord.RetsubanReset = true;
                    }
                    catch (Exception ex)
                    {
                        throw new CarAbnormal(3, "Retsuban.cs@Enter", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Del()
        {
            PlaySound(beep1);
            try
            {
                if (NowSelect == 1)
                {
                    if (RetsubanText.Text != "")
                    {
                        RetsubanText.Text = RetsubanText.Text.Substring(0, RetsubanText.Text.Length - 1);
                    }
                }
                if (NowSelect == 2)
                {
                    if (RetsubanText.Text != "")
                    {
                        CarText.Text = CarText.Text.Substring(0, CarText.Text.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void AllDel()
        {
            PlaySound(beep2);
            if (NowSelect == 1)
            {
                RetsubanText.Text = "";
                PlayLoopingSound(set_trainnum);
            }
            if (NowSelect == 2)
            {
                CarText.Text = "";
                PlayLoopingSound(set_trainsetlen);
            }
        }

        public void Back()
        {
            if (NowSelect > 1)
            {
                NowSelect--;
            }
            AllDel();
        }

        private void PlayLoopingSound(SoundPlayer player)
        {
            set_trainnum.Stop();
            set_trainsetlen.Stop();
            player.PlayLooping();
        }

        private void PlaySound(SoundPlayer player)
        {
            player.Play();
        }

        private void HandleException(Exception ex)
        {
            MainWindow.inspectionRecord.AddException(ex);
        }


        public void Init()
        {
            NowSelect = 0;
            try
            {
                RetsubanText.Invoke((MethodInvoker)(() =>
                {
                    RetsubanText.Text = "";
                    CarText.Text = "";
                }));
                PlaySound(beep3);
            }
            catch(InvalidOperationException ex)
            {

            }
        }

        public void Load()
        {
            NowSelect = 1;
            try
            {
                RetsubanText.Invoke((MethodInvoker)(() =>
                {
                    RetsubanText.Text = "";
                    CarText.Text = "";
                }));
                PlayLoopingSound(set_trainnum);
            }
            catch (InvalidOperationException ex)
            {

            }
        }
    }
}