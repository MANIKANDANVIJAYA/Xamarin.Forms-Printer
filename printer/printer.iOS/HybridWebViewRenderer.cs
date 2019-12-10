using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using printer;
using printer.iOS;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace printer.iOS
{
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                userController = new WKUserContentController();
                var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                userController.AddUserScript(script);
                userController.AddScriptMessageHandler(this, "invokeAction");

                var config = new WKWebViewConfiguration { UserContentController = userController };
                var webView = new WKWebView(Frame, config);
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {

            Element.InvokeAction(message.Body.ToString());
            try
            {
                var printInfo = UIPrintInfo.PrintInfo;
                printInfo.Duplex = UIPrintInfoDuplex.LongEdge;
                printInfo.OutputType = UIPrintInfoOutputType.General;
                printInfo.JobName = "";

                var printer = UIPrintInteractionController.SharedPrintController;
                printer.PrintInfo = printInfo;

                //var data = NSData.FromUrl(url);

                var webView = new UIWebView();
                webView.LoadRequest(NSUrlRequest.FromUrl(new NSUrl(Element.Uri)));

                var tsc = new TaskCompletionSource<bool>();

                EventHandler loadFinished = (s, e) => {
                    tsc.SetResult(true);
                };

                webView.LoadFinished += loadFinished;

                webView.LoadFinished -= loadFinished;

                printer.PrintFormatter = webView.ViewPrintFormatter;

                printer.ShowsPageRange = true;

                printer.Present(true, (handler, completed, err) => {
                    if (!completed && err != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to print");
                    }
                });
            }
            catch (Exception ex) 
            {

            }
        }
    }
 }
