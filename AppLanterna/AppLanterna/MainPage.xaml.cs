﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Battery;
using Xamarin.Essentials;
namespace AppLanterna
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;
        public MainPage()
        {
            InitializeComponent();
            btnOnOff.Source = ImageSource.FromResource("AppLanterna.Image.off.png");
            Carrega_Informacoes_Bateria();
        }

        private async void Carrega_Informacoes_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                } else 
                {
                    lbl_bateria_fraca.Text = "As informacoes sobre a bateria estao indisponiveis :( ";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n ", ex.Message, "OK");
            }
        }
        
        private async void Mudanca_Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_porcentagem_restante.Text = e.RemainingChargePercent.ToString() + "%";
                if (e.IsLow)
                {
                    lbl_bateria_fraca.Text = "Atencao! A bateria esta fraca!";
                }
                else
                {
                    lbl_bateria_fraca.Text = "";
                }

                switch (e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando";
                        break;
                    
                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Descarregando";
                        break;
                    
                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregada";
                        break;
                    
                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem Carregar";
                        break;
                    
                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido";
                        break;
                }

                switch (e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte_carregamento.Text = "Carregador";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte_carregamento.Text = "Bateria";
                        break;
                    case Plugin.Battery.Abstractions.Usb:
                        lbl_fonte_carregamento.Text = "USB";
                        break;
                    case Plugin.Battery.Abstractions.Wireless:
                        lbl_fonte_carregamento.Text = "Sem Fio";
                        break;
                    case Plugin.Battery.Abstractions.Other:
                        lbl_fonte_carregamento.Text = "Desconheido";
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n ", ex.Message, "OK");
            }
        }
        private async void btnOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!lanterna_ligada)
                {
                    lanterna_ligada = true;

                    btnOnOff.Source = ImageSource.FromResource("AppLanterna.Image.on.png");
                    
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOnAsync();
                }
                else
                {
                    lanterna_ligada = false;

                    btnOnOff.Source = ImageSource.FromResource("AppLanterna.Image.off.png");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOffAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n ", ex.Message, "OK")
            }
        }
    }
}
