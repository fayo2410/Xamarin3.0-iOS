﻿using System;

using UIKit;


namespace PhoneApp1
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var TranslatedNumber = string.Empty;
            // Perform any additional setup after loading the view, typically from a nib.

            TranslateButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                var Translator = new PhoneTranslator();
                TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(TranslatedNumber))
                {
                    //No hay numero a llamar
                    CallButton.SetTitle("Llamar", UIControlState.Normal);
                    CallButton.Enabled = false;
                }
                else
                {
                    //Hay un posible numero telefonico a llamar
                    CallButton.SetTitle($"llamar al {TranslatedNumber}", UIControlState.Normal);
                    CallButton.Enabled = true;
                }
            };

            CallButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                var URL = new Foundation.NSUrl($"tel:{TranslatedNumber}");
                //Utilizar el manejador de URL con el prefijo tel: para invocar
                //a la aplicacion Phone de Apple, de lo contrario mostrar un dialogo de alerta.
                if(!UIApplication.SharedApplication.OpenUrl(URL))
                {
                    var Alert = UIAlertController.Create("No soportado",
                        "El esquema 'tel:' no es soportado en este dispositivo",
                        UIAlertControllerStyle.Alert);
                    Alert.AddAction(UIAlertAction.Create("Ok",
                        UIAlertActionStyle.Default, null));
                    PresentViewController(Alert, true, null);
                }
            };


        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void VerifyButton_TouchUpInside(UIButton sender)
        {
            Validate();
        }

        async void Validate()
        {
            var Client = new SALLab05.ServiceClient();
            var Result = await Client.ValidateAsync("correo", "password", this);

            var Alert = UIAlertController.Create("Resultado",
                $"{Result.Status}\n{Result.FullName}\n{Result.Token}", UIAlertControllerStyle.Alert);
            Alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));

            PresentViewController(Alert, true, null);
        }
    }
}
