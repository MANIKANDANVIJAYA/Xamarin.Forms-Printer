using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace printer.Droid
{
    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;
        Context _context;

        public JSBridge(HybridWebViewRenderer hybridRenderer)
        {
             hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
             HybridWebView hybridWebView = new HybridWebView();
             bool value= hybridWebViewRenderer.TryGetTarget(out HybridWebViewRenderer hybrid);
             string uri = hybrid.Element.Uri;
        }
        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            try
            {
                HybridWebViewRenderer hybridRenderer;
                bool value = hybridWebViewRenderer.TryGetTarget(out HybridWebViewRenderer hybrid);
                string uri = hybrid.Element.Uri;
                OnElementChanged(hybrid.Control);
            }
            catch(Exception ex)
            {

            }
        }


        public List<PrintJob> mPrintJobs = new List<PrintJob>();

        protected void OnElementChanged(Android.Webkit.WebView webView)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var printManager = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);
                    PrintDocumentAdapter printAdapter = webView.CreatePrintDocumentAdapter("IDCARD");
                    PrintJob printJob = printManager.Print("IDCARD", printAdapter, new PrintAttributes.Builder().Build());
                    // Save the job object for later status checking
                    mPrintJobs.Add(printJob);
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
      }
    }