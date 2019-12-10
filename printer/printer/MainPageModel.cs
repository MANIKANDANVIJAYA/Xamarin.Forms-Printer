using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using FreshMvvm;
using Xamarin.Forms;

namespace printer
{
    public class MainPageModel : FreshBasePageModel
    {
        public override async void  Init(object initData)
        {
            base.Init(initData);
        }

        public Command printCommand
        {
            get
            {
                return new Command(async() => {

                  
                });
            }
        }
    }
}
